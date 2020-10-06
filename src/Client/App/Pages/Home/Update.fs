module Pages.Home.Update

open Elmish
open Thoth.Fetch

open Shared
open App.Channel
open Pages.Home

let init connectionState =
    { MessageToSend = null
      ConnectionState = connectionState
      ReceivedMessages = [] }, Cmd.ofMsg SyncState

let update msg model : Model * Cmd<Msg> =
    match msg with
    | ConnectionChanged status ->
        let model = { model with ConnectionState = status }
        match status with
        | ConnectedToServer _ ->
            model, Cmd.ofMsg SyncState
        | _ ->
            model, Cmd.none
    | MessageChanged message ->
        { model with MessageToSend = message }, Cmd.none
    | MessageReceived message ->
        { model with ReceivedMessages = message :: model.ReceivedMessages }, Cmd.none
    | MessagesReceived messages ->
        { model with ReceivedMessages = messages }, Cmd.none
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