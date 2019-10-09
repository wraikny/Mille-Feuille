namespace wraikny.MilleFeuille

open Affogato


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Vector2 =
    let inline fromVector2DF (v : asd.Vector2DF) =
        v.ToVector2()

    let inline fromVector2DI (v : asd.Vector2DI) =
        v.ToVector2()

    let inline toVector2DF (v : float32 Vector2) =
        asd.Vector2DF(v.x, v.y)

    let inline toVector2DI (v : int Vector2) =
        asd.Vector2DI(v.x, v.y)

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Vector3 =
    let inline fromVector3DF (v : asd.Vector3DF) =
        v.ToVector3()

    let inline toVector3DF (v : float32 Vector3) =
        asd.Vector3DF(v.x, v.y, v.z)

    let inline toColor (v : byte Vector3) =
        asd.Color(v.x, v.y, v.z)

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Vector4 =
    let inline fromVector4DF (v : asd.Vector4DF) =
        v.ToVector4()

    let inline fromColor (c : asd.Color) =
        c.ToVector4()

    let inline toVector4DF (v : float32 Vector4) =
        asd.Vector4DF(v.x, v.y, v.z, v.w)

    let inline toColor (v : byte Vector4) =
        asd.Color(v.x, v.y, v.z, v.w)

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Rectangle =
    let inline toRectF (r : float32 Rectangle2) : asd.RectF =
        asd.RectF(
            r.position |> Vector2.toVector2DF
            , r.size |> Vector2.toVector2DF
        )

    let inline toRectI (r : int Rectangle2) : asd.RectI =
        asd.RectI(
            r.position |> Vector2.toVector2DI
            , r.size |> Vector2.toVector2DI
        )

    let inline toRectangleShape (r : float32 Rectangle2) : asd.RectangleShape =
        new asd.RectangleShape(
            DrawingArea = toRectF r
        )

    let inline toRectangleCollider (r : float32 Rectangle2) : asd.RectangleCollider =
        new asd.RectangleCollider(
            Area = toRectF r
        )

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Sphere =
    let inline toCircleShape (c : float32 Sphere2) : asd.CircleShape =
        new asd.CircleShape(
            Position = Vector2.toVector2DF c.center
            , InnerDiameter = 0.0f
            , OuterDiameter = c.radius * 2.0f
        )

    let inline toCircleCollider (c : float32 Sphere2) : asd.CircleCollider =
        new asd.CircleCollider(
            Center = Vector2.toVector2DF c.center
            , Radius = c.radius
        )

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Line =
    let inline toLineShape thickness (l : float32 Line2) : asd.LineShape =
        new asd.LineShape(
            Thickness = thickness
            , StartingPosition = Vector2.toVector2DF l.startPoint
            , EndingPosition = Vector2.toVector2DF l.endPoint
        )

    let inline toLineCollider thickness (l : float32 Line2) : asd.LineCollider =
        new asd.LineCollider(
            Thickness = thickness
            , StartingPosition = Vector2.toVector2DF l.startPoint
            , EndingPosition = Vector2.toVector2DF l.endPoint
        )

