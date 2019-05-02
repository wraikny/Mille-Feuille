using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using wraikny.MilleFeuille.Core.Input;

namespace wraikny.MilleFeuille.Core.Input.Config
{
    public interface IJoystickInputConfig
    {
    }

    /// <summary>
    /// ジョイスティックのボタンの対応関係を保存するためのクラス。
    /// </summary>
    [Serializable]
    public class ButtonInputConfig : IJoystickInputConfig
    {
        public int Index { get; }

        public ButtonInputConfig(int index)
        {
            Index = index;
        }
    }

    /// <summary>
    /// ジョイスティックのスティックの対応関係を保存するためのクラス。
    /// </summary>
    [Serializable]
    public class AxisInputConfig : IJoystickInputConfig
    {
        public int Index { get; }
        public Controller.AxisDirection Direction { get; }

        public AxisInputConfig(int index, Controller.AxisDirection direction)
        {
            Index = index;
            Direction = direction;
        }
    }

    /// <summary>
    /// ジョイスティックの入力と操作の対応関係を保存するためのクラス。
    /// </summary>
    /// <typeparam name="TControl"></typeparam>
    /// <typeparam name="TTiltControl"></typeparam>
    [Serializable]
    public class Joystick<TControl, TTiltControl>
    {
        /// <summary>
        /// ジョイスティックの名前を取得する。
        /// </summary>
        public string Name { get; }
        private readonly Dictionary<TControl, IJoystickInputConfig> binding;
        private readonly Dictionary<TTiltControl, int> axisTiltBinding;

        public Joystick(string name)
        {
            Name = name;
            binding = new Dictionary<TControl, IJoystickInputConfig>();
            axisTiltBinding = new Dictionary<TTiltControl, int>();
        }

        /// <summary>
        /// ボタン入力に操作を対応付ける。
        /// </summary>
        /// <param name="abstractKey"></param>
        /// <param name="buttonIndex"></param>
        public void BindButton(TControl abstractKey, int buttonIndex)
        {
            binding[abstractKey] = new ButtonInputConfig(buttonIndex);
        }

        /// <summary>
        /// スティック入力に操作を対応付ける。
        /// </summary>
        /// <param name="abstractKey"></param>
        /// <param name="axisIndex"></param>
        /// <param name="direction"></param>
        public void BindAxis(TControl abstractKey, int axisIndex, Controller.AxisDirection direction)
        {
            binding[abstractKey] = new AxisInputConfig(axisIndex, direction);
        }

        /// <summary>
        /// スティックのインデックスに操作を対応付ける。
        /// </summary>
        /// <param name="abstractKey"></param>
        /// <param name="index"></param>
        public void BindAxisTilt(TTiltControl abstractKey, int index)
        {
            axisTiltBinding[abstractKey] = index;
        }

        /// <summary>
        /// 対応関係から実行するためのコントローラーを作成する。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Controller.JoystickController<TControl, TTiltControl> CreateController(int index)
        {
            var controller = new Controller.JoystickController<TControl, TTiltControl>(index);

            foreach(var item in binding)
            {
                if (item.Value is ButtonInputConfig button)
                {
                    controller.BindButton(item.Key, button.Index);
                }
                else if (item.Value is AxisInputConfig axis)
                {
                    controller.BindAxis(item.Key, axis.Index, axis.Direction);
                }
            }

            foreach(var item in axisTiltBinding)
            {
                controller.BindAxisTilt(item.Key, item.Value);
            }

            return controller;
        }

        /// <summary>
        /// バイナリとして保存する。
        /// </summary>
        /// <param name="path"></param>
        public void SaveToBinaryFile(string path)
        {
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                var bf = new BinaryFormatter();
                bf.Serialize(fs, this);
            }
        }

        /// <summary>
        /// バイナリから読み込む。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Joystick<T, U> LoadFromBinaryFile<T, U>(string path)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var bf = new BinaryFormatter();
                var obj = (Joystick<T, U>)bf.Deserialize(fs);
                return obj;
            }
        }
    }
}
