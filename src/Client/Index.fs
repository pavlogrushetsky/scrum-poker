module Index

open Elmish
open Fable.React
open Fable.React.Props
open Feliz
open Thoth.Json
open Thoth.Fetch
open Browser.Types
open Shared
open System

open Style

/// Status of the websocket.
type WsSender = WebSocketClientMessage -> unit
type BroadcastMode = ViaWebSocket | ViaHTTP
type ConnectionState =
    | DisconnectedFromServer | ConnectedToServer of WsSender | Connecting
    member this.IsConnected =
        match this with
        | ConnectedToServer _ -> true
        | DisconnectedFromServer | Connecting -> false

type Model =
    { MessageToSend : string
      ReceivedMessages : Message list
      ConnectionState : ConnectionState }

type Msg =
    | ReceivedFromServer of WebSocketServerMessage
    | ConnectionChange of ConnectionState
    | MessageChanged of string
    | Broadcast of BroadcastMode * string
    | SyncState

module Channel = 
    open Browser.WebSocket

    let inline decode<'a> x = x |> unbox<string> |> Thoth.Json.Decode.Auto.unsafeFromString<'a>
    let buildWsSender (ws:WebSocket) : WsSender =
        fun (message:WebSocketClientMessage) ->
            let message = { Topic = ""; Ref = ""; Payload = message }
            let message = Thoth.Json.Encode.Auto.toString(0, message)
            ws.send message

    let subscription _ =
        let sub dispatch =
            /// Handles push messages from the server and relays them into Elmish messages.
            let onWebSocketMessage (msg:MessageEvent) =
                let msg = msg.data |> decode<{| Payload : string |}>
                msg.Payload
                |> decode<WebSocketServerMessage>
                |> ReceivedFromServer
                |> dispatch

            /// Continually tries to connect to the server websocket.
            let rec connect () =
                let ws = WebSocket.Create "ws://localhost:8085/channel"
                ws.onmessage <- onWebSocketMessage
                ws.onopen <- (fun ev ->
                    dispatch (ConnectionChange (ConnectedToServer (buildWsSender ws)))
                    printfn "WebSocket opened")
                ws.onclose <- (fun ev ->
                    dispatch (ConnectionChange DisconnectedFromServer)
                    printfn "WebSocket closed. Retrying connection"
                    promise {
                        do! Promise.sleep 2000
                        dispatch (ConnectionChange Connecting)
                        connect() })

            connect()

        Cmd.ofSub sub

let init () =
    { MessageToSend = null
      ConnectionState = DisconnectedFromServer
      ReceivedMessages = [] }, Cmd.none

let update msg model : Model * Cmd<Msg> =
    match msg with
    | MessageChanged msg ->
        { model with MessageToSend = msg }, Cmd.none
    | ConnectionChange status ->
        let model = { model with ConnectionState = status }
        match status with
        | ConnectedToServer _ ->
            model, Cmd.ofMsg SyncState
        | _ ->
            model, Cmd.none
    | ReceivedFromServer message ->
        match message with
        | BroadcastMessage msg -> 
            { model with ReceivedMessages = msg :: model.ReceivedMessages }, Cmd.none
        | BroadcastMessages msgs ->
            { model with ReceivedMessages = msgs }, Cmd.none
    | Broadcast (ViaWebSocket, msg) ->
        match model.ConnectionState with
        | ConnectedToServer sender -> sender (TextMessage msg)
        | _ -> ()
        model, Cmd.none
    | SyncState ->
        match model.ConnectionState with
        | ConnectedToServer sender -> sender (GetMessages 1)
        | _ -> ()
        model, Cmd.none
    | Broadcast (ViaHTTP, msg) ->
        let post = Fetch.post("/api/broadcast", msg)
        model, Cmd.OfPromise.result post

module ViewParts =
    let drawStatus connectionState =
        let className, text =
            match connectionState with
            | DisconnectedFromServer -> "danger", "Disconnected from server"
            | Connecting -> "secondary", "Connecting..."
            | ConnectedToServer _ -> "success", "Connected to server"
        span [ ClassName (sprintf "badge badge-%s" className) ] [ str text ]

let inline ReactComponent name render = FunctionComponent.Of(render, name, equalsButFunctions)

type Menu = {
    CurrentRoute : string
}

let private routeItem (name : string) iconName route current =
    Html.li [
        prop.className [ 
            Bs.``nav-item``
            if route = current then Bs.active
        ]
        prop.children [
            Html.a [
                prop.className [ Bs.``nav-link`` ]
                prop.href ""
                prop.children [
                    Html.text name
                    if route = current then
                        Html.span [
                            prop.className Bs.``sr-only``
                            prop.text "(current)"
                        ]
                ]
            ]
        ]
    ]

let icon name =
    Html.i [
        prop.className [ Fa.fab; Fa.``fa-lg``; name ]
        prop.style [ style.marginRight 3 ]
    ]

