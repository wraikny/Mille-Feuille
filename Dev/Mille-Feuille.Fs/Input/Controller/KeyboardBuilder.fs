namespace wraikny.MilleFeuille.Fs.Input.Controller

open System.Collections.Generic
open System.Linq

open wraikny.MilleFeuille.Core.Input.Controller


type KeyboardBuilder<'T when 'T : comparison> =
    {
        binding : Map<'T, asd.Keys>
    }


module KeyboardBuilder =
    let init() =
        {
            binding = Map.empty
        }

    let internal binding builder = builder.binding

    let bindKey control key builder =
        { builder with
            binding = builder.binding |> Map.add control key
        }

    let rec bindKeys bindings builder =
        bindings |> function
        | [] -> builder
        | (c, k)::xs ->
            builder
            |> bindKey c k
            |> bindKeys xs

    let build (builder : KeyboardBuilder<'T>) =
        let keyboard = new KeyboardController<'T>()

        for bind in builder.binding |> Map.toSeq do
            keyboard.BindKey(bind) |> ignore

        keyboard