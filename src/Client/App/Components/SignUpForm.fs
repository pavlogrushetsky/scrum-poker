module App.Components.SignUpForm

open Feliz
open Feliz.UseDeferred

open App.Style

let private signUp name email password = async {
    do! Async.Sleep 5000
    if name = "admin" && email = "admin@test.com" && password = "admin"
    then return Ok "admin"
    else return Error "Credentials incorrect"
}

let signUpForm = React.functionComponent("SignUpForm", fun () ->
    let (signupState, setSignUpState) = React.useState(Deferred.HasNotStartedYet)
    let nameRef = React.useInputRef()
    let emailRef = React.useInputRef()
    let passwordRef = React.useInputRef()

    let signUp() =
        match nameRef.current, emailRef.current, passwordRef.current with
        | Some name, Some email, Some password -> signUp name.value email.value password.value
        | _ -> failwith "Component hasn't been initialized"

    let startSignUp = React.useDeferredCallback(signUp, setSignUpState)

    let message =
        match signupState with
        | Deferred.HasNotStartedYet -> Html.none
        | Deferred.InProgress -> Html.none
        | Deferred.Failed error ->
            Html.p [
                prop.style [ style.color.red ]
                prop.text (sprintf "Internal error: %s" error.Message)
            ]

        | Deferred.Resolved (Ok user) ->
            Html.p [
                prop.style [ style.color.green ]
                prop.text (sprintf "User %s logged in" user)
            ]

        | Deferred.Resolved (Error error) ->
            Html.p [
                prop.style [ style.color.red ]
                prop.text (sprintf "Login error: %s" error)
            ]
    
    Html.form [
        prop.children [
            message
            Html.div [
                prop.className Bs.``form-group``
                prop.children [
                    Html.label [
                        prop.htmlFor "nameInput"
                        prop.className Bs.``bmd-label-floating``
                        prop.text "Name"
                    ]
                    Html.input [
                        prop.type' "text"
                        prop.className Bs.``form-control``
                        prop.id "nameInput"
                        prop.ref nameRef
                        prop.disabled (Deferred.inProgress signupState)
                    ]
                ]
            ]
            Html.div [
                prop.className Bs.``form-group``
                prop.children [
                    Html.label [
                        prop.htmlFor "emailInput"
                        prop.className Bs.``bmd-label-floating``
                        prop.text "Email"
                    ]
                    Html.input [
                        prop.type' "email"
                        prop.className Bs.``form-control``
                        prop.id "emailInput"
                        prop.ref emailRef
                        prop.disabled (Deferred.inProgress signupState)
                    ]
                ]
            ]
            Html.div [
                prop.className Bs.``form-group``
                prop.children [
                    Html.label [
                        prop.htmlFor "passwordInput"
                        prop.className Bs.``bmd-label-floating``
                        prop.text "Password"
                    ]
                    Html.input [
                        prop.type' "password"
                        prop.className Bs.``form-control``
                        prop.id "passwordInput"
                        prop.ref passwordRef
                        prop.disabled (Deferred.inProgress signupState)
                    ]
                ]
            ]
            Html.div [
                prop.className Bs.``form-group``
                prop.children [
                    Html.label [
                        prop.htmlFor "repeatPasswordInput"
                        prop.className Bs.``bmd-label-floating``
                        prop.text "Repeat Password"
                    ]
                    Html.input [
                        prop.type' "password"
                        prop.className Bs.``form-control``
                        prop.id "repeatPasswordInput"
                        prop.disabled (Deferred.inProgress signupState)
                    ]
                ]
            ]
            Html.div [
                prop.className [ Bs.checkbox; Bs.``my-4`` ]
                prop.children [
                    Html.label [                                                                         
                        prop.children [
                            Html.input [
                                prop.type' "checkbox"
                                prop.disabled (Deferred.inProgress signupState)
                            ]
                            Html.text " Remember Me"
                        ]                                                                                                                                               
                    ]
                ]
            ]
            Html.button [
                prop.type' "button"
                prop.className [ Bs.btn; Bs.``btn-primary``; Bs.``btn-raised``; Bs.``btn-block``; ]
                prop.disabled (Deferred.inProgress signupState)
                prop.onClick(fun _ -> startSignUp())
                prop.children [
                    if Deferred.inProgress signupState
                    then Html.span [
                        prop.className "spinner-grow spinner-grow-sm"
                        prop.role "status"
                        prop.ariaHidden true
                    ]
                    Html.text "Sign Up"
                ]
            ]                                                                                                                                                                                                   
        ]
    ])
