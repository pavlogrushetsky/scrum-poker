module Pages.Register.View

open Feliz

open App.Style
open App.Routing
open Pages.Register

let private register' = React.functionComponent("Register", fun ({ Model = model; Dispatch = dispatch }) ->
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
                                                    Html.form [
                                                        prop.children [
                                                            Html.div [
                                                                prop.className Bs.``form-group``
                                                                prop.children [
                                                                    Html.label [
                                                                        prop.htmlFor "emailInput"
                                                                        prop.className Bs.``bmd-label-floating``
                                                                        prop.text "Email"
                                                                    ]
                                                                    Html.input [
                                                                        prop.type' "email"
                                                                        prop.className Bs.``form-control``
                                                                        prop.id "emailInput"
                                                                    ]
                                                                ]
                                                            ]
                                                            Html.div [
                                                                prop.className Bs.``form-group``
                                                                prop.children [
                                                                    Html.label [
                                                                        prop.htmlFor "passwordInput"
                                                                        prop.className Bs.``bmd-label-floating``
                                                                        prop.text "Password"
                                                                    ]
                                                                    Html.input [
                                                                        prop.type' "password"
                                                                        prop.className Bs.``form-control``
                                                                        prop.id "passwordInput"
                                                                    ]
                                                                ]
                                                            ]
                                                            Html.div [
                                                                prop.className Bs.``form-group``
                                                                prop.children [
                                                                    Html.label [
                                                                        prop.htmlFor "repeatPasswordInput"
                                                                        prop.className Bs.``bmd-label-floating``
                                                                        prop.text "Repeat Password"
                                                                    ]
                                                                    Html.input [
                                                                        prop.type' "password"
                                                                        prop.className Bs.``form-control``
                                                                        prop.id "repeatPasswordInput"
                                                                    ]
                                                                ]
                                                            ]
                                                            Html.div [
                                                                prop.className [ Bs.checkbox; Bs.``my-4`` ]
                                                                prop.children [
                                                                    Html.label [                                                                         
                                                                        prop.children [
                                                                            Html.input [
                                                                                prop.type' "checkbox"
                                                                            ]
                                                                            Html.text " Remember Me"
                                                                        ]                                                                                                                                               
                                                                    ]
                                                                ]
                                                            ]
                                                            Html.button [
                                                                prop.type' "button"
                                                                prop.className [ Bs.btn; Bs.``btn-primary``; Bs.``btn-raised``; Bs.``btn-block`` ]
                                                                prop.text "Sign Up"
                                                            ]  
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
            ]
        ]
    ])

let register model dispatch = register' { Model = model; Dispatch = dispatch }