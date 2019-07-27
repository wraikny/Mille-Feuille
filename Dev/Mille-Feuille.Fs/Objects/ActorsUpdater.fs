namespace wraikny.MilleFeuille.Fs.Objects

open System
open wraikny.Tart.Helper.Utils
open wraikny.Tart.Core.View

open wraikny.MilleFeuille.Core

type ActorsUpdaterArg<'ViewModel, 'Actor, 'ActorViewModel
    when 'Actor :> asd.Object2D
    > =
    {
        create : unit -> 'Actor
        onError : exn -> unit
        onCompleted : unit -> unit
    }


/// 追加削除の発生するasd.Object2Dの更新管理を行うクラス。
[<Class>]
type ActorsUpdater<'Actor, 'ActorViewModel
    when 'Actor :> asd.Object2D
    and 'Actor :> IUpdatee<'ActorViewModel>
    >(name, arg : ActorsUpdaterArg<_, _, _>) as this =
    inherit Layer2DComponent<asd.Layer2D>(name)

    let updater = new ObjectsUpdater<'Actor, 'ActorViewModel>({
        create = arg.create
        add = fun actor ->
            if actor.Layer = null then
                this.Owner.AddObject(actor)

            actor.IsUpdated <- true
            actor.IsDrawn <- true
        remove = fun actor ->
            actor.IsUpdated <- false
            actor.IsDrawn <- false

        // add = this.Owner.AddObject
        // bellow code raises NullReferenceException in asd.ENgine.Update
        // remove = this.Owner.RemoveObject

        dispose = fun actor -> actor.Dispose()
    })

    let iUpdater = updater :> IUpdater<_>

    interface IUpdater<'ActorViewModel> with
        member this.EnabledUpdating
            with get() = iUpdater.EnabledUpdating
            and  set(value) = iUpdater.EnabledUpdating <- value

        member this.EnabledPooling
            with get() = iUpdater.EnabledPooling
            and  set(value) = iUpdater.EnabledPooling <- value

    
    interface IObserver<UpdaterViewModel<'ActorViewModel>> with
        member this.OnNext(input) =
            if this.IsUpdated then
                updater.Update(input)

        member this.OnError(e) = arg.onError(e)

        member this.OnCompleted() = arg.onCompleted()