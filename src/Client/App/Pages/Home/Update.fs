module Pages.Home.Update

open Elmish
open Thoth.Fetch

open Shared

open Pages.Home

let init () =
    { MessageToSend = null
      ConnectionState = DisconnectedFromServer
      ReceivedMessages = [] }, Cmd.ofMsg (ConnectionChange Reconnecting)

let update msg model : Model * Cmd<Msg> =
    match msg with
    | MessageChanged msg ->
        { model with MessageToSend = msg }, Cmd.none
    | ConnectionChange status ->
        let model = { model with ConnectionState = status }
        match status with
        | ConnectedToServer _ ->
            model, Cmd.ofMsg SyncState
        | Reconnecting ->
            model, Channel.subscription model
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