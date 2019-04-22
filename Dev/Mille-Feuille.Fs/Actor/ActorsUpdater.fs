namespace wraikny.MilleFeuille.Fs.Actor

open System.Collections.Generic;
open System.Linq;

open wraikny.Tart.Helper
open wraikny.MilleFeuille.Core.Object


/// 追加削除の発生するオブジェクトのクラスが実装するインターフェース。
[<Interface>]
type IActor<'ActorViewModel> =
    abstract Update : 'ActorViewModel -> unit


/// 追加削除の発生するオブジェクトの更新を行うためのビューモデル。
type UpdaterViewModel<'ActorViewModel> =
    {
        nextID : uint32
        actors : Map<uint32, 'ActorViewModel>
    }


/// 追加削除の発生するオブジェクトの更新管理を行うクラス。
[<Class>]
type ActorsUpdater<'Actor, 'ActorViewModel, 'ViewModel
    when 'Actor :> asd.Object2D
    and  'Actor :> IActor<'ActorViewModel>
    >(name, initializeActor, viewModelSelecter) =
    inherit Layer2DComponent<asd.Layer2D>(name)

    let mutable nextID = 0u
    let actors = new Dictionary<uint32, 'Actor>()

    let initializeActor = initializeActor
    let viewModelSelecter = viewModelSelecter

    
    interface IObserver<'ViewModel> with
        member this.UpdateFromNotify(input) =
            this.Update(viewModelSelecter input)
    

    /// ビューモデルを元にオブジェクトの更新を行う。
    member this.Update(viewModel : UpdaterViewModel<_>) =
        if this.IsUpdated then
            this.AddActors(&viewModel)
            this.UpdateActors(&viewModel)


    /// ビューモデルを元にidを照合してオブジェクトの追加を行う。
    member this.AddActors (viewModel : _ inref) =
        let newNextID = viewModel.nextID
        if nextID <> newNextID then
            
            for id in nextID..(newNextID - 1u) do
                viewModel.actors
                |> Map.tryFind id
                |> function
                | None -> ()
                | Some actorViewModel ->
                    let actor : 'Actor = initializeActor()
                    actor.Update(actorViewModel)

                    actors.Add(id, actor)
                    this.Owner.AddObject(actor)

            nextID <- newNextID


    /// ビューモデルを元にオブジェクトの更新と破棄を行う。
    member this.UpdateActors (viewModel : _ inref) =
        let actors' =
            actors
            |> Seq.map(fun x -> (x.Key, x.Value))

        for (id, actor) in actors' do
            viewModel.actors
            |> Map.tryFind id
            |> function
            | Some actorViewModel ->
                actor.Update(actorViewModel)

            | None ->
                actors.Remove(id) |> ignore


/// ActorsUpdaterクラスを作成するビルダー。
type ActorsUpdaterBuilder<'Actor, 'ActorViewModel, 'ViewModel
    when 'Actor :> asd.Object2D
    and  'Actor :> IActor<'ActorViewModel>
    > =
    {
        initializeActor : unit -> 'Actor
        viewModelSelecter : 'ViewModel -> UpdaterViewModel<'ActorViewModel>
    }


module ActorsUpdaterBuilder =
    /// ビルダーからActorsUpdaterクラスを作成する。
    let build name builder =
        let actorsUpdater =
            new ActorsUpdater<_, _, _>(
                name
                , builder.initializeActor
                , builder.viewModelSelecter
            )

        actorsUpdater