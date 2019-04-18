namespace wraikny.MilleFeuille.Fs.UI.Button

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
    let init() =
        {
            defaultEvents = []
            onEnteredEvents = []
            hoverEvents = []
            onPushedEvents = []
            holdEvents = []
            onReleasedEvents = []
            onExitedEvents = []
        }

    let addDefault f builder =
        { builder with
            defaultEvents = f::builder.defaultEvents
        }

    let addOnEntered f builder =
        { builder with
            onEnteredEvents = f::builder.onEnteredEvents
        }

    let addHover f builder =
        { builder with
            hoverEvents = f::builder.hoverEvents
        }

    let addOnPushed f builder =
        { builder with
            onPushedEvents = f::builder.onPushedEvents
        }

    let addHold f builder =
        { builder with
            holdEvents = f::builder.holdEvents
        }

    let addOnReleased f builder =
        { builder with
            onReleasedEvents = f::builder.onReleasedEvents
        }

    let addOnExited f builder =
        { builder with
            onExitedEvents = f::builder.onExitedEvents
        }

    let private addEvents
        (builder : ButtonBuilder<'T>)
        (button : 'U when 'U :> ButtonComponent<'T>) =
        for f in builder.defaultEvents do
            button.add_Default(fun owner -> f owner)

        for f in builder.onEnteredEvents do
            button.add_OnEntered(fun owner -> f owner)

        for f in builder.hoverEvents do
            button.add_Hover(fun owner -> f owner)

        for f in builder.onPushedEvents do
            button.add_OnPushed(fun owner -> f owner)

        for f in builder.holdEvents do
            button.add_Hold(fun owner -> f owner)

        for f in builder.onReleasedEvents do
            button.add_OnReleased(fun owner -> f owner)

        for f in builder.onExitedEvents do
            button.add_OnExited(fun owner -> f owner)


    let buildController (builder : ButtonBuilder<'T>) =
        let button = new ControllerButtonComponent<'T>()

        button |> addEvents builder

        button


    let buildMouse (mouseButton) (builder : ButtonBuilder<'T>) =
        let button = new MouseButtonComponent<'T>(mouseButton)

        button |> addEvents builder

        button