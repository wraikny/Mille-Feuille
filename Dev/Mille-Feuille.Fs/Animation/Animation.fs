namespace wraikny.MilleFeuille.Fs.Animation

open System.Collections

open wraikny.MilleFeuille.Core.Animation

open FSharpPlus

/// アニメーションクラスを作成するビルダー。
[<Struct>]
type AnimationBuilder<'Obj> =
    {
        name : string
        coroutines : ('Obj -> seq<unit>) list
    }

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module AnimationBuilder =
    /// アニメーションクラスを作成するビルダーを作る。
    let inline init name =
        {
            name = name
            coroutines = []
        }

    /// アニメーションにコルーチンを追加する。
    let inline addCoroutine coroutine builder =
        { builder with
            coroutines = coroutine::builder.coroutines
        }

    /// リストを元にアニメーションにコルーチンを追加する。
    let inline addCoroutines coroutines builder =
        { builder with
            coroutines = (rev coroutines) @ builder.coroutines
        }

    /// ビルダーからアニメーションクラスを作成する。
    let build builder =
        let generator =
            builder.coroutines |> function
            | [] ->
                fun _ -> (Seq.empty).GetEnumerator() :> IEnumerator
            | coroutine::[] ->
                fun owner -> (coroutine owner).GetEnumerator() :> IEnumerator
            | coroutines ->
                fun owner ->
                    let coroutine = seq {
                        for c in (List.rev coroutines) do
                            yield! (c owner)
                    }
                    coroutine.GetEnumerator() :> IEnumerator

        new Animation<_>(builder.name, System.Func<_, _> generator)