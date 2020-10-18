module Pages.Join.View

open Feliz
open Feliz.UseDeferred

open App.Style
open App.Routing

let private joinRoom name room = async {
    do! Async.Sleep 5000
    if name = "user" && room = "room"
    then return Ok "room"
    else return Error "Restricted access"
}

let join = React.functionComponent("Join", fun () ->
    let (joinState, setJoinState) = React.useState(Deferred.HasNotStartedYet)
    let nameRef = React.useInputRef()
    let roomRef = React.useInputRef()


    let join () =
        match nameRef.current, roomRef.current with
        | Some name, Some room -> joinRoom name.value room.value
        | _ -> failwith "Component hasn't been initialized"

    let startJoin = React.useDeferredCallback(join, setJoinState)

    let message =
        match joinState with
        | Deferred.HasNotStartedYet -> Html.none
        | Deferred.InProgress -> Html.none
        | Deferred.Failed error -> Html.attachedError (sprintf "Internal error: %s" error.Message)
        | Deferred.Resolved (Ok user) -> Html.attachedSuccess (sprintf "User %s joined" user)
        | Deferred.Resolved (Error error) -> Html.attachedError (sprintf "Join error: %s" error)

    Html.div [
        prop.className [ Sem.ui; Sem.form; Sem.text; Sem.container; Sem.middle; Sem.aligned ]
        prop.children [
            Html.div [
                prop.className [ Sem.ui; Sem.piled; Sem.segments ]
                prop.children [
                    Html.div [
                        prop.className [ Sem.ui; Sem.attached; Sem.message; ]
                        prop.children [
                            Html.div [
                                prop.className Sem.header
                                prop.text "Join to Scrum Poker room!"
                            ]
                            Html.p [ prop.text "Fill out the form below to join an existing room by reference" ]
                        ]
                    ]
                    message
                    Html.form [
                        prop.className [ Sem.ui; Sem.form; Sem.attached; Sem.fluid; Sem.segment ]
                        prop.children [
                            Html.div [
                                prop.className Sem.field
                                prop.children [
                                    Html.label [ prop.text "Name" ]
                                    Html.input [
                                        prop.placeholder "Name"
                                        prop.type' "text"
                                        prop.ref nameRef
                                        prop.disabled (Deferred.inProgress joinState)
                                    ]
                                ]
                            ]
                            Html.div [
                                prop.className Sem.field
                                prop.children [
                                    Html.label [ prop.text "Room Reference" ]
                                    Html.input [
                                        prop.placeholder "Room Reference"
                                        prop.type' "text"
                                        prop.ref roomRef
                                        prop.disabled (Deferred.inProgress joinState)
                                    ]
                                ]
                            ]
                            Html.div [
                                prop.className [ 
                                    Sem.ui 
                                    Sem.blue
                                    Sem.fluid
                                    "submit" 
                                    Sem.button 
                                    if Deferred.inProgress joinState 
                                    then Sem.loading
                                ]
                                prop.text "Request Access"
                                prop.disabled (Deferred.inProgress joinState)
                                prop.onClick(fun _ -> startJoin())
                            ]                    
                            Html.div [
                                prop.className [ Sem.ui; Sem.divider ]
                            ]
                            Html.a [
                                prop.className [ Sem.ui; Sem.button; Sem.fluid ]
                                prop.href "#"
                                prop.onClick goToUrl
                                prop.text "Back"
                                prop.disabled (Deferred.inProgress joinState)
                            ]   
                        ]        
                    ]                        
                ]
            ]
        ]
    ])