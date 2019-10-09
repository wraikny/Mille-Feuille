[<AutoOpen>]
module wraikny.MilleFeuille.Extension

open Affogato
open System.Collections

type asd.Font with
    member inline this.HorizontalSize(text) =
        this.CalcTextureSize(text, asd.WritingDirection.Horizontal)

    member inline this.VerticalSize(text) =
        this.CalcTextureSize(text, asd.WritingDirection.Vertical)


type asd.Vector2DF with
    member inline v.ToVector2() =
        Vector2.init v.X v.Y

type asd.Vector2DI with
    member inline v.ToVector2() =
        Vector2.init v.X v.Y

type asd.Vector3DF with
    member inline v.ToVector3() =
        Vector3.init v.X v.Y v.Z

type asd.Vector4DF with
    member inline v.ToVector4() =
        Vector4.init v.X v.Y v.Z v.W

type asd.Color with
    member inline v.ToVector4() =
        Vector4.init v.R v.G v.B v.A


type asd.RectF with
    member inline r.ToRect2() =
        Rectangle.init (r.Position.ToVector2()) (r.Size.ToVector2())

type asd.RectI with
    member inline r.ToRect2() =
        Rectangle.init (r.Position.ToVector2()) (r.Size.ToVector2())


type asd.Object2D with
    member inline this.AddCoroutineAsParallel(coroutine : seq<unit>) =
        this.CoroutineManager().AddCoroutineAsParallel(coroutine.GetEnumerator())

type asd.Layer2D with
    member inline this.AddCoroutineAsParallel(coroutine : seq<unit>) =
        this.CoroutineManager().AddCoroutineAsParallel(coroutine.GetEnumerator())

type asd.Scene with
    member inline this.AddCoroutineAsParallel(coroutine : seq<unit>) =
        this.CoroutineManager().AddCoroutineAsParallel(coroutine.GetEnumerator())
