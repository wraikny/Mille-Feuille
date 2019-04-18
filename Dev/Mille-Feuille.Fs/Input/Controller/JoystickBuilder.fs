namespace wraikny.MilleFeuille.Fs.Input.Controller

open System.Collections.Generic
open System.Linq

open wraikny.MilleFeuille.Core.Input.Controller


type JoystickInput<'T> =
    | Button of index : int
    | Axis of index : int * dir : AxisDirection


type JoystickBuilder<'T when 'T : comparison> =
    {
        index : int
        binding : Map<'T, JoystickInput<'T>>
    }

module JoystickBuilder =
    let init(index) =
        {
            index = index
            binding = Map.empty
        }

    let bindInput control input (builder : JoystickBuilder<'T>) =
        { builder with
            binding = builder.binding |> Map.add control input
        }


    let bindButton control index (builder) =
        builder
        |> bindInput control (Button index)


    let bindAxis control input builder =
        builder
        |> bindInput control (Axis input)


    let rec bindInputs bindings builder =
        bindings |> function
        | [] -> builder
        | (c, i)::xs ->
            builder
            |> bindInput c i
            |> bindInputs xs


    let bindButtons bindings builder =
        let bindings =
            bindings
            |> List.map(fun (c, i) -> (c, Button i))

        builder
        |> bindInputs bindings


    let bindAxes bindings builder =
        let bindings =
            bindings
            |> List.map(fun (c, i) -> (c, Axis i))

        builder
        |> bindInputs bindings


    let build (builder : JoystickBuilder<'T>) =
        let joystick = new JoystickController<'T>(builder.index)

        for (control, input) in builder.binding |> Map.toSeq do
            input |> function
            | Button(index) ->
                joystick.BindButton(control, index)
            | Axis(index, dir) ->
                joystick.BindAxis(control, index, dir)

        joystick