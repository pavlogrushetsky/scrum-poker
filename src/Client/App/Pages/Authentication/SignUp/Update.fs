module Pages.SignUp.Update

open Elmish

open Pages.SignUp

let init () =
    { Title = "SignUp" }, Cmd.none

let update msg model =
    match msg with
    | DummyMsg ->
        model, Cmd.none