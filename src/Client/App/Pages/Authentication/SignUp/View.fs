module Pages.SignUp.View

open Feliz
open Feliz.UseDeferred
open Feliz.UseElmish
open Elmish

open App.Style
open App.Routing
open App.Components.FormField

type SignUpPageModel =
    { Text : string }

type SignUpPageMsg =
    | UserSignedUp of string

let init () =
    let signUp =
        { Text = "Sign Up" }
    signUp, Cmd.none

let update msg model =
    match msg with
    | UserSignedUp user ->
        model, Cmd.none

type private SignUpPageProps = 
    { Model    : SignUpPageModel
      Dispatch : SignUpPageMsg -> unit }

type private SignUpFormModel =
    { FirstNameField       : FieldModel
      LastNameField        : FieldModel
      EmailField           : FieldModel
      PasswordField        : FieldModel
      ConfirmPasswordField : FieldModel
      RememberMeField      : FieldModel
      SignUpState          : Deferred<Result<string, string>> }
    member this.InProgress =
        Deferred.inProgress this.SignUpState

type private SignUpFormMsg =
    | FirstNameMsg       of FieldMsg
    | LastNameMsg        of FieldMsg    
    | EmailMsg           of FieldMsg
    | PasswordMsg        of FieldMsg
    | ConfirmPasswordMsg of FieldMsg
    | RememberMeMsg      of FieldMsg
    | SignUpStateChanged of Deferred<Result<string, string>>

let private initForm () = 
    let model = 
        { FirstNameField       = FieldModel.RequiredInitial Text     "First Name"
          LastNameField        = FieldModel.RequiredInitial Text     "Last Name"
          EmailField           = FieldModel.RequiredInitial Email    "Email Address"
          PasswordField        = FieldModel.RequiredInitial Password "Password"
          ConfirmPasswordField = FieldModel.RequiredInitial Password "Confirm Password"
          RememberMeField      = FieldModel.RequiredInitial Check    "Remember Me"
          SignUpState          = Deferred.HasNotStartedYet }
    model, Cmd.none

let private signUpApi firstName lastName email password = async {
    do! Async.Sleep 5000
    if firstName = "admin" && email = "admin@test.com" && password = "admin"
    then return  Ok "admin"
    else return Error "Credentials incorrect"
}

let private updateForm msg model =
    match msg with
    | FirstNameMsg msg ->
        match msg with
        | StateChanged state ->
            let fieldModel = { model.FirstNameField with State = state }
            { model with FirstNameField = fieldModel }, Cmd.none
    | LastNameMsg msg ->
        match msg with
        | StateChanged state ->
            let fieldModel = { model.LastNameField with State = state }
            { model with LastNameField = fieldModel }, Cmd.none
    | EmailMsg msg ->
        match msg with
        | StateChanged state ->
            let fieldModel = { model.EmailField with State = state }
            { model with EmailField = fieldModel }, Cmd.none
    | PasswordMsg msg ->
        match msg with
        | StateChanged state ->
            let fieldModel = { model.PasswordField with State = state }
            { model with PasswordField = fieldModel }, Cmd.none
    | ConfirmPasswordMsg msg ->
        match msg with
        | StateChanged state ->
            let fieldModel = { model.ConfirmPasswordField with State = state }
            { model with ConfirmPasswordField = fieldModel }, Cmd.none
    | RememberMeMsg msg ->
        match msg with
        | StateChanged state ->
            let fieldModel = { model.RememberMeField with State = state }
            { model with RememberMeField = fieldModel }, Cmd.none
    | SignUpStateChanged state ->
        let disabled = Deferred.inProgress state
        let updatedModel = 
            { model with SignUpState          = state 
                         FirstNameField       = { model.FirstNameField       with Disabled = disabled }
                         LastNameField        = { model.LastNameField        with Disabled = disabled }
                         EmailField           = { model.EmailField           with Disabled = disabled }
                         PasswordField        = { model.PasswordField        with Disabled = disabled }
                         ConfirmPasswordField = { model.ConfirmPasswordField with Disabled = disabled }
                         RememberMeField      = { model.RememberMeField      with Disabled = disabled }}                            

        updatedModel, Cmd.none

let private signUp' = React.functionComponent("Sign Up", fun (props : SignUpPageProps) ->

    let formState, formDispatch = React.useElmish(initForm, updateForm, [| |])
    let _, setSignUpState = React.useState(Deferred.HasNotStartedYet)

    let startSignUp = 
        let setState state =             
            setSignUpState state    
            formDispatch (SignUpStateChanged state)   
        let callApi () = 
            signUpApi "" "" "" ""                 
        React.useDeferredCallback(callApi, setState)

    let message =
        match formState.SignUpState with
        | Deferred.HasNotStartedYet       -> Html.none
        | Deferred.InProgress             -> Html.none
        | Deferred.Failed error           -> Html.attachedError   (sprintf "Internal error: %s" error.Message)
        | Deferred.Resolved (Ok user)     -> Html.attachedSuccess (sprintf "User %s signed up" user)
        | Deferred.Resolved (Error error) -> Html.attachedError   (sprintf "Sign up error: %s" error)
    
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
                                    formField formState.FirstNameField (FirstNameMsg >> formDispatch)
                                    formField formState.LastNameField  (LastNameMsg  >> formDispatch)
                                ]
                            ]
                            formField formState.EmailField           (EmailMsg           >> formDispatch)
                            formField formState.PasswordField        (PasswordMsg        >> formDispatch)
                            formField formState.ConfirmPasswordField (ConfirmPasswordMsg >> formDispatch)
                            formField formState.RememberMeField      (RememberMeMsg      >> formDispatch)
                            Html.div [
                                prop.className [ 
                                    Sem.ui 
                                    Sem.blue
                                    Sem.fluid
                                    "submit" 
                                    if formState.InProgress
                                    then Sem.disabled
                                    Sem.button 
                                    if formState.InProgress 
                                    then Sem.loading
                                ]
                                prop.text "Sign Up"
                                prop.onClick (fun _ -> startSignUp ())
                            ]
                            Html.divider "Or"
                            Html.button [
                                prop.className [ 
                                    Sem.ui 
                                    Sem.google
                                    Sem.fluid
                                    Sem.plus
                                    if formState.InProgress
                                    then Sem.disabled
                                    Sem.button 
                                ]
                                prop.children [
                                    Html.i [
                                        prop.className [ Sem.icon; Sem.google; Sem.plus ]
                                    ]
                                    Html.text "Sign Up with Google"
                                ]
                            ]
                            Html.divider ()
                            Html.a [
                                prop.className [ 
                                    Sem.ui
                                    Sem.fluid 
                                    if formState.InProgress
                                    then Sem.disabled 
                                    Sem.button 
                                ]
                                prop.href (routeHash SignInRoute)
                                prop.onClick goToUrl
                                prop.text "Back to Sign In"
                            ]   
                        ]        
                    ]          
                ]
            ]
        ]
    ])

let signUp model dispatch = signUp' { Model = model; Dispatch = dispatch }