module Pages.SignIn.View

open Feliz
open Feliz.UseDeferred

open App.Style
open App.Routing

let private signIn' email password = async {
    do! Async.Sleep 5000
    if email = "admin@test.com" && password = "admin"
    then return Ok "admin"
    else return Error "Credentials incorrect"
}

let signIn = React.functionComponent("SignIn", fun () ->
    let (signInState, setSignInState) = React.useState(Deferred.HasNotStartedYet)  
    let emailRef = React.useInputRef()
    let passwordRef = React.useInputRef()

    let signIn() =
        match emailRef.current, passwordRef.current with
        | Some email, Some password -> signIn' email.value password.value
        | _ -> failwith "Component hasn't been initialized"

    let startSignIn = React.useDeferredCallback(signIn, setSignInState)

    let message =
        match signInState with
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
                    Html.text (sprintf "User %s signed in" user)
                ]
            ]

        | Deferred.Resolved (Error error) ->
            Html.div [
                prop.className [ Sem.ui; Sem.attached; Sem.error; Sem.message ]
                prop.children [
                    Html.i [
                        prop.className [ Sem.icon; Sem.user; Sem.times ]
                    ]
                    Html.text (sprintf "Sign in error: %s" error)
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
                                prop.text "Sign in to Scrum Poker!"
                            ]
                            Html.p [ prop.text "Fill out the form below to sign into your account" ]
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
                                        prop.disabled (Deferred.inProgress signInState)
                                    ]
                                ]
                            ]
                            Html.div [
                                prop.className Sem.field
                                prop.children [
                                    Html.label [ prop.text "Password" ]
                                    Html.input [
                                        prop.placeholder "Password"
                                        prop.type' "password"
                                        prop.ref passwordRef
                                        prop.disabled (Deferred.inProgress signInState)
                                    ]
                                ]
                            ]
                            Html.div [
                                prop.className [ "inline"; Sem.field ]
                                prop.children [
                                    Html.div [
                                        prop.className [ Sem.ui; Sem.checkbox ]
                                        prop.children [
                                            Html.input [
                                                prop.type' "checkbox"
                                                prop.id "rememberMe"
                                                prop.disabled (Deferred.inProgress signInState)
                                            ]
                                            Html.label [
                                                prop.htmlFor "rememberMe"
                                                prop.text "Remember Me"
                                            ]
                                        ]
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
                                    if Deferred.inProgress signInState 
                                    then Sem.loading
                                ]
                                prop.text "Sign In"
                                prop.disabled (Deferred.inProgress signInState)
                                prop.onClick(fun _ -> startSignIn())
                            ]
                            Html.div [
                                prop.className [ Sem.ui; Sem.horizontal; Sem.divider ]
                                prop.text "Or"
                            ]
                            Html.button [
                                prop.className [ 
                                    Sem.ui 
                                    Sem.google
                                    Sem.fluid
                                    Sem.plus
                                    Sem.button 
                                ]
                                prop.disabled (Deferred.inProgress signInState)
                                prop.children [
                                    Html.i [
                                        prop.className [ Sem.icon; Sem.google; Sem.plus ]
                                    ]
                                    Html.text "Sign In with Google"
                                ]
                            ] 
                            Html.div [
                                prop.className [ Sem.ui; Sem.horizontal; Sem.divider ]
                                prop.text "Or"
                            ]
                            Html.a [
                                prop.className [ Sem.ui; Sem.button; Sem.grey; Sem.fluid ]
                                prop.href (routeHash JoinRoute)
                                prop.onClick goToUrl
                                prop.text "Join the Room"
                                prop.disabled (Deferred.inProgress signInState)
                            ]  
                            Html.div [
                                prop.className [ Sem.ui; Sem.divider ]
                            ]
                            Html.a [
                                prop.className [ Sem.ui; Sem.button; Sem.fluid ]
                                prop.href (routeHash RecoverPasswordRoute)
                                prop.onClick goToUrl
                                prop.text "Forgot Password?"
                                prop.disabled (Deferred.inProgress signInState)
                            ]  
                            Html.div [
                                prop.className [ Sem.ui; Sem.hidden; Sem.divider ]
                            ]
                            Html.a [
                                prop.className [ Sem.ui; Sem.button; Sem.fluid ]
                                prop.href (routeHash SignUpRoute)
                                prop.onClick goToUrl
                                prop.text "Create an Account"
                                prop.disabled (Deferred.inProgress signInState)
                            ]   
                        ]        
                    ]                        
                ]
            ]
        ]
    ])