namespace wraikny.MilleFeuille.Fs.Objects


open wraikny.Tart.Helper.Utils
open wraikny.Tart.Core.View


/// 追加削除の発生するマップチップの更新管理を行うクラス。
[<Class>]
type MaptipsUpdater<'ViewModel, 'Chip, 'ChipViewModel
    when 'Chip :> asd.Chip2D
    and  'Chip :> IUpdatee<'ChipViewModel>
    >(create, selecter) as this =
    inherit asd.MapObject2D()

    let selecter = selecter

    let updater = new ObjectsUpdater<'ViewModel, 'Chip, 'ChipViewModel>({
        create = create
        add = fun chip -> this.AddChip(chip) |> ignore
        remove = fun chip -> this.RemoveChip(chip) |> ignore
        dispose = fun chip -> chip.Dispose()
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
        member this.Update(input) =
            if this.IsUpdated then
                updater.Update(selecter input)



/// MaptipsUpdaterクラスを作成するビルダー。
[<Struct>]
type MaptipsUpdaterBuilder<'ViewModel, 'Chip, 'ChipViewModel
    when 'Chip :> asd.Chip2D
    > =
    {
        initChip : unit -> 'Chip
        selectChip : 'ViewModel -> UpdaterViewModel<'ChipViewModel>
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