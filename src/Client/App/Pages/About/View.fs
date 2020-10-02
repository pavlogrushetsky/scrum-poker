module Pages.About.View

open Fable.React
open Fable.React.Props
open Feliz

open Pages.About

let view (model : Model) dispatch =
    Html.h4 [ prop.text model.Title ]

let private about' = React.functionComponent("About", fun ({ Model = model; Dispatch = dispatch }) ->
    Html.h4 [ prop.text model.Title ])

let about model dispatch = about' { Model = model; Dispatch = dispatch }