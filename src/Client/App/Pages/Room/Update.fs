module Pages.Room.Update

open Elmish

open Pages.Room

let init () =
    { Title = "About" }, Cmd.none

let update msg model =
    match msg with
    | DummyMsg ->
        model, Cmd.none