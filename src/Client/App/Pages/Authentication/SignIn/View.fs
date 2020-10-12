module Pages.SignIn.View

open Feliz

open App.Style
open App.Components.SignInForm
open Pages.SignIn

let private signIn' = React.functionComponent("SignIn", fun ({ Model = model; Dispatch = dispatch }) ->
    Html.div [
        prop.className [ Sem.ui; Sem.text; Sem.container; Sem.middle; Sem.aligned ]
        prop.children [
            signInForm ()
        ]
    ])

let signIn model dispatch = signIn' { Model = model; Dispatch = dispatch }