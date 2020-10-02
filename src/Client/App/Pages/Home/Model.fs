module Pages.Home

open Shared

type WsSender = WebSocketClientMessage -> unit
type BroadcastMode = ViaWebSocket | ViaHTTP
type ConnectionState =
    | DisconnectedFromServer | ConnectedToServer of WsSender | Connecting | Reconnecting
    member this.IsConnected =
        match this with
        | ConnectedToServer _ -> true
        | DisconnectedFromServer | Connecting | Reconnecting -> false

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

type ViewProps =
    { Model : Model
      Dispatch : Msg -> unit }