namespace wraikny.MilleFeuille.Fs.Objects


open wraikny.Tart.Helper.Utils
open wraikny.Tart.Core.View

open wraikny.MilleFeuille.Core.Object


/// 追加削除の発生するasd.Object2Dの更新管理を行うクラス。
[<AbstractClass>]
type ActorsUpdater<'ViewModel, 'Actor, 'ActorViewModel
    when 'Actor :> asd.Object2D
    >(name, selecter) as this =
    inherit Layer2DComponent<asd.Layer2D>(name)

    let selecter = selecter

    let updater =
        new ObjectsUpdater<'ViewModel, 'Actor, 'ActorViewModel>(this)

    abstract Create : unit -> 'Actor
    abstract Update : 'Actor * 'ActorViewModel -> unit

    interface IObjectsUpdaterParent<'Actor, 'ActorViewModel> with
        member this.Create() = this.Create()
        member this.Add(actor) = this.Owner.AddObject(actor) |> ignore
        member this.Remove(actor) = this.Owner.RemoveObject(actor) |> ignore
        member this.Dispose(actor) = actor.Dispose()
        member this.Update(chip, viewModel) = this.Update(chip, viewModel)

    interface IObjectsUpdater with
        member this.EnabledUpdating
            with get() = (updater :> IObjectsUpdater).EnabledUpdating
            and  set(value) = (updater :> IObjectsUpdater).EnabledUpdating <- value

        member this.EnabledPooling
            with get() = (updater :> IObjectsUpdater).EnabledPooling
            and  set(value) = (updater :> IObjectsUpdater).EnabledPooling <- value

    
    interface IObserver<'ViewModel> with
        member this.UpdateFromNotify(input) =
            if this.IsUpdated then
                updater.Update(selecter input)



/// ActorsUpdaterクラスを作成するビルダー。
//[<Struct>]
//type ActorsUpdaterBuilder<'ViewModel, 'Actor, 'ActorViewModel
//    when 'Actor :> asd.Object2D
//    > =
//    {
//        initActor : unit -> 'Actor
//        selectActor : 'ViewModel -> UpdaterViewModel<'ActorViewModel> option
//    }


//module ActorsUpdaterBuilder =
//    /// ビルダーからActorsUpdaterクラスを作成する。
//    let build name builder =
//        let actorsUpdater =
//            new ActorsUpdater<_, _, _>(
//                name
//                , builder.initActor
//                , builder.selectActor
//            )

//        actorsUpdater