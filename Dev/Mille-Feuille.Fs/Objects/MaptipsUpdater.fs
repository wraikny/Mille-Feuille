namespace wraikny.MilleFeuille.Fs.Objects


open wraikny.Tart.Helper.Utils
open wraikny.Tart.Core.View


/// 追加削除の発生するマップチップの更新管理を行うクラス。
[<AbstractClass>]
type MaptipsUpdater<'ViewModel, 'Chip, 'ChipViewModel
    when 'Chip :> asd.Chip2D
    >(selecter) as this =
    inherit asd.MapObject2D()

    let selecter = selecter

    let updater = new ObjectsUpdater<'ViewModel, 'Chip, 'ChipViewModel>(this)

    abstract Create : unit -> 'Chip
    abstract Update : 'Chip * 'ChipViewModel -> unit

    interface IObjectsUpdaterParent<'Chip, 'ChipViewModel> with
        member this.Create() = this.Create()
        member this.Add(chip) = this.AddChip(chip) |> ignore
        member this.Remove(chip) = this.RemoveChip(chip) |> ignore
        member this.Dispose(chip) = chip.Dispose()
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



/// MaptipsUpdaterクラスを作成するビルダー。
//[<Struct>]
//type MaptipsUpdaterBuilder<'ViewModel, 'Chip, 'ChipViewModel
//    when 'Chip :> asd.Chip2D
//    > =
//    {
//        initChip : unit -> 'Chip
//        selectChip : 'ViewModel -> UpdaterViewModel<'ChipViewModel> option
//    }


//module MaptipsUpdaterBuilder =
//    /// ビルダーからMaptipsUpdaterクラスを作成する。
//    let build builder =
//        let actorsUpdater =
//            new MaptipsUpdater<_, _, _>(
//                builder.initChip
//                , builder.selectChip
//            )

//        actorsUpdater