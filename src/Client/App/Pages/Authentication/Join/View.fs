module Pages.Join.View

open Feliz

open App.Style
open App.Routing
open Pages.Join

let private join' = React.functionComponent("Join", fun ({ Model = model; Dispatch = dispatch }) ->
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
                                                prop.text "Join the Room"
                                            ]
                                            Html.div [
                                                prop.children [
                                                    Html.form [
                                                        prop.children [
                                                            Html.div [
                                                                prop.className Bs.``form-group``
                                                                prop.children [
                                                                    Html.label [
                                                                        prop.htmlFor "nameInput"
                                                                        prop.className Bs.``bmd-label-floating``
                                                                        prop.text "Name"
                                                                    ]
                                                                    Html.input [
                                                                        prop.type' "text"
                                                                        prop.className Bs.``form-control``
                                                                        prop.id "nameInput"
                                                                    ]
                                                                ]
                                                            ]
                                                            Html.div [
                                                                prop.className Bs.``form-group``
                                                                prop.children [
                                                                    Html.label [
                                                                        prop.htmlFor "referenceInput"
                                                                        prop.className Bs.``bmd-label-floating``
                                                                        prop.text "Room Reference"
                                                                    ]
                                                                    Html.input [
                                                                        prop.type' "text"
                                                                        prop.className Bs.``form-control``
                                                                        prop.id "referenceInput"
                                                                    ]
                                                                ]
                                                            ]
                                                            Html.button [
                                                                prop.className [ Bs.btn; Bs.``btn-primary``; Bs.``btn-raised``; Bs.``btn-block`` ]
                                                                prop.text "Request Access"
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

let join model dispatch = join' { Model = model; Dispatch = dispatch }