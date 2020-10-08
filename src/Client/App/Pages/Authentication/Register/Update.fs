module Pages.Register.Update

open Elmish

open Pages.Register

let init () =
    { Title = "Register" }, Cmd.none

let update msg model =
    match msg with
    | DummyMsg ->
        model, Cmd.none