namespace wraikny.MilleFeuille.Updater

open System


/// 追加削除の発生するマップチップの更新管理を行うクラス。
[<Class; Sealed>]
type MaptipsUpdater<'Key, 'Chip, 'ChipViewModel
    when 'Chip :> asd.Chip2D
    and  'Chip :> IUpdatee<'ChipViewModel>
    and  'Key : equality
    >(create: unit -> 'Chip) as this =
    inherit asd.MapObject2D()

    let updater = new ObjectsUpdater<'Key, 'Chip, 'ChipViewModel>({
        create = create
        add = this.AddChip >> ignore
        remove =
            this.RemoveChip >> ignore
        dispose = fun chip ->
            this.RemoveChip(chip)|> ignore
            chip.Dispose()
            ()
    })

    member __.UpdatingOption
        with get() = updater.UpdatingOption
        and set(x) =
            if x = UpdatingOption.UpdatingWithPooling then
                raise <| System.ArgumentException "UpdatingWithPooling is not supported in this class"

            updater.UpdatingOption <- x

    interface IObserver<UpdaterViewModel<'Key, 'ChipViewModel>> with
        member this.OnNext(input) =
            if this.IsUpdated then
                updater.Update(input)

        member __.OnError(e) = raise e

        member __.OnCompleted() = printfn "Completed"