namespace wraikny.MilleFeuille.Fs.UI.Button

type ButtonComponent< 'T when 'T :> asd.Object2D > =
    {
        defaultEvent : ('T -> unit)
        onEnteredEvent : ('T -> unit)
        hoverEvent : ('T -> unit)
        onPushedEvent : ('T -> unit)
    }

