﻿namespace wraikny.MilleFeuille.Fs.Animation

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

        node :> Animation.INode<'State>



/// アニメーションコントローラクラスを作成するビルダー。
type AnimationControllerBuilder<'Owner, 'State
    when 'State : comparison
    and  'State : not struct
    > =
    {
        name : string
        nodes: Map<'State, Animation.INode<'State>>
    }


module AnimationControllerBuilder =
    /// アニメーションコントローラを作成するビルダーを作る。
    let init name = { name = name; nodes = Map.empty }

    /// コントローラにアニメーションノードを追加する。
    let addNode (state, node) builder =
        { builder with
            nodes =
                builder.nodes
                |> Map.add state (NodeBuilder.build node)
        }

    /// ビルダーからアニメーションコントローラクラスを作成する。
    let build builder =
        let controller =
            new Animation.AnimationController<'State>(builder.name)

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
            new Animation.AnimatorComponent<'Owner, 'State>(name, controller)

        component'