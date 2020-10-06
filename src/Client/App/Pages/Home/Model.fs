module Pages.Home

open App.Channel
open Shared

type Model =
    { MessageToSend : string
      ReceivedMessages : Message list
      ConnectionState : ConnectionState }

type Msg =
    | ConnectionChanged of ConnectionState
    | MessageChanged of string
    | Broadcast of BroadcastMode * string
    | MessageReceived of Message
    | MessagesReceived of Message list
    | SyncState

type ViewProps =
    { Model : Model
      Dispatch : Msg -> unit }