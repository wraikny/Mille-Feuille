namespace wraikny.MilleFeuille.UI

open System.Collections.Generic

open wraikny.Tart.Helper
open wraikny.Tart.Math
open wraikny.MilleFeuille
open wraikny.MilleFeuille.UI
open wraikny.MilleFeuille

//open Bellturrim.View

open FSharpPlus


type Item =
    | Space of float32
    | Text of string
    | TextWith of string * asd.Font
    | Button of string * (unit -> unit)
    | InputField of int * placeholder:string * current:string option * (string -> unit)
    | Rect of thickness:float32 * wRate:float32
    | RectWith of thickness:float32 * wRate:float32 * asd.Color


module WindowSetting =
    type State =
        | Open
        | Close

    type internal RenderingState =
        | Update

    type Alignment = Right of float32 | Left of float32 | Center

    type WindowSize = FixWidth of float32 | Auto | Fixed of asd.Vector2DF * isCenterd:bool

    type ToggleDirection =
        | X
        | Y
        | XY

    with
        member dir.RateFun =
            dir |> function
            | X -> fun a -> asd.Vector2DF(a, 1.0f)
            | Y -> fun a -> asd.Vector2DF(1.0f, a)
            | XY -> fun a -> asd.Vector2DF(a, a)


    type ButtonSize =
        | Auto of margin:asd.Vector2DF
        | Fixed of asd.Vector2DF
        | SetMax of margin:asd.Vector2DF
        | AutoFit of margin:asd.Vector2DF * rate:float32

    type InputFieldSize =
        | Fixed of asd.Vector2DF
        | AutoFit of margin : asd.Vector2DF * rate:float32

    type Object =
        | ButtonObj of MouseButton * asd.Vector2DF
        | InputFieldObj of MouseInputField * asd.Vector2DF
        | TextObj of asd.TextObject2D * asd.Vector2DF
        | RectObj of asd.GeometryObject2D * asd.Vector2DF
    with
        member inline x.DrawnObject =
            x |> function
            | ButtonObj (o, _) -> o :> asd.DrawnObject2D
            | InputFieldObj (o, _) -> o :> asd.DrawnObject2D
            | TextObj (o, _) -> o :> asd.DrawnObject2D
            | RectObj (o, _) -> o :> asd.DrawnObject2D

    let gray = asd.Color(100, 100, 100, 255)
    let gray_hv = asd.Color(50, 50, 50, 255)
    let gray_hd = asd.Color(10, 10, 10, 255)


type WindowSetting = {
    windowSize : WindowSetting.WindowSize
    toggleDirection : WindowSetting.ToggleDirection
    centerPositionRate : float32 Vec2
    togglePositionRate : float32 Vec2

    frameColor : asd.Color

    buttonSize : WindowSetting.ButtonSize
    inputFieldSize : WindowSetting.InputFieldSize
    button : ButtonColor
    inputColor : ButtonColor
    inputFocusColor : ButtonColor

    rectColor : asd.Color

    textFont : asd.Font
    buttonFont : asd.Font
    inputFont : asd.Font

    textMargin : asd.Vector2DF

    itemMargin : float32
    itemAlignment : WindowSetting.Alignment

    openEasing : Easing
    closeEasing : Easing
    animationFrame : uint32
}
with
    static member inline Default(font) =
        let buttonColor = {
            defaultColor = WindowSetting.gray
            hoverColor = WindowSetting.gray_hv
            holdColor = WindowSetting.gray_hd
        }
        {
            windowSize = WindowSetting.Auto
            toggleDirection = WindowSetting.XY
            centerPositionRate = zero
            togglePositionRate = zero

            buttonSize = WindowSetting.ButtonSize.AutoFit(asd.Vector2DF(5.0f, 5.0f), 0.8f)
            inputFieldSize = WindowSetting.InputFieldSize.AutoFit(asd.Vector2DF(5.0f, 5.0f), 0.8f)
            itemAlignment = WindowSetting.Center

            itemMargin = 10.0f
            textMargin = asd.Vector2DF(5.0f, 5.0f)

            openEasing = Easing.OutExpo
            closeEasing = Easing.InExpo
            animationFrame = 15u

            frameColor = asd.Color()
            rectColor = asd.Color()
            textFont = font
            buttonFont = font
            inputFont = font
            button = buttonColor
            inputColor = buttonColor
            inputFocusColor = buttonColor
        }


type IToggleWindow =
    abstract Toggle : bool -> unit
    abstract Toggle : bool * (unit -> unit) -> unit
    abstract IsToggleOn : bool with get
    abstract IsToggleAnimating : bool with get
    //abstract ClearItems : unit -> unit



