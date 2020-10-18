module App.Model

open Shared

open App.Channel
open Pages
open App.Components.Menu

type Page =
    | SignIn of SignIn.Model
    | SignUp of SignUp.Model
    | Join of Join.Model
    | RecoverPassword of RecoverPassword.Model
    | Home of Home.Model
    | Room of Room.Model
    | About of About.Model
    | NotFound

type Model =
    { ConnectionState : ConnectionState
      Menu : Menu
      Page : Page }

type Msg =
    | SignInMsg of SignIn.Msg
    | SignUpMsg of SignUp.Msg
    | JoinMsg of Join.Msg
    | RecoverPasswordMsg of RecoverPassword.Msg
    | HomeMsg of Home.Msg
    | RoomMsg of Room.Msg
    | AboutMsg of About.Msg
    | ReceivedFromServer of WebSocketServerMessage
    | ConnectionChanged of ConnectionState