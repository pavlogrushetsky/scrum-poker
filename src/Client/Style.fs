module Style

open Feliz
open Zanaptak.TypedCssClasses

type Bs = CssClasses<"https://unpkg.com/bootstrap-material-design@4.1.1/dist/css/bootstrap-material-design.min.css", Naming.Verbatim>     

type Fa = CssClasses<"https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.14.0/css/all.min.css", Naming.Verbatim>

type prop with
    static member dataToggle value = prop.custom ("data-toggle", value) 
    static member dataTarget value = prop.custom ("data-target", value)
    static member scope value = prop.custom ("scope", value)