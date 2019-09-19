[<AutoOpen>]
module wraikny.MilleFeuille.Extension

open wraikny.Tart.Math


type asd.Font with
    member inline this.HorizontalSize(text) =
        this.CalcTextureSize(text, asd.WritingDirection.Horizontal)

    member inline this.VerticalSize(text) =
        this.CalcTextureSize(text, asd.WritingDirection.Vertical)


type asd.Vector2DF with
    member inline v.ToVec2() =
        Vec2.init v.X v.Y

type asd.Vector2DI with
    member inline v.ToVec2() =
        Vec2.init v.X v.Y

type asd.Vector3DF with
    member inline v.ToVec3() =
        Vec3.init v.X v.Y v.Z

type asd.Vector4DF with
    member inline v.ToVec4() =
        Vec4.init v.X v.Y v.Z v.W

type asd.Color with
    member inline v.ToVec4() =
        Vec4.init v.R v.G v.B v.A


type asd.RectF with
    member inline r.ToRect2() =
        Rect.init (r.Position.ToVec2()) (r.Size.ToVec2())

type asd.RectI with
    member inline r.ToRect2() =
        Rect.init (r.Position.ToVec2()) (r.Size.ToVec2())


type asd.Object2D with
    member inline this.AddCoroutine(coroutine : seq<unit>) =
        this.AddCoroutine(coroutine.GetEnumerator())

type asd.Layer2D with
    member inline this.AddCoroutine(coroutine : seq<unit>) =
        this.AddCoroutine(coroutine.GetEnumerator())

type asd.Scene with
    member inline this.AddCoroutine(coroutine : seq<unit>) =
        this.AddCoroutine(coroutine.GetEnumerator())