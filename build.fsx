#r "paket:
    storage: none
    source https://api.nuget.org/v3/index.json
    nuget Fake.DotNet.Cli
    nuget Fake.IO.FileSystem
    nuget Fake.Core.Target //"

#if !FAKE

#load ".fake/build.fsx/intellisense.fsx"
#r "netstandard"
#endif

open Fake.Core
open Fake.DotNet
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators

Target.create "Clean" (fun _ ->
    !! "Dev/**/bin"
    ++ "Dev/**/obj"
    |> Shell.cleanDirs 
)

Target.create "Build" (fun _ ->
    !! "Dev/**/*.*proj"
    |> Seq.iter (DotNet.build id)
)

Target.create "All" ignore

"Clean"
  ==> "Build"
  ==> "All"

Target.runOrDefault "All"
