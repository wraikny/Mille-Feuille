namespace wraikny.MilleFeuille.Fs.Input.Controller

open System.Collections.Generic
open System.Linq

open wraikny.MilleFeuille.Core.Input.Controller

/// キーボードコントローラクラスを作成するビルダー。
type KeyboardBuilder<'T when 'T : comparison> =
    {
        binding : Map<'T, asd.Keys>
    }


module KeyboardBuilder =
    /// キーボードコントローラクラスを作成するビルダーを作る。
    let init() =
        {
            binding = Map.empty
        }

    let internal binding builder = builder.binding

    /// キー入力に操作を対応付ける。
    let bindKey control key builder =
        { builder with
            binding = builder.binding |> Map.add control key
        }

    /// リストを元にキー入力に操作を対応付ける。
    let rec bindKeys bindings builder =
        bindings |> function
        | [] -> builder
        | (c, k)::xs ->
            builder
            |> bindKey c k
            |> bindKeys xs

    /// ビルダーからキーボードコントローラクラスを作成する。
    let build (builder : KeyboardBuilder<'T>) =
        let keyboard = new KeyboardController<'T>()

        for bind in builder.binding |> Map.toSeq do
            keyboard.BindKey(bind) |> ignore

        keyboard