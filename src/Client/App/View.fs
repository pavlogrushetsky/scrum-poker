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
        signUp model (SignUpMsg >> dispatch)
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
    Html.div [
        prop.className [ Sem.ui; Sem.inverted; Sem.vertical; "footer"; Sem.segment ]
        prop.children [
            Html.div [
                prop.className [ Sem.ui; Sem.container ]           
                prop.children [
                    Html.div [
                        prop.className [ Sem.ui; Sem.stackable; Sem.inverted; Sem.divided; "equal"; Sem.height; Sem.stackable; Sem.grid ]
                        prop.children [
                            Html.div [
                                prop.className [ Sem.three; Sem.wide; Sem.column ]
                                prop.children [
                                    Html.h4 [
                                        prop.className [ Sem.ui; Sem.inverted; Sem.header ]
                                        prop.text "About"
                                    ]
                                    Html.div [
                                        prop.className [ Sem.ui; Sem.inverted; Sem.link; Sem.list ]
                                        prop.children [
                                            Html.a [
                                                prop.href "#"
                                                prop.className Sem.item
                                                prop.text "Scrum Poker"
                                            ]
                                            Html.a [
                                                prop.href "#"
                                                prop.className Sem.item
                                                prop.text "Github"
                                            ]
                                        ]
                                    ]
                                ]
                            ]
                            Html.div [
                                prop.className [ Sem.ten; Sem.wide; Sem.column ]
                                prop.children [
                                    Html.h4 [
                                        prop.className [ Sem.ui; Sem.inverted; Sem.header ]
                                        prop.text "© 2020 Pavlo Hrushetskyi"
                                    ]
                                ]
                            ]
                        ]
                    ]
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