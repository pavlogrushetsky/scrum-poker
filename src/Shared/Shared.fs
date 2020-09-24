namespace Shared

open System

[<CLIMutable>]
type Message =
    { Id   : Guid
      Time : DateTime
      Text : string }

type WebSocketMessage<'a> =
    { Topic   : string
      Ref     : string
      Payload : 'a }

type WebSocketServerMessage =
    | BroadcastMessage of Message
    | BroadcastMessages of Message list

type WebSocketClientMessage =
    | TextMessage of string
    | GetMessages of int     
