namespace wraikny.MilleFeuille.UI

open System.Collections.Generic

open wraikny.MilleFeuille
open wraikny.MilleFeuille.UI

open FSharpPlus

type MouseInputField(font, buttonColor, focusColor, maxLength, placeholder, defaultText) =
    inherit asd.GeometryObject2D()

    let mosuePush = asd.Engine.Mouse.GetButtonInputState >> (=) asd.ButtonState.Push
    let keyPush = asd.Engine.Keyboard.GetKeyState >> (=) asd.ButtonState.Push
    let keyHold = asd.Engine.Keyboard.GetKeyState >> (=) asd.ButtonState.Hold

    let mutable isFocused = false

    let keysStack  = Stack<string>()


    let button = new MouseButton(font, buttonColor, Text = placeholder)
    do
        defaultText
        |> Option.iter(fun xs ->
            xs |> String.iter( string >> keysStack.Push)
            button.Text <- xs
        )

    let setButtonColor col =
        button.DefaultColor <- col.defaultColor
        button.HoverColor <- col.hoverColor
        button.HoldColor <- col.holdColor

    let onInputEvent = new Event<string>()

    let triggerOnInputEvent() =
        let text =
            seq {for k in keysStack -> k }
            |> Seq.rev
            |> String.concat ""
        onInputEvent.Trigger(text)

    let unfocus() =
        isFocused <- false
        setButtonColor(buttonColor)
        triggerOnInputEvent()
        if keysStack.Count = 0 then
            button.Text <- placeholder

    do
        button.Button.add_OnReleasedEvent(fun _ ->
            isFocused <- true
            setButtonColor(focusColor)
            if keysStack.Count = 0 then
                button.Text <- ""
        )
        button.Button.add_DefaultEvent(fun _ ->
            if isFocused then
                if mosuePush(asd.MouseButtons.ButtonLeft) then
                    unfocus()
        )

        base.AddDrawnChildWithoutColor(button)

    let setText() =
        let text =
            seq {for k in keysStack -> k }
            |> Seq.rev
            |> String.concat ""

        button.Text <- text

    let aToZKeys = seq [int asd.Keys.A .. int asd.Keys.Z]
    let numKeys = seq [int asd.Keys.Num0 .. int asd.Keys.Num9]

    let symbolKeys =
        [
            asd.Keys.Minus, "-"
            asd.Keys.Slash, "/"
            asd.Keys.Period, "."
            asd.Keys.Comma, ","
            asd.Keys.Backslash, "\\"
        ]
        |> Seq.map(fun (a, b) -> (int a, b))

    let inputtableKeys =
        [
            aToZKeys
            numKeys
            (symbolKeys |> Seq.map (fun (a, _) -> a))
        ]
        |> Seq.concat
        |>> enum<asd.Keys>
        |> Set.ofSeq

    let aToZKeysSet = aToZKeys |> Set.ofSeq
    let numKeysSet = numKeys |> Set.ofSeq
    let symbolKeysMap = Map.ofSeq symbolKeys

    let keysToString key =
        let intKey = int key
        if aToZKeysSet |> Set.contains intKey then
            string key
        elif numKeysSet |> Set.contains intKey then
            string ((int intKey) - 7)
        else
            symbolKeysMap |> Map.tryFind intKey |> function
            | Some(s) -> s
            | None ->
                invalidArg "key" "not found in symbolKeys"


    override this.OnUpdate() =
        if isFocused then
            if keysStack.Count < maxLength then
                for key in inputtableKeys do
                    if keyPush key then
                        let s =
                            if (keyHold asd.Keys.LeftShift || keyHold asd.Keys.RightShift) then
                                keysToString key
                            else
                                keysToString key |> String.toLower
                        keysStack.Push(s)
                        setText()
                
            if (keyPush asd.Keys.Backspace) && keysStack.Count > 0 then
                keysStack.Pop() |> ignore
                setText()

            if [asd.Keys.Escape; asd.Keys.Enter] |>> keyPush |> fold (||) false then
                unfocus()


    member __.IsFocused with get() = isFocused
    member __.Text with get() = button.Text
    member __.Button with get() = button.Button
    member __.Size with get() = button.Size and set(v) = button.Size <- v
    member __.OnInputEvent with get() = onInputEvent.Publish