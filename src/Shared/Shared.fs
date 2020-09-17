namespace Shared

open System

[<CLIMutable>]
type Message =
    { Id : Guid
      Time : DateTime
      Text : string }

type WebSocketServerMessage =
    | BroadcastMessage of Message

type WebSocketClientMessage =
    | TextMessage of string
