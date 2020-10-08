module Pages.SignIn.Update

open Elmish

open Pages.SignIn

let init () =
    { Title = "SignIn" }, Cmd.none

let update msg model =
    match msg with
    | DummyMsg ->
        model, Cmd.none