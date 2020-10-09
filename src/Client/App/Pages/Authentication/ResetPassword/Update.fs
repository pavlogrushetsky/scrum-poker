module Pages.ResetPassword.Update

open Elmish

open Pages.ResetPassword

let init () =
    { Title = "Reset Password" }, Cmd.none

let update msg model =
    match msg with
    | DummyMsg ->
        model, Cmd.none