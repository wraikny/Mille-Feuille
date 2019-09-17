namespace wraikny.MilleFeuille.UI

open wraikny.MilleFeuille
open wraikny.MilleFeuille.UI
open wraikny.MilleFeuille.UI

type MouseButton(font, color : ButtonColor) as this =
    inherit asd.GeometryObject2D()

    let textObj = new asd.TextObject2D(Font = font)

    let collider = new asd.RectangleCollider()

    let frameRect = new asd.RectangleShape()
    let frame = new asd.GeometryObject2D(Shape = frameRect)

    let mutable defaultColor = color.defaultColor
    let mutable hoverColor = color.hoverColor
    let mutable holdColor = color .holdColor

    let btn =
        ButtonBuilder.init()
        |> ButtonBuilder.addOnChangedToStateEvents
            (fun _ -> frame.Color <- defaultColor)
            (fun _ -> frame.Color <- hoverColor)
            (fun _ -> frame.Color <- holdColor)
        |> ButtonBuilder.buildMouse "Button" asd.MouseButtons.ButtonLeft

    do
        btn.Attach(this)

    member __.TextSize
        with get() = font.HorizontalSize(textObj.Text)

    member __.Size
        with get() = frameRect.DrawingArea.Size
        and  set(value) =
            frameRect.DrawingArea <- asd.RectF(asd.Vector2DF(), value)
            collider.Area <-
                let n = 1.4f in
                asd.RectF(-value * (n - 1.0f) / 2.0f, value * n)
            textObj.Position <- value / 2.0f



    member this.AutoSize(margin : asd.Vector2DF) =
        let size = font.HorizontalSize(textObj.Text).To2DF() + margin * 2.0f
        this.Size <- size


    member __.DefaultColor
        with set(value) =
            defaultColor <- value
            if btn.State = ButtonState.Default then
                frame.Color <- value

    member __.HoverColor
        with set(value) =
            hoverColor <- value
            if btn.State = ButtonState.Hover then
                frame.Color <- value

    member __.HoldColor
        with set(value) =
           holdColor <- value
           if btn.State = ButtonState.Hold then
               frame.Color <- value


    override this.OnAdded() =
        this.AddCollider(collider)

        this.AddDrawnChildWithoutColor(frame)
        this.AddDrawnChildWithoutColor(textObj)

        frame.Color <- defaultColor

        
    member __.Text
        with get() = textObj.Text
        and  set(text) =
            let textSize = font.HorizontalSize(text).To2DF()
            textObj.CenterPosition <- textSize / 2.0f
            textObj.Text <- text


    member __.ColliderIsVisible
        with get() = collider.IsVisible
        and  set(value) = collider.IsVisible <- value
    
    member __.Button with get() = btn

