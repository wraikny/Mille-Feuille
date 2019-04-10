module wraikny.MilleFeuille.Fs.Tool


type Event<'Msg> =
    internal
    | Nothing
    | Msg of 'Msg
    | OpenDialog of filter : string * path : string * (string -> 'Msg)
    | SaveDialog of filter : string * path : string * (string -> 'Msg)



module rec TreeRec =
    type Item<'Msg> =
        internal
        | Empty
        | Separator
        | SameLine
        | Text of string
        | Selectable of string * selected : bool * 'Msg
        | Image of string * asd.Vector2DF
        | Button of string * Event<'Msg>
        | InputInt of label : string * int * (int -> 'Msg)
        | InputText of label : string * text : string * bufferSize : int * (string -> 'Msg)
        | Combo of label : string * preview : string * (Column<'Msg> list)
        | ListBox of label : string * index : int * items : string list * (int -> 'Msg)

    type Column<'Msg> = internal | Column of (int * Item<'Msg> list) list


    type Menu<'Msg> =
        internal
        | Menu of label : string * (Column<'Msg> list)
        | MenuItem of label : string * shortcut : asd.Keys list option * selected : bool * Event<'Msg>


open TreeRec


module Tree =
    // Event
    let nothing = Nothing

    let msg = Msg

    let openDialog filter path msg = OpenDialog(filter, path, msg)

    let saveDialog filter path msg = SaveDialog(filter, path, msg)

    // Item
    let empty = Empty

    let separator = Separator

    let sameLine = SameLine

    let text = Text

    let selectable text selected msg = Selectable(text, selected, msg)

    let image path size = Image(path, size)

    let button label event = Button(label, event)

    let inputInt label current msg = InputInt(label, current, msg)

    let inputText label current maxbufferSize msg = InputText(label, current, maxbufferSize, msg)

    let combo label preview children = Combo(label, preview, children)

    let listBox label currentIndex items msg = ListBox(label, currentIndex, items, msg)

    // Column
    let column = Column

    // Menu
    let menu label content = Menu.Menu(label, content)

    let menuItem label shortCut selected event = MenuItem(label, shortCut, selected, event)


    let private example : Column<unit> list = [
        column [
            1, [
                text "hoge"
                button "Button" <| Msg()
            ]
        ]
        column [
            1, [
                text "hoge"
                button "Button" <| Msg()
            ]
            1, [
                text "hoge"
                button "Button" <| Msg()
            ]
        ]
    ]



type Window<'Msg> =
    private {
        name : string
        menuBar : Menu<'Msg> list
        content : Column<'Msg> list
    }


type ViewModel<'Msg> =
    private {
        mainWindow : Window<'Msg> option
        windows : Window<'Msg> list
    }



module Render =
    let event x sender =
        x |> function
        | Nothing -> ()

    
    
let useTool f =
    asd.Engine.OpenTool()
    f()
    asd.Engine.CloseTool()
