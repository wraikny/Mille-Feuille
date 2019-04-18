module wraikny.MilleFeuille.Example.Core.UI.MouseButton

open wraikny.MilleFeuille.Core
open wraikny.MilleFeuille.Fs.UI.Button

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

        let mouse = new Input.Mouse.CollidableMouse(10.0f)
        mouse.ColliderVisible <- true
        uiLayer.AddObject(mouse)


        let buttonArea =
            let size = new asd.Vector2DF(300.0f, 150.0f)
            new asd.RectF(-size / 2.0f, size)


        let buttonObj =
            new asd.GeometryObject2D(
                Shape =
                    new asd.RectangleShape(
                        DrawingArea = buttonArea
                    )
                , Color = new asd.Color(255uy, 255uy, 255uy)
                , Position = asd.Engine.WindowSize.To2DF() / 2.0f
            )


        let buttonComponent index =
            let defaultColor = new asd.Color(255uy, 255uy, 255uy)
            let hoverColor = new asd.Color(150uy, 150uy, 150uy)
            let holdColor = new asd.Color(50uy, 50uy, 50uy)

            (ButtonBuilder.init() : ButtonBuilder<asd.GeometryObject2D>)
            //|> ButtonBuilder.addDefault(fun owner -> ())
            //|> ButtonBuilder.addHover(fun owner -> ())
            //|> ButtonBuilder.addHold(fun owner -> ())
            |> ButtonBuilder.addOnEntered(fun owner ->
                printfn "Button%d: OnEntered" index
                owner.Color <- hoverColor
            )
            |> ButtonBuilder.addOnPushed(fun owner ->
                printfn "Button%d: OnPushed" index
                owner.Color <- holdColor
            )
            |> ButtonBuilder.addOnSelected(fun owner ->
                printfn "Button%d: OnSelected" index
                owner.Color <- hoverColor
            )
            |> ButtonBuilder.addOnEntered(fun owner ->
                printfn "Button%d: Onexited" index
                owner.Color <- defaultColor
            )
            |> ButtonBuilder.buildMouse asd.MouseButtons.ButtonLeft

        let btn0 = buttonComponent 0

        buttonObj.AddComponent(btn0, "Button")

        uiLayer.AddObject(buttonObj)

        let selecter = new UI.Button.MouseButtonSelecter(mouse)
        selecter.AddButton(btn0) |> ignore

        uiLayer.AddComponent(selecter, "MouseButtonSelecter")

        ()