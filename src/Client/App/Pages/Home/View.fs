module Pages.Home.View

open System

open Fable.React
open Fable.React.Props
open Browser.Types
open Feliz

open Shared

open App.Style
open App.Channel
open Pages.Home

let private drawStatus connectionState =
    let className, text =
        match connectionState with
        | DisconnectedFromServer -> "danger", "Disconnected from server"
        | Connecting -> "secondary", "Connecting..."
        | Reconnecting -> "secondary", "Connecting..."
        | ConnectedToServer _ -> "success", "Connected to server"
    span [ ClassName (sprintf "badge badge-%s" className) ] [ str text ]

let private home' = React.functionComponent("Home", fun ({ Model = model; Dispatch = dispatch }) ->
    Html.main [
        prop.role "main"
        prop.className Bs.container
        prop.children [
            Html.h3 [ prop.text "Send a message!" ]
            Html.div [
                prop.className Bs.row
                prop.children [
                    Html.div [
                        prop.className Bs.col
                        prop.children [
                            Html.div [
                                prop.className Bs.``form-group``
                                prop.children [
                                    Html.label [
                                        prop.htmlFor "messageInput"
                                        prop.className Bs.``bmd-label-floating``
                                        prop.text "Message"
                                    ]
                                    Html.input [
                                        prop.type' "text"
                                        prop.className Bs.``form-control``
                                        prop.id "messageInput"
                                        prop.onChange (fun (e : Event) -> dispatch(MessageChanged e.Value))
                                    ]
                                ]
                            ]
                        ]
                    ]
                    Html.div [
                        prop.className [ Bs.``col-md-auto``; Bs.``my-auto``]
                        prop.children [
                            Html.button [
                                prop.type' "button"
                                prop.className [ Bs.btn; Bs.``btn-outline-primary`` ]
                                prop.disabled (String.IsNullOrEmpty model.MessageToSend || not model.ConnectionState.IsConnected)
                                prop.onClick (fun _ -> dispatch (Broadcast (ViaWebSocket, model.MessageToSend)))
                                prop.text "Broadcast"
                            ]
                        ]
                    ]
                ]
            ]
            Html.div [
                prop.className Bs.row
                prop.children [
                    Html.div [
                        prop.className Bs.col
                        prop.children [ drawStatus model.ConnectionState ]
                    ]
                ]
            ]
            Html.div [
                prop.className Bs.row
                prop.children [
                    Html.div [
                        prop.className Bs.col
                        prop.children [ 
                            match model.ReceivedMessages with
                            | [] ->
                                ()
                            | messages ->
                                Html.table [
                                    prop.className [ Bs.table; "table-borderless"; Bs.``table-hover`` ]
                                    prop.children [
                                        Html.thead [
                                            prop.className "thead"
                                            prop.children [
                                                Html.tr [
                                                    prop.children [
                                                        Html.th [ 
                                                            prop.scope "col"
                                                            prop.text "Time"
                                                        ]
                                                        Html.th [ 
                                                            prop.scope "col"
                                                            prop.text "Message"
                                                        ]
                                                    ]
                                                ]
                                            ]
                                        ]
                                        Html.tbody [
                                            prop.children [
                                                for message in messages do
                                                    Html.tr [
                                                        prop.children [
                                                            Html.td [ prop.text (sprintf "%O" message.Time) ]
                                                            Html.td [ prop.text message.Text ]
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

let home model dispatch = home' { Model = model; Dispatch = dispatch }