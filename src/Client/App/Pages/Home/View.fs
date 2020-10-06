module Pages.Home.View

open System

open Fable.React
open Fable.React.Props
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
    main [ Role "main"; ClassName "container" ] [
        h3 [] [ str "Send a message!" ]
        div [ ClassName "row" ] [
            div [ ClassName "col" ] [
                div [ ClassName "form-group" ] [
                    label [ HtmlFor "messageInput"; ClassName "bmd-label-floating" ] [ str "Message" ]
                    input [ Typeof "text"; ClassName "form-control"; Id "messageInput"; OnChange (fun e -> dispatch(MessageChanged e.Value)) ]
                ]                    
            ]
            div [ ClassName "col-md-auto my-auto" ] [
                button [ 
                    Typeof "button"; 
                    ClassName "btn btn-outline-primary"; 
                    Disabled (String.IsNullOrEmpty model.MessageToSend || not model.ConnectionState.IsConnected)
                    OnClick (fun _ -> dispatch (Broadcast (ViaWebSocket, model.MessageToSend))) ] [ str "Broadcast"]
            ]
        ]
        div [ ClassName "row" ] [
            div [ ClassName "col" ] [ drawStatus model.ConnectionState ]               
        ]
        div [ ClassName "row" ] [
            div [ ClassName "col" ] [ 
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
    ])

let home model dispatch = home' { Model = model; Dispatch = dispatch }