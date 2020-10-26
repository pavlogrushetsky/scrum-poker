module App.Style

open Feliz
open Zanaptak.TypedCssClasses

open App.Routing

type Bs = CssClasses<"https://unpkg.com/bootstrap-material-design@4.1.1/dist/css/bootstrap-material-design.min.css", Naming.Verbatim>     

type Sem = CssClasses<"https://cdn.jsdelivr.net/npm/semantic-ui@2.4.2/dist/semantic.min.css", Naming.Verbatim>  

type Fa = CssClasses<"https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.14.0/css/all.min.css", Naming.Verbatim>

type prop with
    static member dataToggle value = prop.custom ("data-toggle", value) 
    static member dataTarget value = prop.custom ("data-target", value)
    static member scope value = prop.custom ("scope", value)
    static member dataText value = prop.custom ("data-text", value)

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
                    prop.href (routeHash target)
                    prop.onClick goToUrl
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
    static member attachedMessage (text : string) style =
        Html.div [
            prop.className [ Sem.ui; Sem.attached; style; Sem.message ]
            prop.children [
                Html.i [
                    prop.className [ Sem.icon; Sem.times; Sem.circle ]
                ]
                Html.text text
            ]
        ]
    static member attachedError (text : string) =
        Html.attachedMessage text Sem.error
    static member attachedWarning (text : string) =
        Html.attachedMessage text Sem.warning
    static member attachedInfo (text : string) =
        Html.attachedMessage text Sem.info
    static member attachedSuccess (text : string) =
        Html.attachedMessage text Sem.success
    static member divider (text : string) =
        let withText = not (System.String.IsNullOrEmpty(text))
        Html.div [
            prop.className [ 
                Sem.ui 
                if withText
                then Sem.horizontal
                Sem.divider 
            ]
            if withText
            then prop.text text
        ]
    static member divider () =
        Html.divider ""