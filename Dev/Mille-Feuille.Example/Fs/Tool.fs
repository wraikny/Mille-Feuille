namespace wraikny.MilleFeuille.ExampleFs.Fs.Tool

open wraikny.Tart.Core
open wraikny.MilleFeuille.Fs


module Counter =
    type ViewMsg =
        | Print of string

    module Core =
        open wraikny.Tart.Core.Libraries
        open wraikny.MilleFeuille.Fs.Tool
        open wraikny.MilleFeuille.Fs.Tool.Tree

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



        let update msg model : Model * Cmd<Msg, ViewMsg> =
            let {count=count; tmp=tmp} = model
            msg |> function
            | SetValue(c, t) ->
                { model with
                    count = c
                    tmp = t
                }, Cmd.none
            | Clear ->
                init, Cmd.pushViewMsgs [Print "Cleared!"]
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

        let program =
            {
                init = init
                update = update
                view = view
            }
 
    type CounterUpdater() =
        inherit Updater<ViewMsg>()

        override this.OnUpdate(msg) =
            msg |> function
            | Print s -> printfn "Executed in mainthread: %s" s


    let main () =
        asd.Engine.Initialize("Counter", 800, 600, new asd.EngineOption())
        |> ignore

        let updater = CounterUpdater()

        let messenger =
            let env =
                Environment
                    .Initialize()
                    .SetUpdater(updater)

            Messenger.createMessenger
                env
                Core.program
                

        Tool.open' <| fun _ ->
            asd.Engine.TargetFPS <- 20

            let rec loop view =
                if asd.Engine.DoEvents() then
                    Tool.render view messenger

                    updater.Update()

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