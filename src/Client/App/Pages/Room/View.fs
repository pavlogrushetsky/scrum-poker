module Pages.Room.View

open Fable.React
open Fable.React.Props
open Feliz

open Pages.Room
open App.Style

let view (model : Model) dispatch =
    Html.h4 [ prop.text model.Title ]

let private room' = React.functionComponent("Room", fun ({ Model = model; Dispatch = dispatch }) ->
    Html.main [
        prop.role "main"
        prop.className Bs.container
        prop.children [
            Html.div [
                prop.className Bs.row
                prop.children [
                    Html.div [
                        prop.className Bs.``col-8``
                        prop.children [
                            Html.div [
                                prop.className Bs.``card-deck``
                                prop.children [
                                    Html.div [
                                        prop.className [ Bs.card; Bs.``bg-info`` ]
                                        prop.children [
                                            Html.div [
                                                prop.className Bs.``card-body``
                                                prop.children [
                                                    Html.h1 [
                                                        prop.className Bs.``card-text``
                                                        prop.text "0"
                                                    ]
                                                ]
                                            ]
                                        ]
                                    ]
                                    Html.div [
                                        prop.className Bs.card
                                    ]
                                    Html.div [
                                        prop.className Bs.card
                                    ]
                                    Html.div [
                                        prop.className Bs.card
                                    ]
                                    Html.div [
                                        prop.className Bs.card
                                    ]
                                    Html.div [
                                        prop.className Bs.card
                                    ]
                                ]
                            ]
                        ]
                    ]
                    Html.div [
                        prop.className Bs.``col-4``
                    ]
                ]
            ]
        ]
    ])

let room model dispatch = room' { Model = model; Dispatch = dispatch }