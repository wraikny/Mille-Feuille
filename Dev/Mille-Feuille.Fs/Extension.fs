[<AutoOpen>]
module wraikny.MilleFeuille.Extension

open Affogato
open Affogato.Helper
open System.Collections

type asd.Font with
    member inline this.HorizontalSize(text) =
        this.CalcTextureSize(text, asd.WritingDirection.Horizontal)

    member inline this.VerticalSize(text) =
        this.CalcTextureSize(text, asd.WritingDirection.Vertical)


type asd.Vector2DF with
    member inline v.ToVector2F() =
        Vector2.init v.X v.Y

    member inline v.ToVector2I() =
      Vector2.init (int v.X) (int v.Y)

type asd.Vector2DI with
    member inline v.ToVector2F() =
        Vector2.init (float32 v.X) (float32 v.Y)

    member inline v.ToVector2I() =
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
    member inline r.ToRectangle2() =
        Rectangle.init (r.Position.ToVector2F()) (r.Size.ToVector2F())

type asd.RectI with
    member inline r.ToRectangle2() =
        Rectangle.init (r.Position.ToVector2F()) (r.Size.ToVector2F())

type asd.Object2D with
    member inline this.AddCoroutineAsParallel(coroutine : seq<unit>) =
        this.CoroutineManager().AddCoroutineAsParallel(coroutine.GetEnumerator())

    member inline this.StackCoroutine(coroutine: seq<unit>) =
        this.CoroutineManager().StackCoroutine(coroutine.GetEnumerator())

type asd.Layer2D with
    member inline this.AddCoroutineAsParallel(coroutine : seq<unit>) =
        this.CoroutineManager().AddCoroutineAsParallel(coroutine.GetEnumerator())

    member inline this.StackCoroutine(coroutine: seq<unit>) =
        this.CoroutineManager().StackCoroutine(coroutine.GetEnumerator())

type asd.Scene with
    member inline this.AddCoroutineAsParallel(coroutine : seq<unit>) =
        this.CoroutineManager().AddCoroutineAsParallel(coroutine.GetEnumerator())

    member inline this.StackCoroutine(coroutine: seq<unit>) =
        this.CoroutineManager().StackCoroutine(coroutine.GetEnumerator())
