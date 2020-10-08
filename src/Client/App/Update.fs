module App.Update

open Elmish
open Elmish.Navigation
open Fable.Import
open Browser.Types
open Browser.WebSocket

open Shared

open App.Routing
open App.Model
open App.Channel
open Pages

let private notFound (model : Model) =
    Fable.Core.JS.console.error("Error parsing url: " + Browser.Dom.window.location.href)
    (model, Navigation.modifyUrl (routeHash NotFoundRoute))

let updateRoute (route : PageRoute option) (model : Model) =
    match route with
    | Some SignInRoute ->
        let model', cmd = SignIn.Update.init()
        let menu = { model.Menu with CurrentRoute = SignInRoute }
        { model with Page = SignIn model'; Menu = menu }, Cmd.map SignInMsg cmd
    | Some SignUpRoute ->
        let model', cmd = SignUp.Update.init()
        let menu = { model.Menu with CurrentRoute = SignUpRoute }
        { model with Page = SignUp model'; Menu = menu }, Cmd.map SignUpMsg cmd
    | Some HomeRoute ->
        let model', cmd = Home.Update.init model.ConnectionState
        let menu = { model.Menu with CurrentRoute = HomeRoute }
        { model with Page = Home model'; Menu = menu }, Cmd.map HomeMsg cmd
    | Some (RoomRoute roomName) ->
        let model', cmd = Room.Update.init()
        let menu = { model.Menu with CurrentRoute = (RoomRoute roomName) }
        { model with Page = Room model'; Menu = menu }, Cmd.map RoomMsg cmd
    | Some AboutRoute ->
        let model', cmd = About.Update.init()
        let menu = { model.Menu with CurrentRoute = AboutRoute }
        { model with Page = About model'; Menu = menu }, Cmd.map AboutMsg cmd
    | Some NotFoundRoute ->
        { model with Page = NotFound }, Cmd.none
    | None ->
        notFound model

let init page : Model * Cmd<Msg> =
    let defaultModel () =
        let signInModel, _ = SignIn.Update.init ()
        let model = 
            { ConnectionState = DisconnectedFromServer
              Menu = { CurrentRoute = SignInRoute }
              Page = SignIn signInModel }
        updateRoute page model
    defaultModel ()

let inline private decode<'a> x = x |> unbox<string> |> Thoth.Json.Decode.Auto.unsafeFromString<'a>
let private buildWsSender (ws:WebSocket) : WsSender =
    fun (message:WebSocketClientMessage) ->
        let message = { Topic = ""; Ref = ""; Payload = message }
        let message = Thoth.Json.Encode.Auto.toString(0, message)
        ws.send message

let subscription model =
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
                dispatch (ConnectionChanged (ConnectedToServer (buildWsSender ws)))
                printfn "WebSocket opened")
            ws.onclose <- (fun ev ->
                dispatch (ConnectionChanged DisconnectedFromServer)
                printfn "WebSocket closed. Retrying connection"
                promise {
                    do! Promise.sleep 2000
                    dispatch (ConnectionChanged Connecting)
                    connect() })

        connect()

    Cmd.ofSub sub

let private updatePage updateFunc pageCtor msgCtor model =
    let updatedModel, updatedCmd = updateFunc
    { model with Page = pageCtor updatedModel }, Cmd.map msgCtor updatedCmd

let update (msg : Msg) (model : Model) : Model * Cmd<Msg> =
    match msg, model.Page with
    | SignInMsg msg, SignIn m ->
        model |> updatePage (SignIn.Update.update msg m) SignIn SignInMsg 
    | SignUpMsg msg, SignUp m ->
        model |> updatePage (SignUp.Update.update msg m) SignUp SignUpMsg 
    | HomeMsg msg, Home m ->
        model |> updatePage (Home.Update.update msg m) Home HomeMsg        
    | RoomMsg msg, Room m ->
        model |> updatePage (Room.Update.update msg m) Room RoomMsg
    | AboutMsg msg, About m ->
        model |> updatePage (About.Update.update msg m) About AboutMsg
    | ConnectionChanged state, Home m ->
        let model' = {model with ConnectionState = state}
        model' |> updatePage (Home.Update.update (Home.ConnectionChanged state) m) Home HomeMsg 
    | ReceivedFromServer message, Home m ->
        match message with
        | BroadcastMessage msg ->
            model |> updatePage (Home.Update.update (Home.MessageReceived msg) m) Home HomeMsg 
        | BroadcastMessages msgs ->
            model |> updatePage (Home.Update.update (Home.MessagesReceived msgs) m) Home HomeMsg 
    | ConnectionChanged _, _ ->
        model, Cmd.none 
    | ReceivedFromServer _, _ ->
        model, Cmd.none 
    | SignInMsg _, _ ->
        model, Cmd.none
    | SignUpMsg _, _ ->
        model, Cmd.none
    | HomeMsg _, _ ->
        model, Cmd.none
    | RoomMsg _, _ ->
        model, Cmd.none
    | AboutMsg _, _ ->
        model, Cmd.none