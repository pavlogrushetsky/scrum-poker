module App.Style

open Feliz
open Zanaptak.TypedCssClasses

type Bs = CssClasses<"https://unpkg.com/bootstrap-material-design@4.1.1/dist/css/bootstrap-material-design.min.css", Naming.Verbatim>     

type Fa = CssClasses<"https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.14.0/css/all.min.css", Naming.Verbatim>

type prop with
    static member dataToggle value = prop.custom ("data-toggle", value) 
    static member dataTarget value = prop.custom ("data-target", value)
    static member scope value = prop.custom ("scope", value)

type Html with
    static member route (name : string) icon target current =
        Html.li [
            prop.className [ 
                Bs.``nav-item``
                if target = current then Bs.active
            ]
            prop.children [
                Html.a [
                    prop.className [ Bs.``nav-link`` ]
                    prop.href ""
                    prop.children [
                        Html.text name
                        if target = current then
                            Html.span [
                                prop.className Bs.``sr-only``
                                prop.text "(current)"
                            ]
                    ]
                ]
            ]
        ]
    static member icon name =
        Html.i [
            prop.className [ Fa.fab; Fa.``fa-lg``; name ]
            prop.style [ style.marginRight 3 ]
        ]