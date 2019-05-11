namespace wraikny.MilleFeuille.Fs.Math

open wraikny.Tart.Helper.Math


module Vec2 =
    let toVector2DF (v : float32 Vec2) =
        new asd.Vector2DF(v.x, v.y)

    let toVector2DI (v : int Vec2) =
        new asd.Vector2DI(v.x, v.y)

module Vec3 =
    let toVector3DF (v : float32 Vec3) =
        new asd.Vector3DF(v.x, v.y, v.z)

    let toColor (v : byte Vec3) =
        new asd.Color(v.x, v.y, v.z)

module Vec4 =
    let toVector4DF (v : float32 Vec4) =
        new asd.Vector4DF(v.x, v.y, v.z, v.w)

    let toColor (v : byte Vec4) =
        new asd.Color(v.x, v.y, v.z, v.w)