module wraikny.MilleFeuille.Example.Core.UI.ControllerButton

open System.Linq
open wraikny.MilleFeuille.Core
open wraikny.MilleFeuille.Core.UI
open wraikny.MilleFeuille.Fs.Input.Controller

type Scene() =
    inherit Object.Scene()

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
            let c =
                new UI.Button.ControllerButtonComponent<asd.GeometryObject2D>()

            let defaultColor = new asd.Color(255uy, 255uy, 255uy)
            let hoverColor = new asd.Color(150uy, 150uy, 150uy)
            let holdColor = new asd.Color(50uy, 50uy, 50uy)
            
            c.add_Default(fun owner ->
                () //printfn "Default"
            )

            c.add_Hover(fun owner ->
                () //printfn "Hover"
            )

            c.add_Hold(fun owner ->
                () //printfn "Hold"
            )

            c.add_OnEntered(fun owner ->
                printfn "Button%d: OnEntered" index
                owner.Color <- hoverColor
            )

            c.add_OnPushed(fun owner ->
                printfn "Button%d: OnPushed" index
                owner.Color <- holdColor
            )

            c.add_OnSelected(fun owner ->
                printfn "Button%d: OnSelected" index
                owner.Color <- hoverColor
            )

            c.add_OnExited(fun owner ->
                printfn "Button%d: Onexited" index
                owner.Color <- defaultColor
            )

            c

        let btnCmp1 = createButton 1
        let btnCmp2 = createButton 2
        let btnCmp3 = createButton 3
        let btnCmp4 = createButton 4

        (createButtonObj -100.0f -100.0f).AddComponent(btnCmp1, "Button")
        (createButtonObj -100.0f  100.0f).AddComponent(btnCmp2, "Button")
        (createButtonObj  100.0f  100.0f).AddComponent(btnCmp3, "Button")
        (createButtonObj  100.0f -100.0f).AddComponent(btnCmp4, "Button")

        btnCmp1
            .Chain(btnCmp2, Button.ButtonDirection.Down)
            .Chain(btnCmp3, Button.ButtonDirection.Right)
            .Chain(btnCmp4, Button.ButtonDirection.Up)
            .Chain(btnCmp1, Button.ButtonDirection.Left)
            |> ignore

        uiLayer.AddObject(btnCmp1.Owner)
        uiLayer.AddObject(btnCmp2.Owner)
        uiLayer.AddObject(btnCmp3.Owner)
        uiLayer.AddObject(btnCmp4.Owner)

        let selecter = new Button.ControllerButtonSelecter(btnCmp1)

        let keyboard =
            KeyboardBuilder.init()
            |> KeyboardBuilder.bindKeys
                [
                    (Button.ControllerSelect.Up    , asd.Keys.Up)
                    (Button.ControllerSelect.Down  , asd.Keys.Down)
                    (Button.ControllerSelect.Right , asd.Keys.Right)
                    (Button.ControllerSelect.Left  , asd.Keys.Left)
                    (Button.ControllerSelect.Select, asd.Keys.Space)
                ]
            |> KeyboardBuilder.build

        selecter.AddController(keyboard) |> ignore

        uiLayer.AddComponent(selecter, "Selecter")

        ()