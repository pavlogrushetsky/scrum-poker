module App.Routing

open Elmish.UrlParser
open Elmish.Navigation
open Browser.Types
open Fable.Core.JsInterop

type PageRoute =
    | LoginRoute
    | RegisterRoute
    | HomeRoute
    | RoomRoute of roomName : string
    | AboutRoute
    | NotFoundRoute

let [<Literal>] private Login = "login"
let [<Literal>] private Register = "signup"
let [<Literal>] private Room = "room"
let [<Literal>] private About = "about"
let [<Literal>] private NotFound = "notfound"

let private hash = sprintf "#/%s"

let routeHash = 
    function
    | LoginRoute -> hash Login
    | RegisterRoute -> hash Register
    | HomeRoute -> ""
    | RoomRoute roomName -> hash (sprintf "%s/%s" Room roomName)
    | AboutRoute -> hash About
    | NotFoundRoute -> hash NotFound

let private mapRoute : Parser<PageRoute -> PageRoute, PageRoute> =
    oneOf [
        map LoginRoute (s Login)
        map RegisterRoute (s Register)
        map HomeRoute top
        map RoomRoute (s Room </> str)
        map AboutRoute (s About)
        map NotFoundRoute (s NotFound)
    ]

let parseRoute location = parseHash mapRoute location

let goToUrl (e : MouseEvent) =
    e.preventDefault()
    let href = !!e.currentTarget?href
    Navigation.newUrl href
    |> List.map (fun f -> f ignore)
    |> ignore