namespace wraikny.MilleFeuille.Fs.Geometry

open wraikny.Tart.Helper.Math
open wraikny.Tart.Helper.Geometry

open wraikny.MilleFeuille.Fs.Math

module Rect =
    let inline toRectF (r : float32 Rect2) : asd.RectF =
        new asd.RectF(
            r.position |> Vec2.toVector2DF
            , r.size |> Vec2.toVector2DF
        )

    let inline toRectI (r : int Rect2) : asd.RectI =
        new asd.RectI(
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

