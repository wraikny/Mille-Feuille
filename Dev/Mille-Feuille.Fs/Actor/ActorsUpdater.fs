namespace wraikny.MilleFeuille.Fs.Actor

open System.Collections.Generic;
open System.Linq;

open wraikny.Tart.Helper
open wraikny.MilleFeuille.Core.Object

[<Interface>]
type IActor<'ActorViewModel> =
    abstract Update : 'ActorViewModel -> unit


type UpdaterViewModel<'ActorViewModel> =
    {
        nextID : uint32
        actors : Map<uint32, 'ActorViewModel>
    }


[<Class>]
type ActorsUpdater<'Actor, 'ActorViewModel, 'ViewModel
    when 'Actor :> asd.Object2D
    and  'Actor :> IActor<'ActorViewModel>
    and  'Actor : (new : unit -> 'Actor )
    >(name, viewModelSelecter) =
    inherit Layer2DComponent<asd.Layer2D>(name)

    let mutable nextID = 0u
    let actors = new Dictionary<uint32, 'Actor>()

    let viewModelSelecter = viewModelSelecter

    
    interface IObserver<'ViewModel> with
        member this.UpdateFromNotify(input) =
            this.Update(viewModelSelecter input)
    

    member this.Update(viewModel : UpdaterViewModel<_>) =
        if this.IsUpdated then
            this.AddActors(&viewModel)
            this.UpdateActors(&viewModel)


    member this.AddActors (viewModel : _ inref) =
        let newNextID = viewModel.nextID
        if nextID <> newNextID then
            
            for id in nextID..(newNextID - 1u) do
                viewModel.actors
                |> Map.tryFind id
                |> function
                | None -> ()
                | Some actorVM ->
                    let obj = new 'Actor()
                    actors.Add(id, obj)
                    this.Owner.AddObject(obj)

            nextID <- newNextID


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