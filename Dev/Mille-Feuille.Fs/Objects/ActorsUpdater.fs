namespace wraikny.MilleFeuille.Fs.Objects


open wraikny.Tart.Helper.Utils
open wraikny.Tart.Core.View

open wraikny.MilleFeuille.Core.Object


/// 追加削除の発生するasd.Object2Dの更新管理を行うクラス。
[<Class>]
type ActorsUpdater<'ViewModel, 'Actor, 'ActorViewModel
    when 'Actor :> asd.Object2D
    and 'Actor :> IUpdatee<'ActorViewModel>
    >(name, create, selecter) as this =
    inherit Layer2DComponent<asd.Layer2D>(name)

    let selecter = selecter

    let updater = new ObjectsUpdater<'ViewModel, 'Actor, 'ActorViewModel>({
        create = create
        add = fun actor -> this.Owner.AddObject(actor) |> ignore
        remove = fun actor -> this.Owner.RemoveObject(actor) |> ignore
        dispose = fun actor -> actor.Dispose()
    })

    let iUpdater = updater :> IObjectsUpdater

    interface IObjectsUpdater with
        member this.EnabledUpdating
            with get() = iUpdater.EnabledUpdating
            and  set(value) = iUpdater.EnabledUpdating <- value

        member this.EnabledPooling
            with get() = iUpdater.EnabledPooling
            and  set(value) = iUpdater.EnabledPooling <- value

    
    interface IObserver<'ViewModel> with
        member this.UpdateFromNotify(input) =
            if this.IsUpdated then
                updater.Update(selecter input)



/// ActorsUpdaterクラスを作成するビルダー。
[<Struct>]
type ActorsUpdaterBuilder<'ViewModel, 'Actor, 'ActorViewModel
    when 'Actor :> asd.Object2D
    > =
    {
        initActor : unit -> 'Actor
        selectActor : 'ViewModel -> UpdaterViewModel<'ActorViewModel> option
    }


module ActorsUpdaterBuilder =
    /// ビルダーからActorsUpdaterクラスを作成する。
    let build name builder =
        let actorsUpdater =
            new ActorsUpdater<_, _, _>(
                name
                , builder.initActor
                , builder.selectActor
            )

        actorsUpdater