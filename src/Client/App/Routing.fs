module App.Routing

open Elmish.UrlParser
open Elmish.Navigation
open Browser.Types
open Fable.Core.JsInterop

type PageRoute =
    | SignInRoute
    | SignUpRoute
    | HomeRoute
    | RoomRoute of roomName : string
    | AboutRoute
    | NotFoundRoute

let [<Literal>] private SignIn = "signin"
let [<Literal>] private SignUp = "signup"
let [<Literal>] private Room = "room"
let [<Literal>] private About = "about"
let [<Literal>] private NotFound = "notfound"

let private hash = sprintf "#/%s"

let routeHash = 
    function
    | SignInRoute -> hash SignIn
    | SignUpRoute -> hash SignUp
    | HomeRoute -> ""
    | RoomRoute roomName -> hash (sprintf "%s/%s" Room roomName)
    | AboutRoute -> hash About
    | NotFoundRoute -> hash NotFound

let private mapRoute : Parser<PageRoute -> PageRoute, PageRoute> =
    oneOf [
        map SignInRoute (s SignIn)
        map SignUpRoute (s SignUp)
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