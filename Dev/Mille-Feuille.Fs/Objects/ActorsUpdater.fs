namespace wraikny.MilleFeuille.Fs.Objects

open System.Collections.Generic;
open System.Linq;

open wraikny.Tart.Helper
open wraikny.MilleFeuille.Core.Object


/// 追加削除の発生するasd.Object2Dの更新管理を行うクラス。
[<Class>]
type ActorsUpdater<'ViewModel, 'Actor, 'ActorViewModel
    when 'Actor :> asd.Object2D
    and  'Actor :> IUpdated<'ActorViewModel>
    >(name, init, selecter) =
    inherit Layer2DComponent<asd.Layer2D>(name)

    let selecter = selecter

    member private this.Updater =
        new ObjectsUpdater<'ViewModel, 'Actor, 'ActorViewModel>(
            init
            , (fun o -> this.Owner.AddObject(o))
            , (fun o -> o.Dispose())
        )

    
    interface IObserver<'ViewModel> with
        member this.UpdateFromNotify(input) =
            if this.IsUpdated then
                this.Updater.Update(selecter input)



/// ActorsUpdaterクラスを作成するビルダー。
type ActorsUpdaterBuilder<'ViewModel, 'Actor, 'ActorViewModel
    when 'Actor :> asd.Object2D
    and  'Actor :> IUpdated<'ActorViewModel>
    > =
    {
        init : unit -> 'Actor
        selecter : 'ViewModel -> UpdaterViewModel<'ActorViewModel>
    }


module ActorsUpdaterBuilder =
    /// ビルダーからActorsUpdaterクラスを作成する。
    let build name builder =
        let actorsUpdater =
            new ActorsUpdater<_, _, _>(
                name
                , builder.init
                , builder.selecter
            )

        actorsUpdater