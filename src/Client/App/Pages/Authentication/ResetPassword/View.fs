module Pages.ResetPassword.View

open Feliz

open App.Style
open App.Routing
open Pages.ResetPassword

let private resetPassword' = React.functionComponent("ResetPassword", fun ({ Model = model; Dispatch = dispatch }) ->
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
                                                prop.text "Reset Password"
                                            ]
                                            Html.div [
                                                prop.children [
                                                    Html.form [
                                                        prop.children [
                                                            Html.div [
                                                                prop.className Bs.``form-group``
                                                                prop.children [
                                                                    Html.label [
                                                                        prop.htmlFor "oldPasswordInput"
                                                                        prop.className Bs.``bmd-label-floating``
                                                                        prop.text "Current Password"
                                                                    ]
                                                                    Html.input [
                                                                        prop.type' "password"
                                                                        prop.className Bs.``form-control``
                                                                        prop.id "oldPasswordInput"
                                                                    ]
                                                                ]
                                                            ]
                                                            Html.div [
                                                                prop.className Bs.``form-group``
                                                                prop.children [
                                                                    Html.label [
                                                                        prop.htmlFor "newPasswordInput"
                                                                        prop.className Bs.``bmd-label-floating``
                                                                        prop.text "New Password"
                                                                    ]
                                                                    Html.input [
                                                                        prop.type' "password"
                                                                        prop.className Bs.``form-control``
                                                                        prop.id "newPasswordInput"
                                                                    ]
                                                                ]
                                                            ]
                                                            Html.div [
                                                                prop.className Bs.``form-group``
                                                                prop.children [
                                                                    Html.label [
                                                                        prop.htmlFor "repeatPasswordInput"
                                                                        prop.className Bs.``bmd-label-floating``
                                                                        prop.text "Repeat New Password"
                                                                    ]
                                                                    Html.input [
                                                                        prop.type' "password"
                                                                        prop.className Bs.``form-control``
                                                                        prop.id "repeatPasswordInput"
                                                                    ]
                                                                ]
                                                            ]
                                                            Html.button [
                                                                prop.className [ Bs.btn; Bs.``btn-primary``; Bs.``btn-raised``; Bs.``btn-block`` ]
                                                                prop.text "Reset Password"
                                                            ]    
                                                            Html.hr []   
                                                            Html.a [
                                                                prop.className [ Bs.btn; Bs.``btn-secondary``; Bs.``btn-block`` ]
                                                                // prop.href (routeHash SignInRoute)
                                                                // prop.onClick goToUrl
                                                                prop.text "Back"
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

let resetPassword model dispatch = resetPassword' { Model = model; Dispatch = dispatch }