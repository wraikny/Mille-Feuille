namespace wraikny.MilleFeuille

open wraikny.MilleFeuille


/// アニメーションコントローラで保持するノードを作成するビルダー。
[<Struct>]
type NodeBuilder<'Obj, 'State> =
    {
        animation : AnimationBuilder<'Obj>
        next : 'State option
    }

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module NodeBuilder =
    /// ビルダーからノードクラスを作成する。
    let build builder =
        let anim =
            builder.animation
            |> AnimationBuilder.build

        let node = new AnimationNode<'Obj, 'State>(anim)
        builder.next |> function
        | None -> ()
        | Some next -> node.NextState <- next

        node :> IAnimationNode<'State>



/// アニメーションコントローラクラスを作成するビルダー。
[<Struct>]
type AnimationControllerBuilder<'Owner, 'State
    when 'State : comparison
    and  'State : not struct
    > =
    {
        name : string
        nodes: Map<'State, IAnimationNode<'State>>
    }

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module AnimationControllerBuilder =
    /// アニメーションコントローラを作成するビルダーを作る。
    let inline init name = { name = name; nodes = Map.empty }

    /// コントローラにアニメーションノードを追加する。
    let inline addNode (state, node) builder =
        { builder with
            nodes =
                builder.nodes
                |> Map.add state node
        }


    /// ビルダーを元にコントローラにアニメーションノードを追加する。
    let inline addNodeBuilder (state, node) builder =
        builder
        |> addNode (state, NodeBuilder.build node)


    let rec addNodesList list builder =
        list |> function
        | [] -> builder
        | x::xs ->
            builder
            |> addNode x
            |> addNodesList xs


    let rec addNodeBuildersList list builder =
        list |> function
         [] -> builder
         | x::xs ->
            builder
            |> addNodeBuilder x
            |> addNodeBuildersList xs


    /// ビルダーからアニメーションコントローラクラスを作成する。
    let build builder =
        let controller = AnimationController<'State>(builder.name)

        let nodes =
            builder.nodes
            |> Map.toSeq
            |> Seq.map(fun (s, n) ->
                struct (s, n)
            )

        controller.AddAnimations(nodes)

        controller

    /// ビルダーからアニメーションコンポーネントクラスを作成する。
    let buildComponent name (builder : AnimationControllerBuilder<'Owner, 'State>) =
        let controller =
            builder
            |> build

        let component' =
            new AnimatorComponent<'Owner, 'State>(name, controller)

        component'