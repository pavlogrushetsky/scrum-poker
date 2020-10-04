module Pages.About.Update

open Elmish

open Pages.About

let init () =
    { Title = "About" }, Cmd.none

let update msg model =
    match msg with
    | DummyMsg ->
        model, Cmd.none