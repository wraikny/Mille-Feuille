module wraikny.MilleFeuille.ExampleFs.Core.Animation

open wraikny.Tart.Helper.Math

open wraikny.MilleFeuille.Core.Object
open wraikny.MilleFeuille.Fs.Animation
open wraikny.MilleFeuille.Fs.Input.Controller


type AnimState =
    | First
    | Default
    | Rotate
    | Color


module TestAnims =
    open System.Collections
    open System.Linq

    let waitFrames n = seq{ for _ in 1..n -> () }

    let asParallel (coroutines : #seq<seq<unit>>) =
        seq {
            let coroutines =
                (coroutines
                |> Seq.map(fun c -> c.GetEnumerator() :> IEnumerator)
                ).ToList()

            let mutable isContinue = true

            while isContinue do
                isContinue <- false
                for c in coroutines do
                    if c.MoveNext() && isContinue = false then
                        isContinue <- true
                yield ()
        }

    let firstAnim =
        AnimationBuilder.init "First Animation"
        |> AnimationBuilder.addCoroutine
            (fun (owner : asd.GeometryObject2D) -> seq {
                printfn "First Animation: Begin"

                let rot = seq {
                    let frame = 120
                    for i in 0..frame do
                        let x =
                            Interpolation.getEasing
                                Interpolation.Easing.InOutQuad
                                frame
                                i

                        owner.Angle <- x * 180.0f
                        yield ()
                }

                let col = seq {
                    for i in 0..60 do
                        let i = 255.0f * (float32 i / float32 60) |> byte
                        owner.Color <- new asd.Color(i, i, i)
                        yield ()
                }

                owner.Color <- new asd.Color(0uy, 0uy, 0uy)

                yield! waitFrames 60
                yield! asParallel [rot; col]

                printfn "First Animation: End"
                yield()
            })

    let defaultAnim defaultColor =
        AnimationBuilder.init "Default Animation"
        |> AnimationBuilder.addCoroutine
            (fun (owner : asd.GeometryObject2D) -> seq {
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
            (fun (owner : asd.GeometryObject2D) -> seq {
                printfn "Rotate Animation: Begin"
                let firstRotation = owner.Angle

                for i in 1..(60 * 3) do
                    owner.Angle <- firstRotation + float32 i
                    yield ()
    
                yield! waitFrames 60

                printfn "Rotate Animation: End"
                yield ()
            })
    
    let colorAnim =
        AnimationBuilder.init "Color Animation"
        |> AnimationBuilder.addCoroutine
            (fun (owner : asd.GeometryObject2D) -> seq {
                printfn "Color Animation: Begin"
                let frame = 60
                for i in 0..frame do
                    let i = (1.0f - float32 i / float32 frame) * 255.0f |> byte
                    owner.Color <- new asd.Color(i, i, i)
                    yield ()

                for i in 0..frame do
                    let i = (float32 i / float32 frame) * 255.0f |> byte
                    owner.Color <- new asd.Color(i, i, i)
                    yield ()
    
                yield! waitFrames 30
                    
                printfn "Color Animation: End"
                yield()

            })

    let createComponent defaultColor =
        AnimationControllerBuilder.init "Test Animation"
            [
                (First, {animation = firstAnim; next = Some Default })
                (Default, {animation = defaultAnim defaultColor; next = None })
                (Rotate, {animation = rotateAnim; next = Some Color })
                (Color, {animation = colorAnim; next = Some Rotate })
            ]
        |> AnimationControllerBuilder.buildComponent "TestObj Animator"


type AnimScene() =
    inherit Scene()

    let mainLayer = new asd.Layer2D()
    let keyboard =
        KeyboardBuilder.init()
        |> KeyboardBuilder.bindKeys
            [
                (Default, asd.Keys.Num1)
                (Rotate , asd.Keys.Num2)
                (Color  , asd.Keys.Num3)
            ]
        |> KeyboardBuilder.build

    let defaultColor = new asd.Color(255uy, 255uy, 255uy)
    let animComponent =
        TestAnims.createComponent
            defaultColor
    

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
        animComponent.State <- First


        this.AddLayer(mainLayer)
        mainLayer.AddObject(testObj)


    override this.OnUpdated() =
        let setAnimationStates state =
            keyboard.GetState(state) |> Option.ofNullable
            |> function
            | Some asd.ButtonState.Push ->
                animComponent.State <- state
            | _ -> ()

        setAnimationStates Default
        setAnimationStates Rotate
        setAnimationStates Color


