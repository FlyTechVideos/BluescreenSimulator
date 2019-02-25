using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace BluescreenSimulator
{
    public class ViewModelBase<T> : PropertyChangedObject where T : new()
    {
        protected T Model { get; set; } = new T();
        private readonly Dictionary<string, PropertyInfo> ModelPropertiesNames = new Dictionary<string, PropertyInfo>();
        public ViewModelBase(T model = default)
        {
            if (!model?.Equals(default(T)) ?? false)
            {
                Model = model;
            }
        }

        protected void SetModelProperty(object value,[CallerMemberName] string name = null, params string[] others)
        {
            if (!ModelPropertiesNames.ContainsKey(name))
            {
                var property = typeof(T).GetProperty(name);
                ModelPropertiesNames.Add(name, property);
            }
            ModelPropertiesNames[name].SetValue(Model, value);
            OnPropertyChanged(name);
            foreach (var otherName in others)
            {
                OnPropertyChanged(otherName);
            }
        }
    }
}