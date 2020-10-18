module Pages.SignUp.View

open Feliz
open Feliz.UseDeferred

open App.Style
open App.Routing

let private signUp' firstName lastName email password = async {
    do! Async.Sleep 5000
    if firstName = "admin" && email = "admin@test.com" && password = "admin"
    then return Ok "admin"
    else return Error "Credentials incorrect"
}

let signUp = React.functionComponent("Sign Up", fun () ->
    let (signupState, setSignUpState) = React.useState(Deferred.HasNotStartedYet)
    let firstNameRef = React.useInputRef()
    let lastNameRef = React.useInputRef()
    let emailRef = React.useInputRef()
    let passwordRef = React.useInputRef()

    let signUp() =
        match firstNameRef.current, lastNameRef.current, emailRef.current, passwordRef.current with
        | Some firstName, Some lastName, Some email, Some password -> signUp' firstName.value lastName.value email.value password.value
        | _ -> failwith "Component hasn't been initialized"

    let startSignUp = React.useDeferredCallback(signUp, setSignUpState)

    let message =
        match signupState with
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
                    Html.text (sprintf "User %s signed up" user)
                ]
            ]

        | Deferred.Resolved (Error error) ->
            Html.div [
                prop.className [ Sem.ui; Sem.attached; Sem.error; Sem.message ]
                prop.children [
                    Html.i [
                        prop.className [ Sem.icon; Sem.user; Sem.times ]
                    ]
                    Html.text (sprintf "Sign up error: %s" error)
                ]
            ]
    
    Html.div [
        prop.className [ Sem.ui; Sem.text; Sem.container; Sem.middle; Sem.aligned ]
        prop.children [
            Html.div [
                prop.className [ Sem.ui; Sem.piled; Sem.segments ]
                prop.children [
                    Html.div [
                        prop.className [ Sem.ui; Sem.attached; Sem.message; ]
                        prop.children [
                            Html.div [
                                prop.className Sem.header
                                prop.text "Sign up to Scrum Poker!"
                            ]
                            Html.p [ prop.text "Fill out the form below to sign up for a new account" ]
                        ]
                    ]
                    message
                    Html.form [
                        prop.className [ Sem.ui; Sem.form; Sem.attached; Sem.fluid; Sem.segment ]
                        prop.children [
                            Html.div [
                                prop.className [ Sem.two; Sem.fields ]
                                prop.children [
                                    Html.div [
                                        prop.className [ Sem.field ]
                                        prop.children [
                                            Html.label [ prop.text "First Name" ]
                                            Html.input [
                                                prop.placeholder "First Name"
                                                prop.type' "text"
                                                prop.ref firstNameRef
                                                prop.disabled (Deferred.inProgress signupState)
                                            ]
                                        ]                    
                                    ]
                                    Html.div [
                                        prop.className Sem.field
                                        prop.children [
                                            Html.label [ prop.text "Last Name" ]
                                            Html.input [
                                                prop.placeholder "Last Name"
                                                prop.type' "text"
                                                prop.ref lastNameRef
                                                prop.disabled (Deferred.inProgress signupState)
                                            ]
                                        ]                    
                                    ]
                                ]
                            ]
                            Html.div [
                                prop.className Sem.field
                                prop.children [
                                    Html.label [ prop.text "Email Address" ]
                                    Html.input [
                                        prop.placeholder "Email Address"
                                        prop.type' "email"
                                        prop.ref emailRef
                                        prop.disabled (Deferred.inProgress signupState)
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
                                        prop.disabled (Deferred.inProgress signupState)
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
                                                prop.disabled (Deferred.inProgress signupState)
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
                                    if Deferred.inProgress signupState 
                                    then Sem.loading
                                ]
                                prop.text "Sign Up"
                                prop.disabled (Deferred.inProgress signupState)
                                prop.onClick(fun _ -> startSignUp())
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
                                prop.disabled (Deferred.inProgress signupState)
                                prop.children [
                                    Html.i [
                                        prop.className [ Sem.icon; Sem.google; Sem.plus ]
                                    ]
                                    Html.text "Sign Up with Google"
                                ]
                            ]
                            Html.div [
                                prop.className [ Sem.ui; Sem.divider ]
                            ]
                            Html.a [
                                prop.className [ Sem.ui; Sem.fluid; Sem.button ]
                                prop.href (routeHash SignInRoute)
                                prop.onClick goToUrl
                                prop.text "Back to Sign In"
                                prop.disabled (Deferred.inProgress signupState)
                            ]   
                        ]        
                    ]          
                ]
            ]
        ]
    ])