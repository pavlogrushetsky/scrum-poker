module App.View

open System

open Fable.React
open Fable.React.Props
open Feliz

open Shared

open App.Style
open App.Routing
open App.Components.Menu
open App.Model

open Pages.Login.View
open Pages.Home.View
open Pages.Room.View
open Pages.About.View

let private renderPage dispatch page =
    match page with
    | Login model ->
        login model (LoginMsg >> dispatch)
    | Home model ->
        home model (HomeMsg >> dispatch)
    | Room model ->
        room model (RoomMsg >> dispatch)
    | About model ->
        about model (AboutMsg >> dispatch)
    | NotFound ->
        div [] []

let private footer =
    Html.footer [
        prop.style [
            style.bottom 0
            style.width (length.percent 100)
            style.height (length.px 60)
            style.lineHeight (length.px 60)
            style.backgroundColor "#fafafa"
        ]
        prop.children [
            Html.div [
                prop.className Bs.container
                prop.children [
                    Html.p [
                        prop.className Bs.``float-right``
                        prop.children [
                            Html.a [
                                prop.href "#"
                                prop.text "Back to top"
                            ]
                        ]
                    ]
                    Html.p [ prop.text "© 2020 Pavlo Hrushetskyi" ]
                ]
            ]
        ]
    ]   

let view (model : Model) (dispatch : Msg -> unit) =
    div [] [
        match model.Page with
        | Login _ ->
            ignore ()
        | _ ->
            menu HomeRoute

        renderPage dispatch model.Page

        match model.Page with
        | Login _ ->
            ignore ()
        | _ ->
            footer     
    ]