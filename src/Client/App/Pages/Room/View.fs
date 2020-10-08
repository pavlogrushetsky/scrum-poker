module Pages.Room.View

open Feliz

open Pages.Room

let view (model : Model) dispatch =
    Html.h4 [ prop.text model.Title ]

let private room' = React.functionComponent("Room", fun ({ Model = model; Dispatch = dispatch }) ->
    Html.h4 [ prop.text model.Title ])

let room model dispatch = room' { Model = model; Dispatch = dispatch }