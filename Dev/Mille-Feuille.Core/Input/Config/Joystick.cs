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

    [Serializable]
    public class ButtonInputConfig : IJoystickInputConfig
    {
        public int Index { get; }

        public ButtonInputConfig(int index)
        {
            Index = index;
        }
    }

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

    [Serializable]
    public class Joystick<TControl>
    {
        public string Name { get; }
        private readonly Dictionary<TControl, IJoystickInputConfig> binding;

        public Joystick(string name)
        {
            Name = name;
            binding = new Dictionary<TControl, IJoystickInputConfig>();
        }

        public void BindButton(TControl abstractKey, int buttonIndex)
        {
            binding[abstractKey] = new ButtonInputConfig(buttonIndex);
        }

        public void BindAxis(TControl abstractKey, int axisIndex, Controller.AxisDirection direction)
        {
            binding[abstractKey] = new AxisInputConfig(axisIndex, direction);
        }

        public Controller.JoystickController<TControl> CreateController(int index)
        {
            var controller = new Controller.JoystickController<TControl>(index);

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

            return controller;
        }

        public void SaveToBinaryFile(string path)
        {
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                var bf = new BinaryFormatter();
                bf.Serialize(fs, this);
            }
        }

        public static Joystick<T> LoadFromBinaryFile<T>(string path)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var bf = new BinaryFormatter();
                var obj = (Joystick<T>)bf.Deserialize(fs);
                return obj;
            }
        }
    }
}
