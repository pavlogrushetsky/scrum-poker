module App.Components.Menu

open Elmish
open Feliz

open App.Style
open App.Routing

type Menu = {
    CurrentRoute : PageRoute
}

let private menu' = React.functionComponent("Menu", fun (props : Menu) ->      
    Html.div [
        prop.className [ Sem.ui; "fixed"; Sem.inverted; Sem.menu ]
        prop.children [
            Html.div [
                prop.className [ Sem.ui; Sem.container ]
                prop.children [
                    Html.a [
                        prop.className [ Sem.header; Sem.item ]
                        prop.href ""
                        prop.text "Scrum Poker"
                    ]
                    Html.a [
                        prop.className [ 
                            Sem.blue 
                            Sem.item 
                            if props.CurrentRoute = HomeRoute
                            then Sem.active
                        ]
                        prop.text "Home"
                        prop.href (routeHash HomeRoute)
                        prop.onClick goToUrl
                    ]
                    Html.a [
                        prop.className [ 
                            Sem.item 
                            Sem.green 
                            if props.CurrentRoute = (RoomRoute "1")
                            then Sem.active
                        ]
                        prop.text "Room"
                        prop.href (routeHash (RoomRoute "1"))
                        prop.onClick goToUrl
                    ]
                    Html.a [
                        prop.className [ 
                            Sem.item 
                            Sem.teal 
                            if props.CurrentRoute = AboutRoute
                            then Sem.active
                        ]
                        prop.text "About"
                        prop.href (routeHash AboutRoute)
                        prop.onClick goToUrl
                    ]
                    Html.div [
                        prop.className [ Sem.right; Sem.menu ]
                        prop.children [
                            Html.a [
                                prop.className [ Sem.item ]
                                prop.href "https://github.com/pavlogrushetsky/scrum-poker"
                                prop.target "_blank"
                                prop.children [
                                    Html.i [
                                        prop.className [ Sem.github; Sem.icon ]
                                    ]
                                ]
                            ]
                            Html.a [
                                prop.className [ Sem.item ]
                                prop.text "Sign In"
                                prop.href (routeHash SignInRoute)
                                prop.onClick goToUrl
                            ]
                            Html.a [
                                prop.className [ Sem.item ]
                                prop.text "Sign Up"
                                prop.href (routeHash SignUpRoute)
                                prop.onClick goToUrl
                            ]
                        ]
                    ]
                ]
            ]
        ]
    ])

let menu currentRoute = menu' { CurrentRoute = currentRoute }