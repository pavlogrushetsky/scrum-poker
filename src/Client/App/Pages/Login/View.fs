module Pages.Login.View

open Feliz

open App.Style
open Pages.Login

let private login' = React.functionComponent("Login", fun ({ Model = model; Dispatch = dispatch }) ->
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
                                                prop.text "Login To Scrum Poker!"
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
                                                                prop.className [ Bs.btn; Bs.``btn-primary``; Bs.``btn-raised``; Bs.``btn-block`` ]
                                                                prop.text "Login"
                                                            ]                            
                                                            Html.hr []
                                                            Html.a [  
                                                                prop.className [ Bs.btn; Bs.``btn-secondary``; Bs.``btn-raised``; Bs.``btn-block`` ]
                                                                prop.href "#"
                                                                prop.children [
                                                                    Html.icon Fa.``fa-google``
                                                                    Html.text "  Login with Google"
                                                                ]
                                                            ]
                                                            Html.hr []
                                                            Html.h5 [
                                                                prop.className [ Bs.``text-center`` ]
                                                                prop.text "OR"
                                                            ]
                                                            Html.p [
                                                                prop.className [ Bs.``text-muted``; Bs.``text-center`` ]
                                                                prop.text "Request access to the room by reference!"
                                                            ]
                                                            Html.div [
                                                                prop.className Bs.``form-group``
                                                                prop.children [
                                                                    Html.label [
                                                                        prop.htmlFor "roomReferenceInput"
                                                                        prop.className Bs.``bmd-label-floating``
                                                                        prop.text "Room Reference"
                                                                    ]
                                                                    Html.input [
                                                                        prop.type' "text"
                                                                        prop.className Bs.``form-control``
                                                                        prop.id "roomReferenceInput"
                                                                    ]
                                                                ]
                                                            ]
                                                            Html.button [
                                                                prop.className [ Bs.btn; Bs.``btn-secondary``; Bs.``btn-raised``; Bs.``btn-block`` ]
                                                                prop.text "Request Access"
                                                            ]
                                                            Html.hr []
                                                            Html.button [
                                                                prop.type' "button"
                                                                prop.className [ Bs.btn; Bs.``btn-secondary``; Bs.``btn-block`` ]
                                                                prop.text "Forgot Password?"
                                                            ]
                                                            Html.button [
                                                                prop.type' "button"
                                                                prop.className [ Bs.btn; Bs.``btn-secondary``; Bs.``btn-block`` ]
                                                                prop.text "Create an Account"
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

let login model dispatch = login' { Model = model; Dispatch = dispatch }