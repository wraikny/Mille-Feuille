module wraikny.MilleFeuille.Fs.Tool


type Event<'Msg> =
    internal
    | Msg of 'Msg
    | OpenDialog of filter : string * path : string * (string -> 'Msg)
    | SaveDialog of filter : string * path : string * (string -> 'Msg)



module rec ToolTree =
    type Item<'Msg> =
        internal
        | Separator
        | SameLine
        | Text of string
        | Column of ((float32 -> float32) * Item<'Msg> list) list
        | Selectable of string * selected : bool * 'Msg
        | Image of string * asd.Vector2DF
        | Button of string * Event<'Msg>
        | InputInt of label : string * int * (int -> 'Msg)
        | InputText of label : string * text : string * bufferSize : int * (string -> 'Msg)
        | Combo of label : string * preview : string * tool : (Item<'Msg> list)
        | ListBox of label : string * index : int * items : string list * (int -> 'Msg)


    type Menu<'Msg> =
        internal
        | Menu of label : string * AllContent<'Msg>
        | MenuItem of label : string * shortcut : asd.Keys list option * selected : bool * Event<'Msg>


    type AllContent<'Msg> =
        | Item of Item<'Msg>
        | Menu of Menu<'Msg>


open ToolTree


type Content<'Msg> =
    {
        name : string
        content : Item<'Msg>
    }


type Window<'Msg> =
    {
        menuBar : (Menu<'Msg>) list
        content : Content<'Msg>
    }


type ToolViewModel<'Msg> =
    {
        mainWindow : Window<'Msg> option
        windows : Window<'Msg> list
    }