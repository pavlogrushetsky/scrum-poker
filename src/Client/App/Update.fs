module App.Update

open Elmish
open Elmish.Navigation
open Fable.Import
open Thoth.Fetch

open Shared

open App.Routing
open App.Model

let private notFound (model : Model) =
    Fable.Core.JS.console.error("Error parsing url: " + Browser.Dom.window.location.href)
    (model, Navigation.modifyUrl (routeHash NotFoundRoute))

let updateRoute (route : PageRoute option) (model : Model) =
    match route with
    | Some HomeRoute ->
        model, Cmd.none
    | Some (RoomRoute roomName) ->
        model, Cmd.none
    | Some AboutRoute ->
        model, Cmd.none
    | Some NotFoundRoute ->
        model, Cmd.none
    | None ->
        notFound model

let init page : Model * Cmd<Msg> =
    let defaultModel () =
        let model = 
            { MessageToSend = null
              ConnectionState = DisconnectedFromServer
              ReceivedMessages = [] }
        updateRoute page model
    defaultModel ()

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