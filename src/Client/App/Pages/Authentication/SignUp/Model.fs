module Pages.SignUp

type Model = 
    { Title : string }

type Msg = 
    | DummyMsg

type ViewProps =
    { Model : Model
      Dispatch : Msg -> unit }