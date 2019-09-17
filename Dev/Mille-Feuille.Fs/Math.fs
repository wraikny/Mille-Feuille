namespace wraikny.MilleFeuille

open wraikny.Tart.Helper.Math


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Vec2 =
    let inline fromVector2DF (v : asd.Vector2DF) =
        v.ToVec2()

    let inline fromVector2DI (v : asd.Vector2DI) =
        v.ToVec2()

    let inline toVector2DF (v : float32 Vec2) =
        asd.Vector2DF(v.x, v.y)

    let inline toVector2DI (v : int Vec2) =
        asd.Vector2DI(v.x, v.y)

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Vec3 =
    let inline fromVector3DF (v : asd.Vector3DF) =
        v.ToVec3()

    let inline toVector3DF (v : float32 Vec3) =
        asd.Vector3DF(v.x, v.y, v.z)

    let inline toColor (v : byte Vec3) =
        asd.Color(v.x, v.y, v.z)

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Vec4 =
    let inline fromVector4DF (v : asd.Vector4DF) =
        v.ToVec4()

    let inline fromColor (c : asd.Color) =
        c.ToVec4()

    let inline toVector4DF (v : float32 Vec4) =
        asd.Vector4DF(v.x, v.y, v.z, v.w)

    let inline toColor (v : byte Vec4) =
        asd.Color(v.x, v.y, v.z, v.w)

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Rect =
    let inline toRectF (r : float32 Rect2) : asd.RectF =
        asd.RectF(
            r.position |> Vec2.toVector2DF
            , r.size |> Vec2.toVector2DF
        )

    let inline toRectI (r : int Rect2) : asd.RectI =
        asd.RectI(
            r.position |> Vec2.toVector2DI
            , r.size |> Vec2.toVector2DI
        )

    let inline toRectangleShape (r : float32 Rect2) : asd.RectangleShape =
        new asd.RectangleShape(
            DrawingArea = toRectF r
        )

    let inline toRectangleCollider (r : float32 Rect2) : asd.RectangleCollider =
        new asd.RectangleCollider(
            Area = toRectF r
        )

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Sphere =
    let inline toCircleShape (c : float32 Sphere2) : asd.CircleShape =
        new asd.CircleShape(
            Position = Vec2.toVector2DF c.center
            , InnerDiameter = 0.0f
            , OuterDiameter = c.radius * 2.0f
        )

    let inline toCircleCollider (c : float32 Sphere2) : asd.CircleCollider =
        new asd.CircleCollider(
            Center = Vec2.toVector2DF c.center
            , Radius = c.radius
        )

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Line =
    let inline toLineShape thickness (l : float32 Line2) : asd.LineShape =
        new asd.LineShape(
            Thickness = thickness
            , StartingPosition = Vec2.toVector2DF l.startPoint
            , EndingPosition = Vec2.toVector2DF l.endPoint
        )

    let inline toLineCollider thickness (l : float32 Line2) : asd.LineCollider =
        new asd.LineCollider(
            Thickness = thickness
            , StartingPosition = Vec2.toVector2DF l.startPoint
            , EndingPosition = Vec2.toVector2DF l.endPoint
        )

