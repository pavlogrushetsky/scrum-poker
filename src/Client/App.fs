module App

open Elmish
open Elmish.React

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram Index.init Index.update Index.view
|> Program.withSubscription Index.Channel.subscription
|> Program.toNavigable App.Routing.parseRoute Index.updateRoute
|> Program.withReactBatched "elmish-app"
#if DEBUG
|> Program.withConsoleTrace
|> Program.withDebugger
|> Program.withDebuggerCoders (fst Index.extras) (snd Index.extras)
#endif
|> Program.run
