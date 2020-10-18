module Pages.RecoverPassword.View

open Feliz
open Feliz.UseDeferred

open App.Style
open App.Routing

let private recoverPassword' email = async {
    do! Async.Sleep 5000
    if email = "user@test.com"
    then return Ok "user"
    else return Error "Restricted access"
}

let recoverPassword = React.functionComponent("RecoverPassword", fun () ->
    let (recoverState, setRecoverState) = React.useState(Deferred.HasNotStartedYet)
    let emailRef = React.useInputRef()

    let recover () =
        match emailRef.current with
        | Some email -> recoverPassword' email.value
        | _ -> failwith "Component hasn't been initialized"

    let startRecover = React.useDeferredCallback(recover, setRecoverState)

    let message =
        match recoverState with
        | Deferred.HasNotStartedYet -> Html.none
        | Deferred.InProgress -> Html.none
        | Deferred.Failed error ->
            Html.div [
                prop.className [ Sem.ui; Sem.attached; Sem.error; Sem.message ]
                prop.children [
                    Html.i [
                        prop.className [ Sem.icon; Sem.times; Sem.circle ]
                    ]
                    Html.text (sprintf "Internal error: %s" error.Message)
                ]
            ]

        | Deferred.Resolved (Ok user) ->
            Html.div [
                prop.className [ Sem.ui; Sem.attached; Sem.success; Sem.message ]
                prop.children [
                    Html.i [
                        prop.className [ Sem.icon; Sem.check; Sem.circle ]
                    ]
                    Html.text "Recovery letter sent"
                ]
            ]

        | Deferred.Resolved (Error error) ->
            Html.div [
                prop.className [ Sem.ui; Sem.attached; Sem.error; Sem.message ]
                prop.children [
                    Html.i [
                        prop.className [ Sem.icon; Sem.user; Sem.times ]
                    ]
                    Html.text (sprintf "Recover error: %s" error)
                ]
            ]
    
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
                                prop.text "Recover access to Scrum Poker!"
                            ]
                            Html.p [ prop.text "Specify your Email address to get a recovery letter" ]
                        ]
                    ]
                    message
                    Html.form [
                        prop.className [ Sem.ui; Sem.form; Sem.attached; Sem.fluid; Sem.segment ]
                        prop.children [
                            Html.div [
                                prop.className Sem.field
                                prop.children [
                                    Html.label [ prop.text "Email Address" ]
                                    Html.input [
                                        prop.placeholder "Email Address"
                                        prop.type' "email"
                                        prop.ref emailRef
                                        prop.disabled (Deferred.inProgress recoverState)
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
                                    if Deferred.inProgress recoverState 
                                    then Sem.loading
                                ]
                                prop.text "Send Email"
                                prop.disabled (Deferred.inProgress recoverState)
                                prop.onClick(fun _ -> startRecover())
                            ]                    
                            Html.div [
                                prop.className [ Sem.ui; Sem.divider ]
                            ]
                            Html.a [
                                prop.className [ Sem.ui; Sem.button; Sem.fluid ]
                                prop.href "#"
                                prop.onClick goToUrl
                                prop.text "Back"
                                prop.disabled (Deferred.inProgress recoverState)
                            ]   
                        ]        
                    ]                        
                ]
            ]
        ]
    ])