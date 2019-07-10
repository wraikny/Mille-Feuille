namespace wraikny.MilleFeuille.Fs.Math

open wraikny.Tart.Helper.Math


module Vec2 =
    let inline fromVector2DF (v : asd.Vector2DF) =
        Vec2.init(v.X, v.Y)

    let inline fromVector2DI (v : asd.Vector2DI) =
        Vec2.init(v.X, v.Y)

    let inline toVector2DF (v : float32 Vec2) =
        new asd.Vector2DF(v.x, v.y)

    let inline toVector2DI (v : int Vec2) =
        new asd.Vector2DI(v.x, v.y)

module Vec3 =
    let inline fromVector3DF (v : asd.Vector3DF) =
        Vec3.init(v.X, v.Y, v.Z)

    let inline fromColor (v : asd.Color) =
        Vec3.init(v.R, v.G, v.B)

    let inline toVector3DF (v : float32 Vec3) =
        new asd.Vector3DF(v.x, v.y, v.z)

    let inline toColor (v : byte Vec3) =
        new asd.Color(v.x, v.y, v.z)

module Vec4 =
    let inline fromVector4DF (v : asd.Vector4DF) =
        Vec4.init(v.X, v.Y, v.Z, v.W)

    let inline fromColor (v : asd.Color) =
        Vec4.init(v.R, v.G, v.B, v.A)

    let inline toVector4DF (v : float32 Vec4) =
        new asd.Vector4DF(v.x, v.y, v.z, v.w)

    let inline toColor (v : byte Vec4) =
        new asd.Color(v.x, v.y, v.z, v.w)