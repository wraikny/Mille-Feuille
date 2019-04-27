namespace wraikny.MilleFeuille.Fs.Objects

open System.Collections.Generic;
open System.Linq;

open wraikny.Tart.Helper
open wraikny.MilleFeuille.Core.Object


/// 追加削除の発生するマップチップの更新管理を行うクラス。
[<Class>]
type MaptipsUpdater<'ViewModel, 'Chip, 'ChipViewModel
    when 'Chip :> asd.Chip2D
    and  'Chip :> IUpdated<'ChipViewModel>
    >(init, selecter) as this =
    inherit asd.MapObject2D()

    let init = init
    let selecter = selecter

    let updater =
        new ObjectsUpdater<'ViewModel, 'Chip, 'ChipViewModel>(
            init
            , (fun o -> this.AddChip(o) |> ignore)
            , (fun o -> this.RemoveChip(o) |> ignore)
        )

    interface IObserver<'ViewModel> with
        member this.UpdateFromNotify(input) =
            if this.IsUpdated then
                updater.Update(selecter input)



/// MaptipsUpdaterクラスを作成するビルダー。
type MaptipsUpdaterBuilder<'ViewModel, 'Chip, 'ChipViewModel
    when 'Chip :> asd.Chip2D
    and  'Chip :> IUpdated<'ChipViewModel>
    > =
    {
        init : unit -> 'Chip
        selecter : 'ViewModel -> UpdaterViewModel<'ChipViewModel>
    }


module MaptipsUpdaterBuilder =
    /// ビルダーからMaptipsUpdaterクラスを作成する。
    let build builder =
        let actorsUpdater =
            new MaptipsUpdater<_, _, _>(
                builder.init
                , builder.selecter
            )

        actorsUpdater