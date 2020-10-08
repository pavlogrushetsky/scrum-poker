module Pages.Login.Update

open Elmish

open Pages.Login

let init () =
    { Title = "About" }, Cmd.none

let update msg model =
    match msg with
    | DummyMsg ->
        model, Cmd.none