let MenuComponent = ReactComponent "Menu" (fun (props : Menu) ->
    Html.nav [
        prop.className [ Bs.navbar; Bs.``navbar-expand-lg``; Bs.``navbar-dark``; Bs.``bg-primary``; Bs.``fixed-top`` ]
        prop.children [
            Html.div [
                prop.className Bs.container
                prop.children [
                    Html.a [
                        prop.className Bs.``navbar-brand``
                        prop.href ""
                        prop.text "Scrum Poker"
                    ]
                    Html.button [
                        prop.className Bs.``navbar-toggler``
                        prop.type' "button"
                        prop.dataToggle "collapse"
                        prop.dataTarget "#navbarNavDropdown"
                        prop.ariaControls "navbarNavDropdown"
                        prop.ariaExpanded false
                        prop.ariaLabel "Toggle navigation"
                        prop.children [
                            Html.span [
                                prop.className Bs.``navbar-toggler-icon``
                            ]
                        ]
                    ]
                    Html.div [
                        prop.className [ Bs.collapse; Bs.``navbar-collapse`` ]
                        prop.id "navbarNavDropdown"
                        prop.children [
                            Html.ul [
                                prop.className [ Bs.``navbar-nav``; Bs.``mr-auto`` ]
                                prop.children [
                                    routeItem "Home" "" "" ""
                                    routeItem "About" "compose" "" ""
                                ]
                            ]
                            Html.ul [
                                prop.className [ Bs.``navbar-nav``; Bs.``mr-md-2`` ]
                                prop.children [
                                    Html.li [
                                        prop.className Bs.``nav-item``
                                        prop.children [
                                            Html.a [
                                                prop.className Bs.``nav-link``
                                                prop.href "https://github.com/pavlogrushetsky/scrum-poker"
                                                prop.target "_blank"
                                                prop.children [
                                                    icon Fa.``fa-github``
                                                ]
                                            ]
                                        ]
                                    ]
                                ]
                            ]
                        ]
                    ]
                ]
            ]
        ]
    ])

let view (model : Model) (dispatch : Msg -> unit) =
    div [] [
        { CurrentRoute = "" } |> MenuComponent
        main [ Role "main"; ClassName "container" ] [
            h3 [] [ str "Send a message!" ]
            div [ ClassName "row" ] [
                div [ ClassName "col" ] [
                    div [ ClassName "form-group" ] [
                        label [ HtmlFor "messageInput"; ClassName "bmd-label-floating" ] [ str "Message" ]
                        input [ Typeof "text"; ClassName "form-control"; Id "messageInput"; OnChange (fun e -> dispatch(MessageChanged e.Value)) ]
                    ]                    
                ]
                div [ ClassName "col-md-auto my-auto" ] [
                    button [ 
                        Typeof "button"; 
                        ClassName "btn btn-outline-primary"; 
                        Disabled (String.IsNullOrEmpty model.MessageToSend || not model.ConnectionState.IsConnected)
                        OnClick (fun _ -> dispatch (Broadcast (ViaWebSocket, model.MessageToSend))) ] [ str "Broadcast"]
                ]
            ]
            div [ ClassName "row" ] [
                div [ ClassName "col" ] [ ViewParts.drawStatus model.ConnectionState ]               
            ]
            div [ ClassName "row" ] [
                div [ ClassName "col" ] [ 
                    match model.ReceivedMessages with
                    | [] ->
                        ()
                    | messages ->
                        Html.table [
                            prop.className [ Bs.table; "table-borderless"; Bs.``table-hover`` ]
                            prop.children [
                                Html.thead [
                                    prop.className "thead"
                                    prop.children [
                                        Html.tr [
                                            prop.children [
                                                Html.th [ 
                                                    prop.scope "col"
                                                    prop.text "Time"
                                                ]
                                                Html.th [ 
                                                    prop.scope "col"
                                                    prop.text "Message"
                                                ]
                                            ]
                                        ]
                                    ]
                                ]
                                Html.tbody [
                                    prop.children [
                                        for message in messages do
                                            Html.tr [
                                                prop.children [
                                                    Html.td [ prop.text (sprintf "%O" message.Time) ]
                                                    Html.td [ prop.text message.Text ]
                                                ]
                                            ]
                                    ]
                                ]
                            ]
                        ]
                ]               
            ]                   
        ]
        Html.footer [
            prop.style [
                style.bottom 0
                style.width (length.percent 100)
                style.height (length.px 60)
                style.lineHeight (length.px 60)
                style.backgroundColor "#fafafa"
            ]
            prop.children [
                Html.div [
                    prop.className Bs.container
                    prop.children [
                        Html.p [
                            prop.className Bs.``float-right``
                            prop.children [
                                Html.a [
                                    prop.href "#"
                                    prop.text "Back to top"
                                ]
                            ]
                        ]
                        Html.p [ prop.text "© 2020 Pavlo Hrushetskyi" ]
                    ]
                ]
            ]
        ]
    ]

module CustomEncoders =

    let inline addDummyCoder<'b> extrasIn =
        let typeName = string typeof<'b>
        let simpleEncoder(_ : 'b) = Encode.string (sprintf "%s function" typeName)
        let simpleDecoder = Decode.fail (sprintf "Decoding is not supported for %s type" typeName)
        extrasIn |> Extra.withCustom simpleEncoder simpleDecoder
        
    let inline buildExtras<'a> extraCoders =
        let myEncoder:Encoder<'a> = Encode.Auto.generateEncoder(extra = extraCoders)
        let myDecoder:Decoder<'a> = Decode.Auto.generateDecoder(extra = extraCoders)
        (myEncoder, myDecoder)

let extras : Encoder<Model> * (string -> obj -> Result<Model,DecoderError>) = 
    Extra.empty
    |> CustomEncoders.addDummyCoder<WsSender>
    |> CustomEncoders.buildExtras<Model>
