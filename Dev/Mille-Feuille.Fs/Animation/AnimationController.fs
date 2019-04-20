namespace wraikny.MilleFeuille.Fs.Animation

open wraikny.MilleFeuille.Core

type NodeBuilder<'Obj, 'State> =
    {
        animation : AnimationBuilder<'Obj>
        next : 'State
    }


module NodeBuilder =
    let build builder =
        let anim =
            builder.animation
            |> AnimationBuilder.build

        let node = new Animation.Node<'Obj, 'State>(anim, builder.next)

        node


type AnimationControllerBuilder<'Obj, 'State when 'State : comparison> =
    {
        nodes: Map<'State, NodeBuilder<'Obj, 'State>>
    }


module AnimationControllerBuilder =
    let init nodes = { nodes = nodes }


    let addNode state node builder =
        { builder with
            nodes = builder.nodes |> Map.add state node
        }


    let build name builder =
        let controller = new Animation.AnimationController<'Obj, 'State>(name)

        let nodes =
            builder.nodes
            |> Map.toSeq
            |> Seq.map(fun (s, n) -> struct (s, NodeBuilder.build n))

        controller.AddAnimations(nodes)

        controller


    let buildComponent name builder =
        let controller =
            builder
            |> build name

        let component' = new Animation.AnimatorComponent<_, _>(name, controller)

        component'