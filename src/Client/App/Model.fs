module App.Model

open Shared

open Pages
open App.Components.Menu

type Page =
    | Home of Home.Model
    | Room of Room.Model
    | About of About.Model
    | NotFound

type Model =
    { Menu : Menu
      Page : Page }

type Msg =
    | HomeMsg of Home.Msg
    | RoomMsg of Room.Msg
    | AboutMsg of About.Msg