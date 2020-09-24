module Index

open Elmish
open Fable.React
open Thoth.Json
open Thoth.Fetch
open Fulma
open Browser.Types
open Shared
open System

/// Status of the websocket.
type WsSender = WebSocketClientMessage -> unit
type BroadcastMode = ViaWebSocket | ViaHTTP
type ConnectionState =
    | DisconnectedFromServer | ConnectedToServer of WsSender | Connecting
    member this.IsConnected =
        match this with
        | ConnectedToServer _ -> true
        | DisconnectedFromServer | Connecting -> false

type Model =
    { MessageToSend : string
      ReceivedMessages : Message list
      ConnectionState : ConnectionState }

type Msg =
    | ReceivedFromServer of WebSocketServerMessage
    | ConnectionChange of ConnectionState
    | MessageChanged of string
    | Broadcast of BroadcastMode * string
    | SyncState

module Channel = 
    open Browser.WebSocket

    let inline decode<'a> x = x |> unbox<string> |> Thoth.Json.Decode.Auto.unsafeFromString<'a>
    let buildWsSender (ws:WebSocket) : WsSender =
        fun (message:WebSocketClientMessage) ->
            let message = { Topic = ""; Ref = ""; Payload = message }
            let message = Thoth.Json.Encode.Auto.toString(0, message)
            ws.send message

    let subscription _ =
        let sub dispatch =
            /// Handles push messages from the server and relays them into Elmish messages.
            let onWebSocketMessage (msg:MessageEvent) =
                let msg = msg.data |> decode<{| Payload : string |}>
                msg.Payload
                |> decode<WebSocketServerMessage>
                |> ReceivedFromServer
                |> dispatch

            /// Continually tries to connect to the server websocket.
            let rec connect () =
                let ws = WebSocket.Create "ws://localhost:8085/channel"
                ws.onmessage <- onWebSocketMessage
                ws.onopen <- (fun ev ->
                    dispatch (ConnectionChange (ConnectedToServer (buildWsSender ws)))
                    printfn "WebSocket opened")
                ws.onclose <- (fun ev ->
                    dispatch (ConnectionChange DisconnectedFromServer)
                    printfn "WebSocket closed. Retrying connection"
                    promise {
                        do! Promise.sleep 2000
                        dispatch (ConnectionChange Connecting)
                        connect() })

            connect()

        Cmd.ofSub sub

let init () =
    { MessageToSend = null
      ConnectionState = DisconnectedFromServer
      ReceivedMessages = [] }, Cmd.none

let update msg model : Model * Cmd<Msg> =
    match msg with
    | MessageChanged msg ->
        { model with MessageToSend = msg }, Cmd.none
    | ConnectionChange status ->
        let model = { model with ConnectionState = status }
        match status with
        | ConnectedToServer _ ->
            model, Cmd.ofMsg SyncState
        | _ ->
            model, Cmd.none
    | ReceivedFromServer message ->
        match message with
        | BroadcastMessage msg -> 
            { model with ReceivedMessages = msg :: model.ReceivedMessages }, Cmd.none
        | BroadcastMessages msgs ->
            { model with ReceivedMessages = msgs }, Cmd.none
    | Broadcast (ViaWebSocket, msg) ->
        match model.ConnectionState with
        | ConnectedToServer sender -> sender (TextMessage msg)
        | _ -> ()
        model, Cmd.none
    | SyncState ->
        match model.ConnectionState with
        | ConnectedToServer sender -> sender (GetMessages 1)
        | _ -> ()
        model, Cmd.none
    | Broadcast (ViaHTTP, msg) ->
        let post = Fetch.post("/api/broadcast", msg)
        model, Cmd.OfPromise.result post

module ViewParts =
    let drawStatus connectionState =
        Tag.tag [
            Tag.Color
                (match connectionState with
                 | DisconnectedFromServer -> Color.IsDanger
                 | Connecting -> Color.IsWarning
                 | ConnectedToServer _ -> Color.IsSuccess)
        ] [
            match connectionState with
            | DisconnectedFromServer -> str "Disconnected from server"
            | Connecting -> str "Connecting..."
            | ConnectedToServer _ -> str "Connected to server"
        ]

let view (model : Model) (dispatch : Msg -> unit) =
    div [] [
        Navbar.navbar [ Navbar.Color IsPrimary ] [
            Navbar.Item.div [ ] [
                Heading.h2 [ ] [ str "SAFE Channels Template" ]
            ]
        ]
        Container.container [] [
            Content.content [ Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ] [
                Heading.h3 [] [ str "Send a message!" ]
                Input.text [ Input.OnChange(fun e -> dispatch(MessageChanged e.Value)) ]
            ]
            Columns.columns [] [
                for broadcastMethod in [ ViaHTTP; ViaWebSocket ] do
                    Column.column [] [
                        Button.button
                            [ Button.IsFullWidth
                              Button.Color IsPrimary
                              Button.Disabled (String.IsNullOrEmpty model.MessageToSend || not model.ConnectionState.IsConnected)
                              Button.OnClick (fun _ -> dispatch (Broadcast (broadcastMethod, model.MessageToSend))) ]
                            [ str (sprintf "Click to broadcast %O!" broadcastMethod) ]
                    ]
            ]

            ViewParts.drawStatus model.ConnectionState

            match model.ReceivedMessages with
            | [] ->
                ()
            | messages ->
                Table.table [] [
                    thead [] [
                        tr [] [
                            td [] [ str "Time" ]
                            td [] [ str "Message" ]
                        ]
                    ]
                    tbody [][
                        for message in messages do
                            tr [] [
                                td [] [ str (sprintf "%O" message.Time) ]
                                td [] [ str message.Text ]
                            ]
                    ]
                    tfoot [] []
                ]
        ]
        Footer.footer [ ] [
            Content.content [ Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ] [
                str "Demo by Compositional IT"
            ]
        ]
    ]

module CustomEncoders =

    let inline addDummyCoder<'b> extrasIn =
        let typeName = string typeof<'b>
        let simpleEncoder(_ : 'b) = Encode.string (sprintf "%s function" typeName)
        let simpleDecoder = Decode.fail (sprintf "Decoding is not supported for %s type" typeName)
        extrasIn |> Extra.withCustom simpleEncoder simpleDecoder
        
    let inline buildExtras<'a> extraCoders =
        let myEncoder:Encoder<'a> = Encode.Auto.generateEncoder(extra = extraCoders)
        let myDecoder:Decoder<'a> = Decode.Auto.generateDecoder(extra = extraCoders)
        (myEncoder, myDecoder)

let extras : Encoder<Model> * (string -> obj -> Result<Model,DecoderError>) = 
    Extra.empty
    |> CustomEncoders.addDummyCoder<WsSender>
    |> CustomEncoders.buildExtras<Model>
