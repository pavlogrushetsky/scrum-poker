module Pages.RecoverPassword.Update

open Elmish

open Pages.RecoverPassword

let init () =
    { Title = "Recover Password" }, Cmd.none

let update msg model =
    match msg with
    | DummyMsg ->
        model, Cmd.none