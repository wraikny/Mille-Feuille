namespace wraikny.MilleFeuille.ExampleFs.Fs.Tool

open wraikny.MilleFeuille
open Elmish

module Counter =
    module Core =
        open wraikny.MilleFeuille.Tool
        open wraikny.MilleFeuille.Tool.Tree

        let rand = System.Random()

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


        let init() : Model * Cmd<Msg> =
            {
                count = 0
                tmp = 0
                range = (1, 6)
            }, Cmd.none


        let update msg model : Model * Cmd<Msg> =
            msg |> function
            | SetValue(c, t) ->
                { model with
                    count = c
                    tmp = t
                }, Cmd.none
            | Clear ->
                fst <| init(), Cmd.none
            | Random ->
                let a, b = model.range
                { model with count = rand.Next(a,b) }, Cmd.none
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
    open Core

    let program = Program.mkProgram init update (fun m _ -> m)


    let main () =
        asd.Engine.Initialize("Counter", 800, 600, new asd.EngineOption())
        |> ignore

        let sc = QueueSynchronizationContext()
        let renderer = Tool.Renderer<_>()

        program
        |> Program.withSetState(fun model dispatch ->
            renderer.SetState(Core.view model, dispatch)
        )
        |> Program.run
        Tool.open' <| fun _ ->
            asd.Engine.TargetFPS <- 30

            while asd.Engine.DoEvents() do
                renderer.Render()
                sc.Execute()
                asd.Engine.Update()

        asd.Engine.Terminate()

        0