module wraikny.MilleFeuille.Fs.Sound


let inline createSoundSource path isDecompessed =
    try
        asd.Engine.Sound.CreateSoundSource(path, isDecompessed)
        |> Result.Ok
    with
    | e ->
        Result.Error <| e.ToString()


let inline play source =
    try
        asd.Engine.Sound.Play source
        |> Result.Ok
    with
    | e ->
        e.ToString() |> Result.Error