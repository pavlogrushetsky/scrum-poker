module App

open Elmish
open Elmish.React

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram App.Update.init App.Update.update App.View.view
|> Program.withSubscription App.Update.subscription
|> Program.toNavigable App.Routing.parseRoute App.Update.updateRoute
|> Program.withReactSynchronous "elmish-app"
#if DEBUG
|> Program.withConsoleTrace
|> Program.withDebugger
#endif
|> Program.run
