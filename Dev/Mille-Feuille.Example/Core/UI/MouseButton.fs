module wraikny.MilleFeuille.Example.Core.UI.MouseButton

open wraikny.MilleFeuille.Core

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


        let buttonComponent =
            let c = new UI.Button.MouseButtonComponent<asd.GeometryObject2D>(asd.MouseButtons.ButtonLeft)
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
                printfn "OnEntered"
                owner.Color <- hoverColor
            )

            c.add_OnPushed(fun owner ->
                printfn "OnPushed"
                owner.Color <- holdColor
            )

            c.add_OnSelected(fun owner ->
                printfn "OnSelected"
                owner.Color <- hoverColor
            )

            c.add_OnExited(fun owner ->
                printfn "Onexited"
                owner.Color <- defaultColor
            )

            c.add_OnOwnerAdded(fun owner ->
                let buttonCollider =
                    new asd.RectangleCollider(
                        Area = buttonArea
                        , IsVisible = true
                    )
                owner.AddCollider(buttonCollider)
            )

            c

        buttonObj.AddComponent(buttonComponent, "Button")

        uiLayer.AddObject(buttonObj)

        let selecter = new UI.Button.MouseButtonSelecter(mouse)
        selecter.AddButton(buttonComponent) |> ignore

        uiLayer.AddComponent(selecter, "MouseButtonSelecter")

        ()


let main () =
    asd.Engine.Initialize("Mouse Button", 800, 600, new asd.EngineOption())
    |> ignore

    let scene = new Scene()


    asd.Engine.ChangeScene(scene)
    

    let rec loop () =
        if asd.Engine.DoEvents() then
            asd.Engine.Update()
            loop ()

    loop()

    asd.Engine.Terminate()

    0