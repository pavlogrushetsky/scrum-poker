module Index

open Elmish
open Fable.React
open Fable.React.Props
open Thoth.Json
open Thoth.Fetch
open Browser.Types
open Shared
open System

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

type HTMLAttr =
    | [<CompiledName("data-target")>] DataTarget of string
    | [<CompiledName("data-placement")>] DataPlacement of string
    | [<CompiledName("data-dismiss")>] DataDismiss of string
    | [<CompiledName("aria-hidden")>] AriaHidden of bool
    | [<CompiledName("aria-label")>] AriaLabel of string
    | [<CompiledName("aria-labelledby")>] AriaLabelledBy of string
    | [<CompiledName("data-live-search")>] DataLiveSearch of bool
    | [<CompiledName("data-content")>] DataContent of string
    interface IHTMLProp

let private routeItem name iconName route current =
    li [ classList [ "nav-item", true; "active", route = current ] ] [
        a [ ClassName "nav-link"; Href "" ] [
            //yield icon iconName
            yield str name
            if route = current then
                yield span [ ClassName "sr-only" ] [ str "(current)" ]
            else
                ignore()
        ]
    ]

let icon name =
    i [ ClassName (sprintf "fab fa-lg fa-%s" name); Style [ MarginRight "3px" ] ] []

let MenuComponent = ReactComponent "Menu" (fun (props : Menu) ->
    nav [ ClassName "navbar navbar-expand-lg navbar-dark bg-primary fixed-top" ] [
        div [ ClassName "container" ] [
            a [ ClassName "navbar-brand"; Href "#" ] [ str "Scrum Poker" ]
            button [ ClassName "navbar-toggler";
                     Typeof "button";
                     DataToggle "collapse";
                     DataTarget "#navbarNavDropdown";
                     AriaControls "navbarNavDropdown";
                     AriaExpanded false;
                     AriaLabel "Toggle navigation" ] [
                span [ ClassName "navbar-toggler-icon" ] []
            ]
            div [ ClassName "collapse navbar-collapse"; Id "navbarNavDropdown" ] [
                ul [ ClassName "navbar-nav mr-auto" ] [
                    routeItem "Home" "" "" ""
                    routeItem "About" "compose" "" ""
                ]
                ul [ ClassName "navbar-nav mr-md-2" ] [
                    li [ ClassName "nav-item" ] [
                        a [ ClassName "nav-link"; Href "https://github.com/pavlogrushetsky/scrum-poker"; Target "_blank" ] [
                            icon "github"
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
                        table [ ClassName "table table-dorderless table-hover" ] [
                            thead [ ClassName "thead" ] [
                                tr [] [
                                    th [ Scope "col" ] [ str "Time" ]
                                    th [ Scope "col" ] [ str "Message" ]
                                ]
                            ]
                            tbody [][
                                for message in messages do
                                    tr [] [
                                        td [] [ str (sprintf "%O" message.Time) ]
                                        td [] [ str message.Text ]
                                    ]
                            ]
                            tfoot [] []
                        ]
                ]               
            ]                   
        ]
        footer [ ClassName "footer" ] [
            div [ ClassName "container" ] [
                p [ ClassName "float-right" ] [
                    a [ Href "#" ] [ str "Back to top" ]
                ]
                p [] [ str "© 2020" ]
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
