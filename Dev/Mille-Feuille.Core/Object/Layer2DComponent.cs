﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wraikny.MilleFeuille.Core.Object
{
    public class Layer2DComponent<T> : asd.Layer2DComponent
        where T : asd.Layer2D
    {
        public string Name { get; }

        public Layer2DComponent(string name)
        {
            Name = name;
        }

        /// <summary>
        /// このコンポーネントを持つレイヤーがシーンに登録されたときのイベント。
        /// </summary>
        public event Action<T> OnAddedEvent = delegate { };

        /// <summary>
        /// このコンポーネントを持つレイヤーがシーンから登録解除されたときの直前のイベント。
        /// </summary>
        public event Action<T> OnRemovedEvent = delegate { };

        /// <summary>
        /// このコンポーネントを持つレイヤーが更新される直前のイベント。
        /// </summary>
        public event Action<T> OnUpdatingEvent = delegate { };

        /// <summary>
        /// このコンポーネントを持つレイヤーが更新される直後のイベント。
        /// </summary>
        public event Action<T> OnUpdatedEvent = delegate { };

        protected override void OnLayerAdded()
        {
            base.OnLayerAdded();
            OnAddedEvent((T)Owner);
        }
        protected override void OnLayerRemoved()
        {
            base.OnLayerRemoved();
            OnRemovedEvent((T)Owner);
        }

        protected override void OnUpdating()
        {
            base.OnUpdating();
            OnUpdatingEvent((T)Owner);
        }

        protected override void OnLayerUpdated()
        {
            base.OnLayerUpdated();

            OnUpdatedEvent((T)Owner);
        }
    }
}