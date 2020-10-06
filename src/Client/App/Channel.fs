module App.Channel

open Shared

type WsSender = WebSocketClientMessage -> unit

type BroadcastMode = ViaWebSocket | ViaHTTP

type ConnectionState =
    | DisconnectedFromServer | ConnectedToServer of WsSender | Connecting | Reconnecting
    member this.IsConnected =
        match this with
        | ConnectedToServer _ -> true
        | DisconnectedFromServer | Connecting | Reconnecting -> false