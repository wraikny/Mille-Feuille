namespace wraikny.MilleFeuille.Fs.Geometry

open wraikny.Tart.Helper.Math
open wraikny.Tart.Helper.Geometry

open wraikny.MilleFeuille.Fs.Math

module Rect =
    let toRectF (r : float32 Vec2 Rect) : asd.RectF =
        new asd.RectF(
            r.position |> Vec2.toVector2DF
            , r.size |> Vec2.toVector2DF
        )

    let toRectI (r : int Vec2 Rect) : asd.RectI =
        new asd.RectI(
            r.position |> Vec2.toVector2DI
            , r.size |> Vec2.toVector2DI
        )

    let toRectangleShape (r : float32 Vec2 Rect) : asd.RectangleShape =
        new asd.RectangleShape(
            DrawingArea = toRectF r
        )

    let toRectangleCollider (r : float32 Vec2 Rect) : asd.RectangleCollider =
        new asd.RectangleCollider(
            Area = toRectF r
        )

module Sphere =
    let toCircleShape (c : Sphere<float32, float32 Vec2>) : asd.CircleShape =
        new asd.CircleShape(
            Position = Vec2.toVector2DF c.center
            , InnerDiameter = 0.0f
            , OuterDiameter = c.radius * 2.0f
        )

    let toCircleCollider (c : Sphere<float32, float32 Vec2>) : asd.CircleCollider =
        new asd.CircleCollider(
            Center = Vec2.toVector2DF c.center
            , Radius = c.radius
        )

module Line =
    let toLineShape thickness (l : float32 Vec2 Line) : asd.LineShape =
        new asd.LineShape(
            Thickness = thickness
            , StartingPosition = Vec2.toVector2DF l.startPoint
            , EndingPosition = Vec2.toVector2DF l.endPoint
        )

    let toLineCollider thickness (l : float32 Vec2 Line) : asd.LineCollider =
        new asd.LineCollider(
            Thickness = thickness
            , StartingPosition = Vec2.toVector2DF l.startPoint
            , EndingPosition = Vec2.toVector2DF l.endPoint
        )

