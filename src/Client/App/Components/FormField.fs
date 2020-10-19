module App.Components.FormField

open Elmish
open Feliz

open App.Style

type FieldType =
    | Text
    | Email
    | Password

type FieldModel = 
    { FieldType       : FieldType
      Label           : string
      Placeholder     : string
      Disabled        : bool
      Required        : bool
      ErrorMessage    : string option }

type FieldValidationState =
    | Valid
    | Invalid

type FieldMsg =
    | ValidationStateChanged of FieldValidationState

type private FieldProps =
    { Model    : FieldModel
      Dispatch : FieldMsg -> unit }

let private firmField' = React.functionComponent("FormField", fun (props : FieldProps) ->      
    let fieldType =
        match props.Model.FieldType with
        | Text -> "text"
        | Email -> "email"
        | Password -> "password"

    let errorMessage =
        match props.Model.ErrorMessage with
        | Some error -> error
        | None -> sprintf "%s is required" props.Model.Label

    let (validationError, setValidationError) = React.useState(false)

    let validate value =
        if System.String.IsNullOrEmpty(value) 
        then
            setValidationError true
            props.Dispatch (ValidationStateChanged Invalid)
        else
            setValidationError false
            props.Dispatch (ValidationStateChanged Valid)    
   
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
                prop.disabled props.Model.Disabled
                prop.onChange (fun (value : string) -> if props.Model.Required then validate value)
            ]
            if validationError then 
                Html.div [
                    prop.className [ Sem.ui; Sem.pointing; Sem.red; Sem.basic; Sem.label ]
                    prop.text errorMessage
                ]
        ]                    
    ])

let formField model dispatch = firmField' { Model = model; Dispatch = dispatch }