namespace wraikny.MilleFeuille.Fs.Animation

open wraikny.MilleFeuille.Core

/// アニメーションコントローラで保持するノードを作成するビルダー。
type NodeBuilder<'Obj, 'State> =
    {
        animation : AnimationBuilder<'Obj>
        next : 'State option
    }


module NodeBuilder =
    /// ノードクラスを作成するビルダーを作る。
    let build builder =
        let anim =
            builder.animation
            |> AnimationBuilder.build

        let node = new Animation.Node<'Obj, 'State>(anim)
        builder.next |> function
        | None -> ()
        | Some next -> node.NextState <- next

        node



/// アニメーションコントローラクラスを作成するビルダー。
type AnimationControllerBuilder<'Obj, 'State when 'State : comparison> =
    {
        name : string
        nodes: Map<'State, NodeBuilder<'Obj, 'State>>
    }


module AnimationControllerBuilder =
    /// アニメーションコントローラを作成するビルダーを作る。
    let init name nodes = { name = name; nodes = nodes |> Map.ofList }

    /// コントローラにアニメーションノードを追加する。
    let addNode state node builder =
        { builder with
            nodes = builder.nodes |> Map.add state node
        }

    /// ビルダーからアニメーションコントローラクラスを作成する。
    let build builder =
        let controller = new Animation.AnimationController<'Obj, 'State>(builder.name)

        let nodes =
            builder.nodes
            |> Map.toSeq
            |> Seq.map(fun (s, n) -> struct (s, NodeBuilder.build n))

        controller.AddAnimations(nodes)

        controller

    /// ビルダーからアニメーションコンポーネントクラスを作成する。
    let buildComponent name builder =
        let controller =
            builder
            |> build

        let component' = new Animation.AnimatorComponent<_, _>(name, controller)

        component'