﻿namespace wraikny.MilleFeuille.Input

open System.Collections.Generic
open System.Linq

open wraikny.MilleFeuille.Input

[<Struct>]
type JoystickInput<'T> =
    | Button of buttonIndex : int
    | Axis of axisIndex : int * dir : AxisDirection


/// ジョイスティックコントローラクラスを作成するビルダー。
[<Struct>]
type JoystickBuilder<'T, 'U
    when 'T : comparison
    and  'U : comparison
    > =
    {
        index : int
        binding : Map<'T, JoystickInput<'T>>
        axisTiltBinding : Map<'U, int>
    }

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module JoystickBuilder =
    /// ジョイスティックコントローラクラスを作成するビルダーを作る。
    let inline init(index) =
        {
            index = index
            binding = Map.empty
            axisTiltBinding = Map.empty
        }

    /// ジョイスティック入力に操作を対応付ける。
    let inline bindInput control input (builder : JoystickBuilder<_, _>) =
        { builder with
            binding =
                builder.binding
                |> Map.add control input
        }

    /// ボタン入力に操作を対応付ける。
    let inline bindButton control index (builder) =
        builder
        |> bindInput control (Button index)


    /// スティック入力に操作を対応付ける。
    let inline bindAxis control input builder =
        builder
        |> bindInput control (Axis input)


    /// スティックのインデックスに操作を対応付ける。
    let inline bindAxisTilt control index builder =
        { builder with
            axisTiltBinding =
                builder.axisTiltBinding
                |> Map.add control index
        }


    /// リストをもとにジョイスティック入力に操作を対応付ける。
    let bindInputs (bindings : #seq<_>) builder =
        let mutable m = builder.binding
        for (k, v) in bindings do
            m <- m |> Map.add k v

        { builder with binding = m }


    /// リストをもとにジョイスティックのボタン入力に操作を対応付ける。
    let bindButtonsList bindings builder =
        let bindings =
            bindings
            |> List.map(fun (c, i) -> c, Button i)

        builder
        |> bindInputs bindings


    /// リストをもとにジョイスティックのスティック入力に操作を対応付ける。
    let bindAxesList bindings builder =
        let bindings =
            bindings
            |> List.map(fun (c, i) -> c, Axis i)

        builder
        |> bindInputs bindings


    /// リストをもとにスティックのインデックスに操作を対応付ける。
    let bindAxesTiltList bindings builder =
        let mutable m = builder.axisTiltBinding
        for (k, v) in bindings do
            m <- m |> Map.add k v

        { builder with axisTiltBinding = m }


    /// ビルダーからジョイスティックコントローラクラスを作成する。
    let build (builder : JoystickBuilder<'T, 'U>) =
        let joystick = new JoystickController<'T, 'U>(builder.index)

        for (control, input) in builder.binding |> Map.toSeq do
            input |> function
            | Button(index) ->
                joystick.BindButton(control, index)
            | Axis(index, dir) ->
                joystick.BindAxis(control, index, dir)

        joystick