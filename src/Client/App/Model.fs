module App.Model

open Shared

open App.Channel
open Pages
open App.Components.Menu

type Page =
    | Home of Home.Model
    | Room of Room.Model
    | About of About.Model
    | NotFound

type Model =
    { ConnectionState : ConnectionState
      Menu : Menu
      Page : Page }

type Msg =
    | HomeMsg of Home.Msg
    | RoomMsg of Room.Msg
    | AboutMsg of About.Msg
    | ReceivedFromServer of WebSocketServerMessage
    | ConnectionChanged of ConnectionState