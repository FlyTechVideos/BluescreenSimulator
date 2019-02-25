using System;
using System.Reflection;
using System.Windows;
using BluescreenSimulator.ViewModels;

namespace BluescreenSimulator
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class BluescreenViewAttribute : Attribute
    {
        public Type WindowType { get; set; }
        public BluescreenViewAttribute(Type windowType)
        {
            WindowType = windowType;
        }
    }

    public static class BluescreenExtensions
    {
        public static void ShowView(this IBluescreenViewModel bluescreen)
        {
            var attribute = bluescreen.GetType().GetCustomAttribute<BluescreenViewAttribute>();
            if (attribute is null) throw new InvalidOperationException("No BluescreenViewAttribute has been found");
            var type = attribute.WindowType;

            void DispatcherWindowShow()
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var window = (Window) Activator.CreateInstance(type, bluescreen);
                    window.Show();
                    return window;
                });
            }
            bluescreen.ExecuteCommand.Execute((Action)DispatcherWindowShow);
        }
    }
}