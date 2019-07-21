module wraikny.MilleFeuille.Fs.Tool


type Event<'Msg> =
    internal
    | Nothing
    | Msg of 'Msg
    | OpenDialog of filter : string * path : string * (string -> 'Msg)
    | SaveDialog of filter : string * path : string * (string -> 'Msg)



module TreeDef =
    /// ウィンドウアイテムの構造を表す型。
    type Item<'Msg> =
        internal
        | Empty
        | Separator
        | SameLine
        | Text of string
        | Selectable of string * selected : bool * 'Msg
        | Button of string * Event<'Msg>
        | Image of string * asd.Vector2DF
        | ColorEdit4 of label : string * current : asd.Color * (asd.Color -> 'Msg)
        | InputInt of label : string * int * (int -> 'Msg)
        | InputText of label : string * text : string * bufferSize : int option * (string -> 'Msg)
        | ListBox of label : string * current : int * items : string list * (int -> 'Msg)
        | Combo of label : string * current : int * items : string list * (int -> 'Msg)


    /// カラムの構造を表す型。
    type Column<'Msg> =
        internal
        | NoColumn of Item<'Msg> list
        | Column of (float32 option * Item<'Msg> list) list


    /// メニューの構造を表す型。
    type Menu<'Msg> =
        internal
        | Menu of label : string * (Menu<'Msg> list)
        | MenuItem of label : string * shortcut : asd.Keys list option * selected : bool * Event<'Msg>


open TreeDef


[<Struct>]
type Window =
    private
    | Fullscreen of offset : int
    | Windowed


type Window<'Msg> =
    private {
        name : string
        menuBar : Menu<'Msg> list option
        contents : Column<'Msg> list
    }


type ViewModel<'Msg> =
    private {
        mainWindow : (Window<'Msg> * int) option
        windows : Window<'Msg> list
    }


module Event =
    /// 何も行わないことを表す。
    let nothing = Nothing

    /// 指定のメッセージを発火することを表す。
    let message = Msg

    /// ファイルを開くダイアログを開くことを表す。
    let openDialog filter path msg = OpenDialog(filter, path, msg)

    /// ファイルを保存するダイアログを開くことを表す。
    let saveDialog filter path msg = SaveDialog(filter, path, msg)


module Tree =
    // Item

    /// 何もないことを表す。
    let empty = Empty

    /// 区切り線を表す。
    let separator = Separator

    /// 改行しないことを表す。
    let sameLine = SameLine

    /// 文字列を表す。
    let text = Text

    /// 選択可能な文字列を表す。
    let selectable text selected msg = Selectable(text, selected, msg)

    /// ボタンを表す。
    let button label event = Button(label, event)

    /// 画像を表す。
    let image path size = Image(path, size)

    /// 色選択機能を表す。
    let colorEdit4 label current msg = ColorEdit4(label, current, msg)

    /// 整数の入力欄を表す。
    let inputInt label current msg = InputInt(label, current, msg)

    /// 文字列の入力欄表す。
    let inputText label current maxbufferSize msg = InputText(label, current, maxbufferSize, msg)

    /// リストボックスを表す。
    let listBox label currentIndex items msg = ListBox(label, currentIndex, items, msg)

    /// コンボボックスを表す。
    let combo label currentIndex items msg = Combo(label, currentIndex, items, msg)

    // Column
    /// カラムを表す。
    let column = function
        | [] ->
            NoColumn []
        | (w, l)::[] ->
            NoColumn l
        | xs ->
            Column xs

    /// カラムを持たないことを表す。
    let noColumn = NoColumn

    // Menu
    /// メニューを表す。
    let menu label menus = Menu.Menu(label, menus)

    /// メニュー要素を表す。
    let menuItem label shortcut selected event = MenuItem(label, shortcut, selected, event)

    /// メインウィンドウを表す。
    let mainWindow name offset menuBar contents =
        {
            name = name
            menuBar = menuBar
            contents = contents
        }, Fullscreen offset
    
    /// ウィンドウを表す。
    let window name menuBar contents =
        {
            name = name
            menuBar = menuBar
            contents = contents
        }
    
    /// Tool機能のViewModel全体を表す。
    let viewModel mainWindow windows =
        {
            mainWindow = mainWindow
            windows = windows
        }

    let private exampleColumn : Column<unit> list = [
        noColumn[
            text "hoge"
            button "Button" <| Msg()
        ]
        column [
            Some 100.f, [
                text "hoge"
                button "Button" <| Msg()
            ]
            Some 100.f, [
                text "hoge"
                button "Button" <| Msg()
            ]
        ]
    ]

    let private exampleMenu : Menu<unit> list = [
        menu "test1" [
            menuItem "item" None false Nothing
            menuItem "item" None false Nothing
            menu "child" [
                menuItem "item" None false Nothing
                menuItem "item" None false Nothing
            ]
        ]
        menu "test2" [
            menuItem "item" None false Nothing
            menuItem "item" None false Nothing
            menu "child" [
                menuItem "item" None false Nothing
                menuItem "item" None false Nothing
            ]
        ]
    ]


let inline open' f =
    asd.Engine.OpenTool()
    f()
    asd.Engine.CloseTool()


module private Helper =
    let inline window label f =
        if asd.Engine.Tool.Begin(label) then
            f()
            asd.Engine.Tool.End()

    let inline fullscreen label offset f =
        if asd.Engine.Tool.BeginFullscreen(label, offset) then
            f()
            asd.Engine.Tool.End()

    let inline menuBar f =
        if asd.Engine.Tool.BeginMenuBar() then
            f()
            asd.Engine.Tool.EndMenuBar()

    let inline mainMenuBar f =
        if asd.Engine.Tool.BeginMainMenuBar() then
            f()
            asd.Engine.Tool.EndMainMenuBar()


    let inline menu label f =
        if asd.Engine.Tool.BeginMenu(label) then
            f()
            asd.Engine.Tool.EndMenu()


    let inline menuItem label shortcut selected f =
        if asd.Engine.Tool.MenuItem(label, shortcut, [|selected|]) then
            f()

    let inline combo label preview f =
        if asd.Engine.Tool.BeginCombo(label, preview) then
            f()
            asd.Engine.Tool.EndCombo()


open wraikny.Tart.Core
open wraikny.Tart.Helper.Utils

open FSharpPlus

module internal Render =
    let eventRender x (sender : IMsgQueue<'Msg>) =
        x |> function
        | Nothing -> ()
        | Msg msg -> sender.Enqueue(msg)
        | OpenDialog (filter, path, msg) ->
            asd.Engine.Tool.OpenDialog(filter, path)
            |> msg
            |> sender.Enqueue
        | SaveDialog (filter, path, msg) ->
            asd.Engine.Tool.SaveDialog(filter, path)
            |> msg
            |> sender.Enqueue


    let inline selectable (label, selected, msg) (sender : IMsgQueue<'Msg>) =
        if asd.Engine.Tool.Selectable(label, selected) then
            sender.Enqueue(msg)

    let itemRender x (sender : IMsgQueue<'Msg>) =
        x |> function
        | Empty -> ()
        | Separator -> asd.Engine.Tool.Separator()
        | SameLine -> asd.Engine.Tool.SameLine()
        | Text text -> asd.Engine.Tool.Text(text)

        | Selectable (label, selected, msg) ->
            selectable (label, selected, msg) sender

        | Button(label, event) ->
            if asd.Engine.Tool.Button(label) then
                eventRender event sender

        | Image(path, size) ->
            asd.Engine.Tool.Image(
                asd.Engine.Graphics.CreateTexture2D(path)
                , size
            )

        | ColorEdit4(label, current, msg) ->
            let color : float32 [] =
                [|current.R; current.G; current.B; current.A|]
                |> Array.map float32
                |> Array.map (fun x -> x / 255.0f)

            if asd.Engine.Tool.ColorEdit4(label, color) then
                let color =
                    color
                    |> Array.map (fun x -> x * 255.0f)
                    |> Array.map byte
                msg(new asd.Color(color.[0], color.[1], color.[2], color.[3]))
                |> sender.Enqueue

        | InputInt(label, current, msg) ->
            let i = [|current|]
            if asd.Engine.Tool.InputInt(label, i) then
                msg i.[0]
                |> sender.Enqueue

        | InputText(label, current, bufferSize, msg) ->
            let n = current |> String.length
            let bufferSize = bufferSize |> Option.defaultValue(n + 256)
            let s : sbyte [] =
                Array.append
                    (current |> Seq.map sbyte |> toArray)
                    [|for _ in 1..bufferSize-n -> 0y|]

            if asd.Engine.Tool.InputText(label, s, bufferSize) then
                let s =
                    s
                    |> Array.takeWhile(fun x -> x <> 0y)
                    |>> byte

                let s = System.Text.Encoding.UTF8.GetString (s, 0, s |> length)
                    
                msg(s) |> sender.Enqueue

        | ListBox(label, current, items, msg) ->
            let itemsStr =
                items
                |>> fun s -> s.Replace(";", ":")
                |> String.concat ";"

            let current = [|current|]
            if asd.Engine.Tool.ListBox(label, current, itemsStr) then
                msg current.[0]
                |> sender.Enqueue

        | Combo(label, current, items, msg) ->
            let preview =
                items
                |> tryItem current
                |> Option.defaultValue ""

            Helper.combo label preview <| fun _ ->
                for (index, item) in items |> List.indexed do
                    selectable(item, index = current, msg index) sender
            

    let columnRender (column) (sender : IMsgQueue<'Msg>) =
        column |> function
        | NoColumn list ->
            for i in list do
                 itemRender i sender
        | Column list ->
            let currentIndex = asd.Engine.Tool.ColumnIndex

            let columnSize = length list

            let render (w, il) =
                w |> function
                | None -> ()
                | Some w ->
                    asd.Engine.Tool.SetColumnWidth(currentIndex, w)

                for i in il do
                    itemRender i sender
        

            list |> function
            | [] -> ()
            | x::xs ->
                asd.Engine.Tool.Columns(columnSize)

                // render x

                for x in list do
                    render x
                    asd.Engine.Tool.NextColumn()


    let rec menuRender x (sender : IMsgQueue<'Msg>) =
        x |> function
        | Menu(label, list) ->
            Helper.menu label <| fun _ ->
                for m in list do menuRender m sender

        | MenuItem(label, shortcut, selected, event) ->
            let shortcutStr =
                shortcut
                |>> fun x ->
                    x |>> fun y -> y.ToString()
                    |> String.concat "+"
                |> Option.defaultValue("")

            shortcut |> function
            | None -> ()
            | Some keys ->
                let pushed =
                    keys
                    |>>(
                        asd.Engine.Keyboard.GetKeyState
                        >> (=) asd.ButtonState.Push
                    )
                    |> fold (||) false

                if pushed then
                    eventRender event sender

            Helper.menuItem label shortcutStr selected <| fun _ ->
                eventRender event sender


    let windowRender (x : Window<'Msg>) (window : Window) (sender : IMsgQueue<'Msg>) =
        let renderMenu() =
            x.menuBar |> function
            | None -> ()
            | Some menus ->
                for m in menus do
                    menuRender m sender
        
        let renderContent() =
            for c in x.contents do
                columnRender c sender

        window |> function
        | Fullscreen offset ->
            Helper.fullscreen x.name offset <| fun _ ->
                if x.menuBar.IsSome then
                    Helper.mainMenuBar renderMenu
                renderContent()

        | Windowed ->
            Helper.window x.name <| fun _ ->
                if x.menuBar.IsSome then
                    Helper.menuBar renderMenu
                renderContent()
    

let render (x : ViewModel<'Msg>) (sender : IMsgQueue<'Msg>) =
    x.mainWindow |> function
    | None -> ()
    | Some(mainWindow, offset) ->
        Render.windowRender mainWindow (Fullscreen offset) sender

    for w in x.windows do
        Render.windowRender w Windowed sender
