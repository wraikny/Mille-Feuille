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