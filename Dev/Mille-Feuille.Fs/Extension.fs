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

open wraikny.Tart.Helper.Math

type asd.Vector2DF with
    member inline v.ToVec2() =
        Vec2.init(v.X, v.Y)

type asd.Vector2DI with
    member inline v.ToVec2() =
        Vec2.init(v.X, v.Y)

type asd.Vector3DF with
    member inline v.ToVec3() =
        Vec3.init(v.X, v.Y, v.Z)

type asd.Vector4DF with
    member inline v.ToVec4() =
        Vec4.init(v.X, v.Y, v.Z, v.W)

type asd.Color with
    member inline v.ToVec4() =
        Vec4.init(v.R, v.G, v.B, v.A)

open wraikny.Tart.Helper.Geometry

type asd.RectF with
    member inline r.ToRect2() =
        Rect.init (r.Position.ToVec2()) (r.Size.ToVec2())

type asd.RectI with
    member inline r.ToRect2() =
        Rect.init (r.Position.ToVec2()) (r.Size.ToVec2())