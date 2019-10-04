namespace wraikny.MilleFeuille.ExampleFs.Fs.Tool

open wraikny.Tart.Core
open wraikny.MilleFeuille


module Counter =
    type ViewMsg =
        | Print of string

    module Core =
        open wraikny.Tart.Core.Libraries
        open wraikny.MilleFeuille.Tool
        open wraikny.MilleFeuille.Tool.Tree

        type Model =
            {
                count : int
                tmp : int
                range : int * int
            }

        type Msg =
            | SetValue of count : int * tmp : int
            | Clear
            | Random
            | SetCount of int
            | SetTmp of int
            | SetRange of int * int


        let init : Model * Cmd<Msg, ViewMsg> =
            {
                count = 0
                tmp = 0
                range = (1, 6)
            }, Cmd.none


        let update msg model : Model * Cmd<Msg, ViewMsg> =
            let {count=count; tmp=tmp} = model
            msg |> function
            | SetValue(c, t) ->
                { model with
                    count = c
                    tmp = t
                }, Cmd.none
            | Clear ->
                fst init, Cmd.ofPort(Print "Cleared!")
            | Random ->
                let a, b = model.range
                model, (SideEffect.performWith SetCount (Random.int a b))
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


        let program =
            {
                init = init
                update = update
                view = view
            }


    let main () =
        asd.Engine.Initialize("Counter", 800, 600, new asd.EngineOption())
        |> ignore


        let messenger =
            let env = { seed = System.Random().Next() }

            Messenger.Create(env, Core.program)

        messenger.ViewModel
            .Subscribe(fun x -> Tool.render x messenger.Enqueue) |> ignore

        messenger.ViewMsg.Subscribe(fun x ->
            x |> function
            | Print s -> printfn "Executed in mainthread: %s" s
        ) |> ignore


        Tool.open' <| fun _ ->
            messenger.StartAsync() |> ignore

            asd.Engine.TargetFPS <- 20


            while asd.Engine.DoEvents() do
                messenger.NotifyView()
                asd.Engine.Update()
            
            messenger.Stop()

        asd.Engine.Terminate()

        0