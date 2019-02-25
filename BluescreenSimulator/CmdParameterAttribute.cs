using System;

namespace BluescreenSimulator
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CmdParameterAttribute : Attribute
    {
        public string Parameter { get; set; }

        public CmdParameterAttribute(string parameter)
        {
            Parameter = parameter;
        }
    }
}