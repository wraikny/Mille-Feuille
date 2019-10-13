namespace wraikny.MilleFeuille.Updater

open System


/// 追加削除の発生するasd.Object2Dの更新管理を行うクラス。
[<Class; Sealed>]
type ActorsUpdater<'Key, 'Actor, 'ActorViewModel
    when 'Actor :> asd.Object2D
    and 'Actor :> IUpdatee<'ActorViewModel>
    and  'Key : equality
    >(layer : asd.Layer2D, enebleObjectsRemoving, create : unit -> 'Actor) =

    let updater = new ObjectsUpdater<'Key, 'Actor, 'ActorViewModel>({
        create = create
        add =
            if enebleObjectsRemoving then
                layer.AddObject
            else
                fun actor ->
                if actor.Layer = null then
                    layer.AddObject(actor)

                actor.IsUpdated <- true
                actor.IsDrawn <- true
        remove =
            if enebleObjectsRemoving then
                layer.RemoveObject
            else
                fun actor ->
                    actor.IsUpdated <- false
                    actor.IsDrawn <- false

        // add = this.Owner.AddObject
        // bellow code raises NullReferenceException in asd.Engine.Update
        // remove = this.Owner.RemoveObject

        dispose = fun actor -> actor.Dispose()
    })

    new(layer, create) = new ActorsUpdater<_, _, _>(layer, false, create)

    member __.UpdatingOption
        with get() = updater.UpdatingOption
        and set(x) = updater.UpdatingOption <- x

    member __.Remove(id) = updater.Remove(id)
    
    interface IObserver<UpdaterViewModel<'Key, 'ActorViewModel>> with
        member this.OnNext(input) =
            if layer.IsUpdated then
                updater.Update(input)

        member this.OnError(e) = raise e

        member this.OnCompleted() = printfn "Completed"
