namespace wraikny.MilleFeuille.Fs.Animation

open System.Collections

open wraikny.MilleFeuille.Core.Animation

type AnimationBuilder<'Obj> =
    {
        name : string
        coroutines : ('Obj -> seq<unit>) list
    }


module AnimationBuilder =
    let init name =
        {
            name = name
            coroutines = []
        }


    let addCoroutine coroutines builder =
        { builder with
            coroutines = builder.coroutines @ coroutines
        }


    let build builder =
        new Animation<_>(
            builder.name
            , fun owner ->
                let coroutine = seq {
                    for c in builder.coroutines do yield! (c owner)
                }
                coroutine.GetEnumerator() :> IEnumerator
        )