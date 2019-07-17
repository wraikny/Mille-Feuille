module wraikny.MilleFeuille.Fs.Extension

type asd.DrawnObject2D with
    member inline this.AddDrawnChildWithAll(o) =
        this.AddDrawnChild(
            o,
            asd.ChildManagementMode.RegistrationToLayer |||
            asd.ChildManagementMode.Disposal |||
            asd.ChildManagementMode.IsUpdated |||
            asd.ChildManagementMode.IsDrawn,

            asd.ChildTransformingMode.All,

            asd.ChildDrawingMode.Color |||
            asd.ChildDrawingMode.DrawingPriority
        )

    member inline this.AddDrawnChildWithoutColor(o) =
        this.AddDrawnChild(
            o,
            asd.ChildManagementMode.RegistrationToLayer |||
            asd.ChildManagementMode.Disposal |||
            asd.ChildManagementMode.IsUpdated |||
            asd.ChildManagementMode.IsDrawn,

            asd.ChildTransformingMode.All,

            asd.ChildDrawingMode.DrawingPriority
        )