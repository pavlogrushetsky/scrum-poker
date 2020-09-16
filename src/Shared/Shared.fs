namespace Shared

open System

type Message =
    { Time : DateTime
      Message : string }

type WebSocketServerMessage =
    | BroadcastMessage of Message

type WebSocketClientMessage =
    | TextMessage of string
