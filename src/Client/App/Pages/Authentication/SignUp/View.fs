module Pages.SignUp.View

open Feliz

open App.Style
open App.Components.SignUpForm
open Pages.SignUp

let private signUp' = React.functionComponent("Sign Up", fun ({ Model = model; Dispatch = dispatch }) ->
    Html.div [
        prop.className [ Sem.ui; Sem.text; Sem.container; Sem.middle; Sem.aligned ]
        prop.children [
            signUpForm ()
        ]
    ])

let signUp model dispatch = signUp' { Model = model; Dispatch = dispatch }