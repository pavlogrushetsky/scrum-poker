module App.Model

open Shared

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