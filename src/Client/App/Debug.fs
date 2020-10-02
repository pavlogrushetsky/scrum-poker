module App.Debug

open Thoth.Json

open App.Model

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