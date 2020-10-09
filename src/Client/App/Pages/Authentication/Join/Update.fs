module Pages.Join.Update

open Elmish

open Pages.Join

let init () =
    { Title = "Join" }, Cmd.none

let update msg model =
    match msg with
    | DummyMsg ->
        model, Cmd.none