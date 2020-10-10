module Pages.SignUp.View

open Feliz

open App.Style
open App.Routing
open App.Components.SignUpForm
open Pages.SignUp

let private signUp' = React.functionComponent("Sign Up", fun ({ Model = model; Dispatch = dispatch }) ->
    Html.main [
        prop.role "main"
        prop.className Bs.container
        prop.children [
            Html.div [
                prop.className [ Bs.row; Bs.``justify-content-center`` ]
                prop.children [
                    Html.div [
                        prop.className [ Bs.``col-6`` ]
                        prop.children [
                            Html.div [
                                prop.className [ Bs.card ]
                                prop.children [
                                    Html.div [
                                        prop.className [ Bs.``card-body`` ]
                                        prop.children [
                                            Html.h5 [
                                                prop.className [ Bs.``card-title``; Bs.``text-center`` ]
                                                prop.text "Sign Up"
                                            ]
                                            Html.div [
                                                prop.children [
                                                    signUpForm ()
                                                    Html.hr []   
                                                    Html.a [
                                                        prop.className [ Bs.btn; Bs.``btn-secondary``; Bs.``btn-block`` ]
                                                        prop.href (routeHash SignInRoute)
                                                        prop.onClick goToUrl
                                                        prop.text "Back to Sign In"
                                                    ]     
                                                ]
                                            ]
                                        ]
                                    ]
                                ]
                            ]
                        ]
                    ]
                ]
            ]
        ]
    ])

let signUp model dispatch = signUp' { Model = model; Dispatch = dispatch }