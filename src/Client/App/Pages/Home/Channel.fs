module Pages.Home.Channel

open Elmish
open Fable.Import
open Browser.Types
open Browser.WebSocket

open Shared

open Pages.Home

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