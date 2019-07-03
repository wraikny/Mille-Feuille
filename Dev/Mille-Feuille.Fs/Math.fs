namespace wraikny.MilleFeuille.Fs.Math

open wraikny.Tart.Helper.Math


module Vec2 =
    let inline toVector2DF (v : float32 Vec2) =
        new asd.Vector2DF(v.x, v.y)

    let inline toVector2DI (v : int Vec2) =
        new asd.Vector2DI(v.x, v.y)

module Vec3 =
    let inline toVector3DF (v : float32 Vec3) =
        new asd.Vector3DF(v.x, v.y, v.z)

    let inline toColor (v : byte Vec3) =
        new asd.Color(v.x, v.y, v.z)

module Vec4 =
    let inline toVector4DF (v : float32 Vec4) =
        new asd.Vector4DF(v.x, v.y, v.z, v.w)

    let inline toColor (v : byte Vec4) =
        new asd.Color(v.x, v.y, v.z, v.w)