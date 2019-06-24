namespace wraikny.MilleFeuille.Fs.UI.Button


/// ボタンクラスを作成するビルダー。
[<Struct>]
type ButtonBuilder< 'T when 'T :> asd.Object2D > =
    {
        defaultEvents : ('T -> unit) list
        onEnteredEvents : ('T -> unit) list
        hoverEvents : ('T -> unit) list
        onPushedEvents : ('T -> unit) list
        holdEvents : ('T -> unit) list
        onReleasedEvents : ('T -> unit) list
        onExitedEvents : ('T -> unit) list
    }


open wraikny.MilleFeuille.Core.UI.Button


module ButtonBuilder =
    /// ボタンクラスを作成するビルダーを作る。
    let inline init() =
        {
            defaultEvents = []
            onEnteredEvents = []
            hoverEvents = []
            onPushedEvents = []
            holdEvents = []
            onReleasedEvents = []
            onExitedEvents = []
        }


    /// デフォルト時に毎フレーム呼び出されるイベントを追加する。
    let inline addDefaultEvent f builder =
        { builder with
            defaultEvents = f::builder.defaultEvents
        }

    /// フォーカスが入った時に呼び出されるイベントを追加する。
    let inline addOnEnteredEvent f builder =
        { builder with
            onEnteredEvents = f::builder.onEnteredEvents
        }

    /// ホバー時に毎フレーム呼び出されるイベントを追加する。
    let inline addHoverEvent f builder =
        { builder with
            hoverEvents = f::builder.hoverEvents
        }

    /// 選択ボタンが押された時に呼び出されるイベントを追加する。
    let inline addOnPushedEvent f builder =
        { builder with
            onPushedEvents = f::builder.onPushedEvents
        }

    /// ホールド時に毎フレーム呼び出されるイベントを追加する。
    let inline addHoldEvent f builder =
        { builder with
            holdEvents = f::builder.holdEvents
        }

    /// 選択ボタンが離された時に呼び出されるイベントを追加する。
    let inline addOnReleasedEvent f builder =
        { builder with
            onReleasedEvents = f::builder.onReleasedEvents
        }

    /// フォーカスが外れた時に呼び出されるイベントを追加する。
    let inline addOnExitedEvent f builder =
        { builder with
            onExitedEvents = f::builder.onExitedEvents
        }

    let private addEvents
        (builder : ButtonBuilder<'T>)
        (button : 'U when 'U :> ButtonComponent<'T>) =
        for f in builder.defaultEvents do
            button.add_DefaultEvent(fun owner -> f owner)

        for f in builder.onEnteredEvents do
            button.add_OnEnteredEvent(fun owner -> f owner)

        for f in builder.hoverEvents do
            button.add_HoverEvent(fun owner -> f owner)

        for f in builder.onPushedEvents do
            button.add_OnPushedEvent(fun owner -> f owner)

        for f in builder.holdEvents do
            button.add_HoldEvent(fun owner -> f owner)

        for f in builder.onReleasedEvents do
            button.add_OnReleasedEvent(fun owner -> f owner)

        for f in builder.onExitedEvents do
            button.add_OnExitedEvent(fun owner -> f owner)


    /// ボタンの情報をもとに、コントローラ操作可能なボタンを作成する。
    let buildController (name) (builder : ButtonBuilder<'T>) =
        let button = new ControllerButtonComponent<'T>(name)

        button |> addEvents builder

        button

    /// ボタンの情報をもとに、マウス操作可能なボタンを作成する。
    let buildMouse (name) (mouseButton) (builder : ButtonBuilder<'T>) =
        let button = new MouseButtonComponent<'T>(name, mouseButton)

        button |> addEvents builder

        button