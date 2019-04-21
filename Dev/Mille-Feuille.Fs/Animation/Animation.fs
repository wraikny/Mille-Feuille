namespace wraikny.MilleFeuille.Fs.Animation

open System.Collections

open wraikny.MilleFeuille.Core.Animation

/// アニメーションクラスを作成するビルダー。
type AnimationBuilder<'Obj> =
    {
        name : string
        coroutines : ('Obj -> seq<unit>) list
    }


module AnimationBuilder =
    /// アニメーションクラスを作成するビルダーを作る。
    let init name =
        {
            name = name
            coroutines = []
        }

    /// アニメーションにコルーチンを追加する。
    let addCoroutine coroutine builder =
        { builder with
            coroutines = builder.coroutines @ [coroutine]
        }

    /// リストを元にアニメーションにコルーチンを追加する。
    let addCoroutines coroutines builder =
        { builder with
            coroutines = builder.coroutines @ coroutines
        }

    /// ビルダーからアニメーションクラスを作成する。
    let build builder =
        new Animation<_>(
            builder.name
            , fun owner ->
                let coroutine = seq {
                    for c in builder.coroutines do yield! (c owner)
                }
                coroutine.GetEnumerator() :> IEnumerator
        )