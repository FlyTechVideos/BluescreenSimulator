using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace BluescreenSimulator
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class CmdParameterAttribute : Attribute
    {
        public string Parameter { get; private set; }
        public string FullAlias { get; set; }

        public string Description { get; set; } = "No description";

        public static string NoDash(string source) => new string(source.Where((c,i) => c != '-' || i > 2).ToArray());
        public string GetCommandLineOption(bool isOption = false) => (FullAlias != null ? $"{NoDash(Parameter)}|{NoDash(FullAlias)}" : NoDash(Parameter)) + (isOption ? "" : "=");

        public static OptionSet GetBaseOptionSet(Action<Type> onFound, out Dictionary<Type, string> types)
        {
            var optionSet = new OptionSet();
            var dic = new Dictionary<Type, string>();
            foreach (var t in Assembly.GetEntryAssembly().ExportedTypes.Where(t => IsDefined(t, typeof(CmdParameterAttribute)))
                .Select(t => new { Attribute = t.GetCustomAttribute<CmdParameterAttribute>(), Type = t }))
            {
                optionSet.Add(t.Attribute.GetCommandLineOption(true),t.Attribute.Description, s =>
                {
                    if (s != null) onFound(t.Type);
                });
                dic.Add(t.Type, t.Attribute.Parameter);
            }

            types = dic;
            return optionSet;
        }

        public static OptionSet GetOptionSetFor(Type source, object target)
        {
            var optionSet = new OptionSet();
            foreach (var property in source.GetProperties()
                .Select(p => new { p, Attribute = p.GetCustomAttribute<CmdParameterAttribute>() })
                .Where(p => p.Attribute != null))
            {
                optionSet.Add(property.Attribute.GetCommandLineOption(property.p.PropertyType == typeof(bool)), property.Attribute.Description,
               s =>
                {
                    if (TryConvertValue(s.Replace(@"\n", Environment.NewLine), property.p.PropertyType, out var r))
                    {
                        property.p.SetValue(target, r);
                    }
                });
            }

            return optionSet;
        }

        private static bool TryConvertValue(object value, Type targetType, out object result)
        {
            if (value.GetType() == targetType)
            {
                result = value;
                return true;
            }
            try
            {
                result = Convert.ChangeType(value, targetType);
            }
            catch (Exception) { }
            if (value is string s)
            {
                if (targetType == typeof(bool))
                {
                    result = true;
                    return true;
                }
                if (targetType == typeof(Color))
                {
                    if (TryGetColor(s, out var color))
                    {
                        result = color;
                        return true;
                    }
                }
            }

            result = null;
            return false; // meh.
        }

        private static bool TryGetColor(string c, out Color result)
        {
            if (!c.StartsWith("#")) c = $"#{c}";
            try
            {
                var color = ColorConverter.ConvertFromString(c) as Color?;
                if (color is null)
                {
                    result = default(Color);
                    return false;
                }

                result = color.Value;
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Something bad occured when parsing the color: {c}, \n {e}");
            }
            result = default(Color);
            return false;
        }
        public CmdParameterAttribute(string parameter) => Parameter = parameter;
    }
}