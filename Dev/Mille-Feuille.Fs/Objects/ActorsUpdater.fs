namespace wraikny.MilleFeuille.Fs.Objects

open System
open wraikny.Tart.Helper.Utils
open wraikny.Tart.Core.View

open wraikny.MilleFeuille.Core.Object

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
        add = fun actor -> this.Owner.AddObject(actor) |> ignore
        remove = fun actor -> this.Owner.RemoveObject(actor) |> ignore
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