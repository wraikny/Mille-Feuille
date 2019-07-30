namespace wraikny.MilleFeuille.Fs.Objects

open System
open wraikny.Tart.Helper.Utils
open wraikny.Tart.Core.View


type MaptipsUpdaterArg<'ViewModel, 'Chip, 'ChipViewModel
    when 'Chip :> asd.Chip2D
    > =
    {
        create : unit -> 'Chip
        onError : exn -> unit
        onCompleted : unit -> unit
    }


/// 追加削除の発生するマップチップの更新管理を行うクラス。
[<Class>]
type MaptipsUpdater<'Chip, 'ChipViewModel
    when 'Chip :> asd.Chip2D
    and  'Chip :> IUpdatee<'ChipViewModel>
    >(arg : MaptipsUpdaterArg<_, _, _>) as this =
    inherit asd.MapObject2D()

    let updater = new ObjectsUpdater<'Chip, 'ChipViewModel>({
        create = arg.create
        add = fun chip -> this.AddChip(chip) |> ignore
        remove = fun chip -> this.RemoveChip(chip) |> ignore
        dispose = fun chip -> chip.Dispose()
    })

    member __.UpdatingOption
        with get() = updater.UpdatingOption
        and set(x) = updater.UpdatingOption <- x

    interface IObserver<UpdaterViewModel<'ChipViewModel>> with
        member this.OnNext(input) =
            if this.IsUpdated then
                updater.Update(input)

        member __.OnError(e) = arg.onError(e)

        member __.OnCompleted() = arg.onCompleted()