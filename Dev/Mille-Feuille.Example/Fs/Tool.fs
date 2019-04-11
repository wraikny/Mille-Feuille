namespace wraikny.MilleFeuille.Example.Fs.Tool

open wraikny.Tart.Core
open wraikny.Tart.Core.Libraries

open wraikny.MilleFeuille.Fs.Tool
open wraikny.MilleFeuille.Fs.Tool.Tree

module Counter =
    type Model =
        {
            count : int
            tmp : int
            range : int * int
        }

    let init : Model =
        {
            count = 0
            tmp = 0
            range = (1, 6)
        }

    type Msg =
        | SetValue of count : int * tmp : int
        | Clear
        | Random
        | SetCount of int
        | SetTmp of int
        | SetRange of int * int


    let update msg model : Model * Cmd<Msg, _> =
        let {count=count; tmp=tmp} = model
        msg |> function
        | SetValue(c, t) ->
            { model with
                count = c
                tmp = t
            }, Cmd.none
        | Clear ->
            init, Cmd.none
        | Random ->
            let a, b = model.range
            model, (Random.generate SetCount (Random.int a b))
        | SetCount i ->
            { model with count = i }, Cmd.none
        | SetTmp i ->
            { model with tmp = i }, Cmd.none
        | SetRange(a, b) ->
            { model with range = a, b }, Cmd.none


    let view model : ViewModel<Msg> =
        let
            {
                count = count
                tmp = tmp
                range = rMin, rMax
            }
            = model

        viewModel None [
            window "Counter" None [
                noColumn [
                    text <| sprintf "Count: %d" model.count
                    text <| sprintf "Tmp: %d" model.tmp
                    text <| sprintf "Random Range: %A" model.range
                    button "Clear" <| Event.message Clear

                    separator

                    inputInt "value" model.tmp SetTmp
                    button "Add" <| Event.message (SetValue(count + tmp, 0))
                    sameLine
                    button "Sub" <| Event.message (SetValue(count - tmp, 0))

                    separator

                    inputInt "Min" rMin <| fun i -> SetRange(i, rMax)
                    inputInt "Max" rMax <| fun i -> SetRange(rMin, i)
                    button "Random" <| Event.message Random
                ]
            ]
        ]


    let messengerBuilder updater : IMessenger<Msg, ViewModel<Msg>> =
        IMessenger.createMessenger
            {
                init = init
                update = update
                view = view
            }
            updater


    let main () =
        asd.Engine.Initialize("Counter", 800, 600, new asd.EngineOption())
        |> ignore

        let messenger = messengerBuilder None

        open' <| fun _ ->
            asd.Engine.TargetFPS <- 20

            let rec loop view =
                if asd.Engine.DoEvents() then
                    render view messenger

                    asd.Engine.Update()

                    messenger.TryViewModel |> function
                    | Some newView -> loop newView
                    | None -> loop view


            messenger.StartAsync() |> ignore
            
            messenger.TryViewModel |> function
            | Some view -> loop view
            | None -> ()
            
            messenger.Stop()

        asd.Engine.Terminate()

        0