namespace wraikny.MilleFeuille.Input

open System.Collections.Generic
open System.Linq

open wraikny.MilleFeuille.Input

/// キーボードコントローラクラスを作成するビルダー。
[<Struct>]
type KeyboardBuilder<'T when 'T : comparison> =
    {
        binding : Map<'T, asd.Keys>
    }

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module KeyboardBuilder =
    /// キーボードコントローラクラスを作成するビルダーを作る。
    let inline init() =
        {
            binding = Map.empty
        }

    let inline internal binding builder = builder.binding

    /// キー入力に操作を対応付ける。
    let inline bindKey control key builder =
        { builder with
            binding = builder.binding |> Map.add control key
        }

    /// リストを元にキー入力に操作を対応付ける。
    let bindKeysList (bindings : #seq<_>) builder =
        let mutable m = builder.binding
        for (k, v) in bindings do
            m <- m |> Map.add k v

        { builder with binding = m }

    /// ビルダーからキーボードコントローラクラスを作成する。
    let build (builder : KeyboardBuilder<'T>) =
        let keyboard = new KeyboardController<'T>()

        for bind in builder.binding |> Map.toSeq do
            keyboard.BindKey(bind) |> ignore

        keyboard