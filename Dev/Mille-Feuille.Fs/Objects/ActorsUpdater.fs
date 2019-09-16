namespace wraikny.MilleFeuille.Objects

open System
open wraikny.Tart.Helper.Utils
open wraikny.Tart.Core.View

open wraikny.MilleFeuille

type ActorsUpdaterArg<'ViewModel, 'Actor, 'ActorViewModel
    when 'Actor :> asd.Object2D
    > =
    {
        create : unit -> 'Actor
        onError : exn -> unit
        onCompleted : unit -> unit
    }


/// 追加削除の発生するasd.Object2Dの更新管理を行うクラス。
[<Class; Sealed>]
type ActorsUpdater<'Actor, 'ActorViewModel
    when 'Actor :> asd.Object2D
    and 'Actor :> IUpdatee<'ActorViewModel>
    >(layer : asd.Layer2D, enebleObjectsRemoving, arg : ActorsUpdaterArg<_, _, _>) =

    let updater = new ObjectsUpdater<'Actor, 'ActorViewModel>({
        create = arg.create
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

    new(layer, arg) = new ActorsUpdater<_, _>(layer, false, arg)

    member __.UpdatingOption
        with get() = updater.UpdatingOption
        and set(x) = updater.UpdatingOption <- x

    member __.Remove(id) = updater.Remove(id)
    
    interface IObserver<UpdaterViewModel<'ActorViewModel>> with
        member this.OnNext(input) =
            if layer.IsUpdated then
                updater.Update(input)

        member this.OnError(e) = arg.onError(e)

        member this.OnCompleted() = arg.onCompleted()