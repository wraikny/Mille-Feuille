namespace wraikny.MilleFeuille.Example

module Program =
    let startScene() =
        asd.Engine.Initialize("Example Fs", 800, 600, new asd.EngineOption())
        |> ignore
        
        let scene = new Core.UI.ControllerButton.Scene()

        asd.Engine.ChangeScene(scene)
    
        let rec loop () =
            if asd.Engine.DoEvents() then
                asd.Engine.Update()
                loop ()
    
        loop()
    
        asd.Engine.Terminate()

    [<EntryPoint>]
    let main _ = 
        // Fs.Tool.Counter.main()
        startScene()
        0
