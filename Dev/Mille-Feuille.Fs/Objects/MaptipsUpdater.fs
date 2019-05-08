namespace wraikny.MilleFeuille.Fs.Objects


open wraikny.Tart.Helper.Utils
open wraikny.Tart.Core.View


/// 追加削除の発生するマップチップの更新管理を行うクラス。
[<Class>]
type MaptipsUpdater<'ViewModel, 'Chip, 'ChipViewModel
    when 'Chip :> asd.Chip2D
    and  'Chip :> IObjectUpdatee<'ChipViewModel>
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


    interface IObjectsUpdater with
        member this.UpdatingEnabled
            with get() = (updater :> IObjectsUpdater).UpdatingEnabled
            and  set(value) = (updater :> IObjectsUpdater).UpdatingEnabled <- value

    interface IObserver<'ViewModel> with
        member this.UpdateFromNotify(input) =
            if this.IsUpdated then
                updater.Update(selecter input)



/// MaptipsUpdaterクラスを作成するビルダー。
[<Struct>]
type MaptipsUpdaterBuilder<'ViewModel, 'Chip, 'ChipViewModel
    when 'Chip :> asd.Chip2D
    and  'Chip :> IObjectUpdatee<'ChipViewModel>
    > =
    {
        initChip : unit -> 'Chip
        selectChip : 'ViewModel -> UpdaterViewModel<'ChipViewModel> option
    }


module MaptipsUpdaterBuilder =
    /// ビルダーからMaptipsUpdaterクラスを作成する。
    let build builder =
        let actorsUpdater =
            new MaptipsUpdater<_, _, _>(
                builder.initChip
                , builder.selectChip
            )

        actorsUpdater