[<AutoOpen>]
module wraikny.MilleFeuille.Component.Coroutine

open wraikny.MilleFeuille


type asd.Object2D with
    member this.StartCoroutine(name, sequence : seq<unit>) =
        let c = new Object2DComponent<asd.Object2D>(name)
        let coroutine = sequence.GetEnumerator()
        c.add_OnUpdateEvent(fun _ ->
            if not <| coroutine.MoveNext() then
                c.Dispose()
        )
        c.Attach(this)

type asd.Layer2D with
    member this.StartCoroutine(name, sequence : seq<unit>) =
        let c = new Layer2DComponent<asd.Layer2D>(name)
        let coroutine = sequence.GetEnumerator()
        c.add_OnUpdatedEvent(fun _ ->
            if not <| coroutine.MoveNext() then
                c.Dispose()
        )
        c.Attach(this)

type asd.Scene with
    member this.StartCoroutine(name, sequence : seq<unit>) =
        let c = new SceneComponent<asd.Scene>(name)
        let coroutine = sequence.GetEnumerator()
        c.add_OnUpdatedEvent(fun _ ->
            if not <| coroutine.MoveNext() then
                c.Dispose()
        )
        c.Attach(this) 