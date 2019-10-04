module wraikny.MilleFeuille.ExampleFs.Animation

open wraikny.Tart.Helper
open wraikny.MilleFeuille
open wraikny.MilleFeuille.Input
open Affogato

type AnimState =
    | First
    | Default
    | Rotate
    | Color


module TestAnims =
    let rotation easing frame (s, e) (owner : asd.Object2D) =
        seq {
            for i in 0..frame ->
                let x = Easing.calculate easing frame i

                owner.Angle <- s + (e - s) * x
        }


    let color easing frame (s, e) (owner : asd.DrawnObject2D) =
        seq {
            for i in 0..frame ->
                let x = Easing.calculate easing frame i

                let b = (s + (e - s) * x) |> byte
                owner.Color <- new asd.Color(b, b, b)
        }

    let firstAnim isFinishedFirst =
        AnimationBuilder.init "First Animation"
        |> AnimationBuilder.addCoroutine
            (fun (owner : asd.DrawnObject2D) -> seq {
                printfn "First Animation: Begin"

                isFinishedFirst := false
                owner.Color <- new asd.Color(0uy, 0uy, 0uy)
                yield ()

                yield! Coroutine.sleep 60
                yield! Coroutine.asParallel
                    [
                        owner |> rotation Easing.OutQuad 120 (0.0f, 180.0f)
                        owner |> color Easing.OutQuad 120 (0.0f, 255.0f)
                    ]

                printfn "First Animation: End"
                isFinishedFirst := true
                yield ()
            })

    let defaultAnim defaultColor =
        AnimationBuilder.init "Default Animation"
        |> AnimationBuilder.addCoroutine
            (fun (owner : asd.DrawnObject2D) -> seq {
                printfn "Default Animation: Begin"
                owner.Angle <- 0.0f
                owner.Color <- defaultColor
                yield ()

                printfn "Default Animation: Begin"
                yield()
            })
    
    let rotateAnim =
        AnimationBuilder.init "Rotate Animation"
        |> AnimationBuilder.addCoroutine
            (fun (owner : asd.Object2D) -> seq {
                printfn "Rotate Animation: Begin"
                let first = owner.Angle

                yield! owner |> rotation Easing.InOutBack 180 (first, first + 180.0f)
                yield! Coroutine.sleep 60

                printfn "Rotate Animation: End"
                yield ()
            })
    
    let colorAnim =
        AnimationBuilder.init "Color Animation"
        |> AnimationBuilder.addCoroutine
            (fun (owner : asd.DrawnObject2D) -> seq {
                printfn "Color Animation: Begin"
                let frame = 60

                yield! owner |> color Easing.InOutCubic frame (255.0f, 0.0f)
                yield! owner |> color Easing.InOutCubic frame (0.0f, 255.0f)
                yield! Coroutine.sleep 30
                    
                printfn "Color Animation: End"
                yield()

            })

    let createComponent defaultColor isFinishedFirst =
        (AnimationControllerBuilder.init "Test Animation"
            : AnimationControllerBuilder<asd.GeometryObject2D, _>
        )
        |> AnimationControllerBuilder.addNodesList
            [
                First, {
                    animation = firstAnim isFinishedFirst
                    next = Some Default
                } |> NodeBuilder.build
                Default, {
                    animation = defaultAnim defaultColor
                    next = None
                } |> NodeBuilder.build
                Rotate, {
                    animation = rotateAnim
                    next = Some Color
                } |> NodeBuilder.build
                Color, {
                    animation = colorAnim
                    next = Some Rotate
                } |> NodeBuilder.build
            ]
        |> AnimationControllerBuilder.buildComponent "TestObj Animator"


type AnimScene() =
    inherit Scene()

    let mainLayer = new asd.Layer2D()
    let keyboard =
        KeyboardBuilder.init()
        |> KeyboardBuilder.bindKeysList
            [
                Default, asd.Keys.Num1
                Rotate , asd.Keys.Num2
                Color  , asd.Keys.Num3
            ]
        |> KeyboardBuilder.build

    let defaultColor = new asd.Color(255uy, 255uy, 255uy)
    let isFinishedFirst = ref false

    let animComponent =
        TestAnims.createComponent
            defaultColor
            isFinishedFirst
    

    override this.OnRegistered() =
        let size = new asd.Vector2DF(200.0f, 200.0f)

        let testObj =
            new asd.GeometryObject2D(
                Shape =
                    new asd.RectangleShape(
                        DrawingArea =
                            new asd.RectF(-size / 2.0f, size)
                    )
                , Position =
                    asd.Engine.WindowSize.To2DF() / 2.0f
            )

        testObj.AddComponent(animComponent, animComponent.Name)
        animComponent.Start(First)

        this.AddLayer(mainLayer)
        mainLayer.AddObject(testObj)


    override this.OnUpdated() =
        if !isFinishedFirst then
            let setAnimationStates state =
                keyboard.GetState(state) |> Option.ofNullable
                |> function
                | Some asd.ButtonState.Push ->
                    animComponent.Start(state)
                    
                | _ -> ()

            setAnimationStates Default
            setAnimationStates Rotate
            setAnimationStates Color


