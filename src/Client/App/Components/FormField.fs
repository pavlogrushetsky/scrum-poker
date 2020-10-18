module App.Components.FormField

open Feliz
open Browser.Types

open App.Style

type FormFieldProps =
    { InputType : string
      Label : string
      Placeholder : string
      Disabled : bool }

let formField' model = React.forwardRef("formField", fun ((), (ref : IRefValue<option<HTMLInputElement>>)) ->
    let (_, setInputState) = React.useState("")
    let (errorState, setErrorState) = React.useState(false)

    let onChange () =
        match ref.current with
        | Some input ->
            setInputState input.value
            (System.String.IsNullOrEmpty(input.value) || System.String.IsNullOrWhiteSpace(input.value)) 
            |> setErrorState
        | None -> ()

    Html.div [
        prop.className [ 
            Sem.field 
            if errorState 
            then Sem.error
        ]
        prop.children [
            Html.label [ prop.text model.Label ]
            Html.input [
                prop.placeholder model.Placeholder
                prop.type' model.InputType
                prop.ref ref
                prop.disabled model.Disabled
                prop.onChange (fun (_ : Event) -> onChange ())
            ]
        ]                    
    ])

let textField label ref disabled = 
    formField' 
        { InputType = "text"
          Label = label
          Placeholder = label
          Disabled = disabled } ((), ref)