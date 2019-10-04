module wraikny.MilleFeuille.ExampleFs.UI.ControllerButton

open System.Linq
open wraikny.MilleFeuille
open wraikny.MilleFeuille.UI
open wraikny.MilleFeuille.Input

type Scene() =
    inherit wraikny.MilleFeuille.Scene()

    let uiLayer = new asd.Layer2D()

    override this.OnRegistered() =
        this.AddLayer(uiLayer)

        uiLayer.AddObject <|
        new asd.GeometryObject2D(
            Shape =
                new asd.RectangleShape(
                    DrawingArea = new asd.RectF(
                        new asd.Vector2DF(0.0f, 0.0f)
                        , asd.Engine.WindowSize.To2DF()
                    )
                )
            , Color =
                new asd.Color(0uy, 100uy, 150uy)
        )

        let createButtonObj x y =
            let buttonArea =
                let size = new asd.Vector2DF(150.0f, 150.0f)
                new asd.RectF(-size / 2.0f, size)

            new asd.GeometryObject2D(
                Shape =
                    new asd.RectangleShape(
                        DrawingArea = buttonArea
                    )
                , Color = new asd.Color(255uy, 255uy, 255uy)
                , Position = asd.Engine.WindowSize.To2DF() / 2.0f + new asd.Vector2DF(x, y)
            )

        let createButton index =
            let defaultColor = new asd.Color(255uy, 255uy, 255uy)
            let hoverColor = new asd.Color(150uy, 150uy, 150uy)
            let holdColor = new asd.Color(50uy, 50uy, 50uy)

            ButtonBuilder.init()
            //|> ButtonBuilder.addDefaultEvent(fun owner -> ())
            //|> ButtonBuilder.addHoverEvent(fun owner -> ())
            //|> ButtonBuilder.addHoldEvent(fun owner -> ())
            |> ButtonBuilder.addOnEnteredEvent(fun (owner : asd.GeometryObject2D) ->
                printfn "Button%d: OnEntered" index
                owner.Color <- hoverColor
            )
            |> ButtonBuilder.addOnPushedEvent(fun owner ->
                printfn "Button%d: OnPushed" index
                owner.Color <- holdColor
            )
            |> ButtonBuilder.addOnReleasedEvent(fun owner ->
                printfn "Button%d: OnReleased" index
                owner.Color <- hoverColor
            )
            |> ButtonBuilder.addOnExitedEvent(fun owner ->
                printfn "Button%d: Onexited" index
                owner.Color <- defaultColor
            )
            |> ButtonBuilder.buildController "Button"

        let btnCmp1 = createButton 1
        let btnCmp2 = createButton 2
        let btnCmp3 = createButton 3
        let btnCmp4 = createButton 4

        (createButtonObj -100.0f -100.0f).AddComponent(btnCmp1, btnCmp1.Name)
        (createButtonObj -100.0f  100.0f).AddComponent(btnCmp2, btnCmp2.Name)
        (createButtonObj  100.0f  100.0f).AddComponent(btnCmp3, btnCmp3.Name)
        (createButtonObj  100.0f -100.0f).AddComponent(btnCmp4, btnCmp4.Name)

        btnCmp1
            .Chain(btnCmp2, ButtonDirection.Down)
            .Chain(btnCmp3, ButtonDirection.Right)
            .Chain(btnCmp4, ButtonDirection.Up)
            .Chain(btnCmp1, ButtonDirection.Left)
            |> ignore

        uiLayer.AddObject(btnCmp1.Owner)
        uiLayer.AddObject(btnCmp2.Owner)
        uiLayer.AddObject(btnCmp3.Owner)
        uiLayer.AddObject(btnCmp4.Owner)

        let selecter = new ControllerButtonSelecter(btnCmp1)

        let keyboard =
            KeyboardBuilder.init()
            |> KeyboardBuilder.bindKeysList
                [
                    ControllerSelect.Up    , asd.Keys.Up
                    ControllerSelect.Down  , asd.Keys.Down
                    ControllerSelect.Right , asd.Keys.Right
                    ControllerSelect.Left  , asd.Keys.Left
                    ControllerSelect.Select, asd.Keys.Space
                    ControllerSelect.Cancel, asd.Keys.Escape
                ]
            |> KeyboardBuilder.build

        selecter.AddController(keyboard) |> ignore

        uiLayer.AddComponent(selecter, "Selecter")

        ()