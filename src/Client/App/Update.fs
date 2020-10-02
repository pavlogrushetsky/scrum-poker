module App.Update

open Elmish
open Elmish.Navigation
open Fable.Import
open Thoth.Fetch

open Shared

open App.Routing
open App.Model
open Pages

let private notFound (model : Model) =
    Fable.Core.JS.console.error("Error parsing url: " + Browser.Dom.window.location.href)
    (model, Navigation.modifyUrl (routeHash NotFoundRoute))

let updateRoute (route : PageRoute option) (model : Model) =
    match route with
    | Some HomeRoute ->
        let model', cmd = Home.Update.init()
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
        let homeModel, _ = Home.Update.init ()
        let model = 
            { Menu = { CurrentRoute = HomeRoute }
              Page = Home homeModel }
        updateRoute page model
    defaultModel ()

let private updatePage updateFunc pageCtor msgCtor model =
    let updatedModel, updatedCmd = updateFunc
    { model with Page = pageCtor updatedModel }, Cmd.map msgCtor updatedCmd

let subscription (model : Model) =
    Cmd.batch [ Cmd.map HomeMsg (Home.Channel.subscription model) ]   

let update (msg : Msg) (model : Model) : Model * Cmd<Msg> =
    match msg, model.Page with
    | HomeMsg msg, Home m ->
        model |> updatePage (Home.Update.update msg m) Home HomeMsg        
    | RoomMsg msg, Room m ->
        model |> updatePage (Room.Update.update msg m) Room RoomMsg
    | AboutMsg msg, About m ->
        model |> updatePage (About.Update.update msg m) About AboutMsg
    | HomeMsg _, _ ->
        model, Cmd.none
    | RoomMsg _, _ ->
        model, Cmd.none
    | AboutMsg _, _ ->
        model, Cmd.none