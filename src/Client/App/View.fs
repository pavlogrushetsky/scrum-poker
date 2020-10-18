module App.View

open Fable.React
open Feliz

open App.Style
open App.Components.Menu
open App.Model

open Pages.SignIn.View
open Pages.SignUp.View
open Pages.Join.View
open Pages.RecoverPassword.View
open Pages.Home.View
open Pages.Room.View
open Pages.About.View

let private renderPage dispatch page =
    match page with
    | SignIn model ->
        signIn ()
    | SignUp model ->
        signUp ()
    | Join model ->
        join ()
    | RecoverPassword model ->
        recoverPassword ()
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
        | SignIn _ | SignUp _ | Join _ | RecoverPassword _ ->
            ignore ()
        | _ ->
            menu model.Menu.CurrentRoute

        renderPage dispatch model.Page

        match model.Page with
        | SignIn _ | SignUp _ | Join _ | RecoverPassword _ ->
            ignore ()
        | _ ->
            footer     
    ]