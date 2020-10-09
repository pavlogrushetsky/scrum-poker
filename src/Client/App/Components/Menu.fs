module App.Components.Menu

open Elmish
open Feliz

open App.Style
open App.Routing

type Menu = {
    CurrentRoute : PageRoute
}

let private menu' = React.functionComponent("Menu", fun (props : Menu) ->
    Html.nav [
        prop.className [ Bs.navbar; Bs.``navbar-expand-lg``; Bs.``navbar-dark``; Bs.``bg-primary``; Bs.``fixed-top`` ]
        prop.children [
            Html.div [
                prop.className Bs.container
                prop.children [
                    Html.a [
                        prop.className Bs.``navbar-brand``
                        prop.href ""
                        prop.text "Scrum Poker"
                    ]
                    Html.button [
                        prop.className Bs.``navbar-toggler``
                        prop.type' "button"
                        prop.dataToggle "collapse"
                        prop.dataTarget "#navbarNavDropdown"
                        prop.ariaControls "navbarNavDropdown"
                        prop.ariaExpanded false
                        prop.ariaLabel "Toggle navigation"
                        prop.children [
                            Html.span [
                                prop.className Bs.``navbar-toggler-icon``
                            ]
                        ]
                    ]
                    Html.div [
                        prop.className [ Bs.collapse; Bs.``navbar-collapse`` ]
                        prop.id "navbarNavDropdown"
                        prop.children [
                            Html.ul [
                                prop.className [ Bs.``navbar-nav``; Bs.``mr-auto`` ]
                                prop.children [
                                    Html.route "Home" "" HomeRoute props.CurrentRoute
                                    Html.route "Room #1" "" (RoomRoute "Room #1") props.CurrentRoute
                                    Html.route "About" "" AboutRoute props.CurrentRoute
                                ]
                            ]
                            Html.ul [
                                prop.className [ Bs.``navbar-nav``; Bs.``mr-md-2`` ]
                                prop.children [                                    
                                    Html.li [
                                        prop.className Bs.``nav-item``
                                        prop.children [                                           
                                            Html.a [
                                                prop.className Bs.``nav-link``
                                                prop.href "https://github.com/pavlogrushetsky/scrum-poker"
                                                prop.target "_blank"
                                                prop.children [
                                                    Html.icon Fa.``fa-github``
                                                ]
                                            ]
                                        ]
                                    ]
                                    Html.route "Sign In" "" SignInRoute props.CurrentRoute
                                    Html.route "Join Room" "" JoinRoute props.CurrentRoute
                                    Html.route "Sign Up" "" SignUpRoute props.CurrentRoute
                                ]
                            ]
                        ]
                    ]
                ]
            ]
        ]
    ])

let menu currentRoute = menu' { CurrentRoute = currentRoute }