module wraikny.MilleFeuille.Example.Core.UI.MouseButton

open wraikny.MilleFeuille.Core

type Scene() =
    inherit Object.Scene()

    let uiLayer = new asd.Layer2D()

    override this.OnRegistered() =
        this.AddLayer(uiLayer)

        let mouse = new Input.Mouse.CollidableMouse(5.0f)
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
                , Color = new asd.Color(255uy, 255uy, 255uy, 255uy)
                , Position = asd.Engine.WindowSize.To2DF() / 2.0f
            )
        uiLayer.AddObject(buttonObj)

        let buttonComponent =
            let c = new UI.Button.ButtonComponent<asd.GeometryObject2D>()
            c.add_Default(fun owner ->
                () //printfn "Default"
            )

            c.add_Hover(fun owner ->
                () //printfn "Hover"
            )

            c.add_Hold(fun owner ->
                () //printfn "Hold"
            )

            c.add_OnEnter(fun owner ->
                printfn "OnEnter"
            )

            c.add_OnPushed(fun owner ->
                printfn "OnPushed"
            )

            c.add_OnSelected(fun owner ->
                printfn "OnSelected"
            )

            c.add_OnExit(fun owner ->
                printfn "Onexit"
            )

            c

        let buttonCollider = new asd.RectangleCollider(Area = buttonArea)
        let mouseButton = new UI.Button.MouseButton(buttonObj, buttonCollider);
        mouseButton.SetButtonComponent(asd.MouseButtons.ButtonLeft, buttonComponent)

        let selecter = new UI.Button.MouseButtonSelecter(mouse)
        selecter.AddButton(mouseButton) |> ignore

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