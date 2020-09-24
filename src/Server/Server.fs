module Server

open FSharp.Control.Tasks.V2
open Saturn
open Giraffe
open Microsoft.Extensions.Logging
open Shared
open System

module Database =
    open LiteDB
    open LiteDB.FSharp

    type Storage () = 
        let database = 
            let mapper = FSharpBsonMapper()
            let connStr = "Filename=Messages.db;mode=Exclusive"
            new LiteDatabase (connStr, mapper)
        let messages = database.GetCollection<Message> "messages"

        member _.GetMessages () = 
            messages.FindAll () 
            |> List.ofSeq 
            |> List.sortByDescending (fun m -> m.Time)

        member _.AddMessage (message : Message) =
            messages.Insert message |> ignore
            Ok ()

module Channel = 
    open Thoth.Json.Net
    open Database

    /// Sends a message to a specific client by their socket ID.
    let sendMessage (hub : Channels.ISocketHub) socketId (payload : WebSocketServerMessage) = task {
        let payload = Encode.Auto.toString(0, payload)
        do! hub.SendMessageToClient "/channel" socketId "" payload
    }

    /// Sends a message to all connected clients.
    let broadcastMessage (hub:Channels.ISocketHub) (payload:WebSocketServerMessage) = task {
        let payload = Encode.Auto.toString(0, payload)
        do! hub.SendMessageToClients "/channel" "" payload
    }

    /// Sets up the channel to listen to clients.
    let channel (db : Storage) = channel {
        join (fun ctx clientInfo ->
            task {
                ctx.GetLogger().LogInformation("Client has connected. They've been assigned socket Id: {socketId}", clientInfo.SocketId)                
                return Channels.Ok
            })
        handle "" (fun ctx clientInfo message ->
            task {
                let hub = ctx.GetService<Channels.ISocketHub>()
                let message = message.Payload |> string |> Decode.Auto.unsafeFromString<WebSocketClientMessage>

                match message with
                | TextMessage message ->
                    let text = sprintf "Websocket message: %s" message
                    let message = { Id = Guid.NewGuid(); Time = System.DateTime.UtcNow; Text = text }
                    db.AddMessage (message) |> ignore
                    do! broadcastMessage hub (BroadcastMessage message)
                | GetMessages _ ->
                    let messages = db.GetMessages ()
                    do! sendMessage hub clientInfo.SocketId (BroadcastMessages messages)
            })
    }

let webApp = router {
    post "/api/broadcast" (fun next ctx ->
        task {
            let! message = ctx.BindModelAsync()
            let hub = ctx.GetService<Channels.ISocketHub>()
            let message = sprintf "HTTP message: %O" message
            do! Channel.broadcastMessage hub (BroadcastMessage { Id = Guid.NewGuid(); Time = System.DateTime.UtcNow; Text = message })
            return! next ctx
        })
}

let app =
    let db = Database.Storage ()
    application {
        url "http://0.0.0.0:8085"
        use_router webApp
        memory_cache
        use_static "public"
        use_json_serializer(Thoth.Json.Giraffe.ThothSerializer())
        use_gzip
        add_channel "/channel" (Channel.channel db)
    }

run app
