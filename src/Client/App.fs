module App

open Elmish
open Elmish.React

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram Index.init Index.update Index.view
#if DEBUG
|> Program.withConsoleTrace
#endif
|> Program.withSubscription Index.Channel.subscription
|> Program.withReactSynchronous "elmish-app"
#if DEBUG
|> Program.withDebugger
|> Program.withDebuggerCoders (fst Index.extras) (snd Index.extras)
#endif
|> Program.run
