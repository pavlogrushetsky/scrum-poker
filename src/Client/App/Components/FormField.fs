module App.Components.FormField

open Elmish
open Feliz

open App.Style

type FieldType =
    | Text
    | Email
    | Password
    | Check

type FieldValidationState =
    | Valid
    | Invalid

type FieldState =
    { Value : string
      ValidationState : FieldValidationState }
    static member Default fieldType =
        let defaultValue, defaultState =
            match fieldType with
            | Check -> "off", Valid
            | _ -> "", Invalid
        { Value = defaultValue
          ValidationState = defaultState }

type FieldModel = 
    { FieldType    : FieldType
      Label        : string
      Placeholder  : string
      State        : FieldState
      Disabled     : bool
      Required     : bool
      ErrorMessage : string option }
    static member RequiredInitial fieldType name =
        { FieldType       = fieldType
          Label           = name
          Placeholder     = name
          State           = FieldState.Default fieldType
          Disabled        = false
          Required        = true
          ErrorMessage    = None }

type FieldMsg =
    | StateChanged of FieldState

type private FieldProps =
    { Model    : FieldModel
      Dispatch : FieldMsg -> unit }

let private firmField' = React.functionComponent("FormField", fun (props : FieldProps) ->      
    let fieldType =
        match props.Model.FieldType with
        | Text -> "text"
        | Email -> "email"
        | Password -> "password"
        | Check -> "checkbox"

    let errorMessage =
        match props.Model.ErrorMessage with
        | Some error -> error
        | None -> sprintf "%s is required" props.Model.Label

    let (validationError, setValidationError) = React.useState(false)

    let validate value =
        let validationState = 
            match System.String.IsNullOrEmpty(value) && props.Model.Required with
            | true -> 
                setValidationError true
                Invalid
            | false -> 
                setValidationError false
                Valid
        let state = 
            { Value = value
              ValidationState = validationState }

        props.Dispatch (StateChanged state)   
    
    match props.Model.FieldType with
    | Check ->
        Html.div [
            prop.className [ "inline"; Sem.field ]
            prop.children [
                Html.div [
                    prop.className [ Sem.ui; Sem.checkbox ]
                    prop.children [
                        Html.input [
                            prop.type' fieldType
                            prop.id "rememberMe"
                            prop.isChecked (props.Model.State.Value = "on")
                            prop.disabled props.Model.Disabled
                            prop.onChange (fun (check : bool) -> validate (if check then "on" else "off"))
                        ]
                        Html.label [
                            prop.htmlFor "rememberMe"
                            prop.text props.Model.Label
                        ]
                    ]
                ]
            ]
        ]
    | _ ->
        Html.div [
            prop.className [ 
                Sem.field 
                if validationError
                then Sem.error
            ]
            prop.children [
                Html.label [ prop.text props.Model.Label ]
                Html.input [
                    prop.placeholder props.Model.Placeholder
                    prop.type' fieldType
                    prop.value props.Model.State.Value
                    prop.disabled props.Model.Disabled
                    prop.onChange validate
                ]
                if validationError then 
                    Html.div [
                        prop.className [ Sem.ui; Sem.pointing; Sem.red; Sem.basic; Sem.label ]
                        prop.text errorMessage
                    ]
            ]                    
        ])

let formField model dispatch = firmField' { Model = model; Dispatch = dispatch }