type MouseWindow(setting : WindowSetting, mouse : MouseButtonSelecter) as this =
    inherit asd.GeometryObject2D()

    // let defaultToggleOn = defaultArg defaultToggleOn false

    let mutable mouse = mouse
    let mutable currentSize = asd.Vector2DF()

    let frameRect = new asd.RectangleShape()

    let frameObj = new asd.GeometryObject2D(Shape = frameRect, Color = setting.frameColor)

    let itemParent = new asd.GeometryObject2D(IsDrawn = false, IsUpdated = false)

    let mutable isToggleOn = false
    let mutable isToggleAnimating = false
    let mutable isContentsAnimating = false

    let mutable toggleCallback = fun() -> ()

    let waitContents = seq { while isContentsAnimating do yield() }

    let toggleAnimComponent =
        let openAnim = 
            AnimationBuilder.init "OpenAnimation"
            |> AnimationBuilder.addCoroutine(fun _ -> seq {

                isToggleAnimating <- true
                let setting = this.WindowSetting

                let frame = int setting.animationFrame
                let f = Easing.calculate setting.openEasing frame
                yield()

                // Waiting UIContens Updated
                yield! waitContents

                let size0 = this.CurrentSize
                let dirRate = setting.toggleDirection.RateFun

                let centerPosRate = Vec2.toVector2DF setting.centerPositionRate
                let togglePosRate = Vec2.toVector2DF setting.togglePositionRate
                let togglePos0 = size0 * (togglePosRate - centerPosRate)

                yield! seq {
                    for i in 1..frame ->
                        let a = f i
                        let size : asd.Vector2DF = size0 * dirRate a
                        frameRect.DrawingArea <- asd.RectF(asd.Vector2DF(), size)
                        let togglePosA = size * (togglePosRate - centerPosRate)
                        frameObj.Position <- -size * centerPosRate + (togglePos0 - togglePosA)
                }
                
                itemParent.IsUpdated <- true
                itemParent.IsDrawn <- true

                isToggleOn <- true
                isToggleAnimating <- false
                toggleCallback()
                yield()
            })
        let closeAnim = 
            AnimationBuilder.init "CloseAnimation"
            |> AnimationBuilder.addCoroutine(fun _ -> seq {

                isToggleAnimating <- true
                let setting = this.WindowSetting

                itemParent.IsUpdated <- false
                itemParent.IsDrawn <- false

                let frame = int setting.animationFrame
                let f = Easing.calculate setting.closeEasing frame
                yield()

                // Waiting UIContens Updated
                yield! waitContents
                let size0 = this.CurrentSize

                let dirRate = setting.toggleDirection.RateFun

                let centerPosRate = Vec2.toVector2DF setting.centerPositionRate
                let togglePosRate = Vec2.toVector2DF setting.togglePositionRate
                let togglePos0 = size0 * (togglePosRate - centerPosRate)

                yield! seq {
                    for i in 1..frame ->
                        let a = 1.0f - (f i)
                        let size : asd.Vector2DF = size0 * dirRate a
                        frameRect.DrawingArea <- asd.RectF(asd.Vector2DF(), size)
                        
                        let togglePosA = size * (togglePosRate - centerPosRate)
                        frameObj.Position <- -size * centerPosRate + (togglePos0 - togglePosA)
                }

                isToggleOn <- false
                isToggleAnimating <- false
                toggleCallback()
                yield()
            })

        AnimationControllerBuilder.init "Toggle Animation"
        |> AnimationControllerBuilder.addNodeBuildersList [
            WindowSetting.Open, {
                animation = openAnim
                next = None
            }
            WindowSetting.Close, {
                animation = closeAnim
                next = None
            }
        ]
        |> AnimationControllerBuilder.buildComponent "Toggle Animtion Component"


    let renderedObjects = new List<WindowSetting.Object>()
    let mutable renderingItemsAreUpdated = false
    let mutable uiContents = []
    let renderingAnimComponent =
        let updateItemComponent =
            AnimationBuilder.init "Item Update Animation"
            |> AnimationBuilder.addCoroutine(fun _ -> seq {
                isContentsAnimating <- true
                this.ClearItems()
                // yield()

                this.UpdateItems()
                isContentsAnimating <- false
                yield()
            })
        AnimationControllerBuilder.init "Rendering Animation"
        |> AnimationControllerBuilder.addNodeBuildersList [
            WindowSetting.Update, {
                animation = updateItemComponent
                next = None
            }
        ]
        |> AnimationControllerBuilder.buildComponent "Rendering Animation Component"

    do
        toggleAnimComponent.Attach(this)
        renderingAnimComponent.Attach(this)


    interface IToggleWindow with
        member this.Toggle(x) = this.Toggle(x)
        member this.Toggle(x, callback) = this.Toggle(x, callback)
        member this.IsToggleOn with get() = this.IsToggleOn
        member this.IsToggleAnimating with get() = this.IsToggleAnimating
        //member this.UIContents
        //    with get() = this.UIContents
        //    and set(x) = this.UIContents <- x
        //member this.ClearItems() = this.ClearItems()


    member __.Mouse
        with get() = mouse
        and set(x) = mouse <- x

    member __.CurrentSize
        with get() : asd.Vector2DF = currentSize
        //and set(v) = currentSize <- v

    member __.IsToggleAnimating with get() = isToggleAnimating

    member __.IsToggleOn with get() = isToggleOn

    member __.Toggle(toggle : bool, ?callback) =
        if not isToggleAnimating then
            toggleCallback <- defaultArg callback <| fun() -> ()
            toggleAnimComponent.Start(if toggle then WindowSetting.Open else WindowSetting.Close)

    //member __.Focus(x : bool) =
    //    frameObj.IsUpdated <- x

    member private __.WindowSetting with get() = setting

    member __.UIContents
        with get() = uiContents
        and set(value) =
            if seq value <> seq uiContents then
                uiContents <- value
                this.ReRendering()

    member __.ReRendering() = renderingItemsAreUpdated <- true

    override this.OnAdded() =
        this.AddDrawnChildWithoutColor(frameObj)
        frameObj.AddDrawnChildWithoutColor(itemParent)


    override this.OnUpdate() =
        if renderingItemsAreUpdated then
            renderingItemsAreUpdated <- false
            renderingAnimComponent.Start(WindowSetting.Update)
        
    member this.ClearItems() =
        for item in renderedObjects do
            item |> function
            | WindowSetting.ButtonObj (o, _) ->
                mouse.RemoveButton(o.Button) |> ignore
            | _ -> ()

            item.DrawnObject.Dispose()

        renderedObjects.Clear()

    member private this.UpdateItems() =
        let inline addItem o =
            itemParent.AddDrawnChild(
                o,
                asd.ChildManagementMode.RegistrationToLayer |||
                asd.ChildManagementMode.IsDrawn |||
                asd.ChildManagementMode.IsUpdated,
                asd.ChildTransformingMode.All,
                asd.ChildDrawingMode.DrawingPriority
            )

        let buttonFont = this.WindowSetting.buttonFont
        let textFont = this.WindowSetting.textFont

        let inline calcVecMax (xs : seq<asd.Vector2DF>) =
            asd.Vector2DF(
                xs
                |>> fun x -> x.X
                |> Seq.append [| 0.0f |]
                |> Seq.max,
                xs
                |>> fun x -> x.Y
                |> Seq.append [| 0.0f |]
                |> Seq.max
            )


        let buttonSizeMax = lazy(
            seq {
                for x in this.UIContents do
                    match x with
                    | Button(text, _) ->
                        let textSize = buttonFont.HorizontalSize(text).To2DF()
                        let size = this.WindowSetting.buttonSize |> function
                            | WindowSetting.ButtonSize.Fixed size -> size
                            | WindowSetting.ButtonSize.Auto margin
                            | WindowSetting.SetMax margin
                            | WindowSetting.ButtonSize.AutoFit (margin, _) ->
                                textSize + margin * 2.0f
                        yield size
                    | InputField (length, _, _, _) ->
                        let size = this.WindowSetting.inputFieldSize |> function
                            | WindowSetting.InputFieldSize.Fixed size -> size
                            | WindowSetting.InputFieldSize.AutoFit (margin, _) ->
                                let text = [for _ in 1..length -> "A"] |> String.concat ""
                                let textSize = buttonFont.HorizontalSize(text).To2DF()
                                textSize + margin * 2.0f
                        yield size
                    | _ -> ()
            } |> calcVecMax
        )

        let textSizeMax = lazy(
            seq {
                for x in this.UIContents do
                    match x with
                    | Text(text) ->
                        yield textFont.HorizontalSize(text).To2DF() + (setting.textMargin * 2.0f)
                    | TextWith(text, font) ->
                        yield font.HorizontalSize(text).To2DF() + (setting.textMargin * 2.0f)
                    | _ -> ()
            }
            |> calcVecMax
        )

        let maxWidth = lazy( max (buttonSizeMax.Force().X) (textSizeMax.Force().X) )

        let currentWidth =
            this.WindowSetting.windowSize |> function
            | WindowSetting.WindowSize.Auto ->
                let i =
                    this.WindowSetting.itemAlignment |> function
                    | WindowSetting.Left i -> i
                    | WindowSetting.Right i -> i
                    | WindowSetting.Center -> 0.0f
                maxWidth.Force() + i * 2.0f
            | WindowSetting.WindowSize.FixWidth x -> x
            | WindowSetting.WindowSize.Fixed (v,  _) -> v.X

        itemParent.Position <-
            let x = 
                this.WindowSetting.itemAlignment |> function
                | WindowSetting.Left i -> i
                | WindowSetting.Right i -> currentWidth - i
                | WindowSetting.Center -> currentWidth / 2.0f
            asd.Vector2DF(x, 0.0f)

        let mutable posY = this.WindowSetting.itemMargin

        let inline setPosition (size : asd.Vector2DF) (o : asd.Object2D) =
            let posX =
                this.WindowSetting.itemAlignment |> function
                | WindowSetting.Left _ -> 0.0f
                | WindowSetting.Right _ -> -size.X
                | WindowSetting.Center -> -size.X * 0.5f
            o.Position <- asd.Vector2DF(posX, posY)
            posY <- posY + size.Y + this.WindowSetting.itemMargin


        let inline createText(text, font) =
            let o = new asd.TextObject2D(Font = font)
            addItem(o)
            o.Text <- text

            let size = font.HorizontalSize(text).To2DF()
            o |> setPosition size

            renderedObjects.Add(WindowSetting.TextObj (o, size))

        let inline createRect(thickness, wRate, color) =
            let size = asd.Vector2DF(currentWidth * wRate, thickness)
            let rect = new asd.RectangleShape(DrawingArea = asd.RectF(asd.Vector2DF(), size))
            let o = new asd.GeometryObject2D(Shape = rect, Color = color)

            addItem(o)

            o |> setPosition size

            renderedObjects.Add(WindowSetting.RectObj (o, size))

        for item in this.UIContents do
            item |> function
            | Button (text, f) ->
                let o = new MouseButton(buttonFont, this.WindowSetting.button)
                addItem(o)

                o.Text <- text
                o.Button.add_OnReleasedEvent(fun _ -> f())


                let textSize = buttonFont.HorizontalSize(text).To2DF()
                let size = this.WindowSetting.buttonSize |> function
                    | WindowSetting.ButtonSize.Auto margin ->
                        textSize + margin * 2.0f
                    | WindowSetting.ButtonSize.Fixed size -> size
                    | WindowSetting.ButtonSize.AutoFit (_, rate) ->
                        asd.Vector2DF(currentWidth * rate, buttonSizeMax.Force().Y)
                    | WindowSetting.SetMax _ ->
                        buttonSizeMax.Force()

                o.Size <- size
                o |> setPosition size
                
                renderedObjects.Add(WindowSetting.ButtonObj (o, size))
                mouse.AddButton(o.Button) |> ignore

            | InputField(maxLen, placeholder, current, f) ->
                let setting = this.WindowSetting
                let o = new MouseInputField(setting.inputFont, setting.inputColor, setting.inputFocusColor, maxLen, placeholder, current)
                addItem(o)

                o.OnInputEvent.Add f

                let size = setting.inputFieldSize |> function
                    | WindowSetting.InputFieldSize.Fixed size -> size
                    | WindowSetting.InputFieldSize.AutoFit (_, rate) ->
                        asd.Vector2DF(currentWidth * rate, buttonSizeMax.Force().Y)

                o.Size <- size
                o |> setPosition size

                renderedObjects.Add(WindowSetting.InputFieldObj (o, size))
                mouse.AddButton(o.Button) |> ignore



            | TextWith (text, font) ->
                createText(text, font)
            
            | Text text ->
                createText(text, textFont)

            | Rect(thickness, wRate) ->
                createRect(thickness, wRate, this.WindowSetting.rectColor)

            | RectWith(thickness, wRate, color) ->
                createRect(thickness, wRate, color)

            | Space h ->
                posY <- posY + h

        let itemsSize = asd.Vector2DF(currentWidth, posY)

        setting.windowSize |> function
        | WindowSetting.Fixed (v, isCenterd) ->
            currentSize <- v
            if isCenterd then
                itemParent.Position <- asd.Vector2DF(itemParent.Position.X, (v.Y - itemsSize.Y) * 0.5f )
        | _ ->
            currentSize <- itemsSize

            if isToggleOn then
                frameRect.DrawingArea <- asd.RectF(asd.Vector2DF(), currentSize)
                //frameObj.Position <- currentSize * this.WindowSetting.toggleDirection.ToVec()
                frameObj.Position <- -currentSize * Vec2.toVector2DF this.WindowSetting.centerPositionRate