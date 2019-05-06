namespace wraikny.MilleFeuille.Fs.Objects


open wraikny.Tart.Helper
open wraikny.Tart.Core.View

open wraikny.MilleFeuille.Core.Object


/// 追加削除の発生するasd.Object2Dの更新管理を行うクラス。
[<Class>]
type ActorsUpdater<'ViewModel, 'Actor, 'ActorViewModel
    when 'Actor :> asd.Object2D
    and  'Actor :> IObjectUpdatee<'ActorViewModel>
    >(name, init, selecter) as this =
    inherit Layer2DComponent<asd.Layer2D>(name)

    let selecter = selecter

    let updater =
        new ObjectsUpdater<'ViewModel, 'Actor, 'ActorViewModel>(
            init
            , (fun o -> this.Owner.AddObject(o))
            , (fun o -> o.Dispose())
        )


    interface IObjectsUpdater with
        member this.UpdatingEnabled
            with get() = (updater :> IObjectsUpdater).UpdatingEnabled
            and  set(value) = (updater :> IObjectsUpdater).UpdatingEnabled <- value

    
    interface IObserver<'ViewModel> with
        member this.UpdateFromNotify(input) =
            if this.IsUpdated then
                updater.Update(selecter input)



/// ActorsUpdaterクラスを作成するビルダー。
type ActorsUpdaterBuilder<'ViewModel, 'Actor, 'ActorViewModel
    when 'Actor :> asd.Object2D
    and  'Actor :> IObjectUpdatee<'ActorViewModel>
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