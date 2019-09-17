module wraikny.MilleFeuille.Sound


let inline createSoundSource path isDecompessed =
    try
        asd.Engine.Sound.CreateSoundSource(path, isDecompessed)
        |> Result.Ok
    with
    | e ->
        Result.Error e


let inline play source =
    try
        asd.Engine.Sound.Play source
        |> Result.Ok
    with
    | e ->
        Result.Error e