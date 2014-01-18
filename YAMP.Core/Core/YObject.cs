using System;
using System.Collections.Generic;
using System.Reflection;
using YAMP.Attributes;

namespace YAMP.Core
{
    public class YObject : IObject
    {
        object _content;
        Type _type;

        const string ARRAY_LENGTH = "length";

        public YObject(object content)
        {
            _content = content;
            _type = content.GetType();
        }

        public bool HasProperty(string name)
        {
            if (_content is Array && name == ARRAY_LENGTH)
                return true;

            var property = _type.GetProperty(name);
            return property != null && property.GetCustomAttributes(typeof(DescriptionAttribute), false).Length != 0;
        }

        public bool TryReadProperty(string name, out Dynamic value)
        {
            var p = _type.GetProperty(name);

            if (_content is Array && name == ARRAY_LENGTH)
            {
                value = new Dynamic(((Array)_content).Length);
                return true;
            }

            if (p != null && p.CanRead)
            {
                value = new Dynamic(p.GetValue(_content, null));
                return true;
            }

            value = null;
            return false;
        }

        public bool TryWriteProperty(string name, Dynamic value)
        {
            var p = _type.GetProperty(name);

            if (p != null && p.CanWrite)
            {
                p.SetValue(_content, value.Value, null);
                return true;
            }

            return false;
        }

        public string[] Properties
        {
            get
            {
                var properties = _type.GetProperties();
                var ps = new List<string>();

                if (_content is Array)
                    ps.Add(ARRAY_LENGTH);

                foreach (var p in properties)
                {
                    if (p.GetCustomAttributes(typeof(DescriptionAttribute), false).Length > 0)
                        ps.Add(p.Name);
                }

                return ps.ToArray();
            }
        }

        public bool HasMethod(string name)
        {
            var method = _type.GetMethod(name);
            return method != null && method.GetCustomAttributes(typeof(DescriptionAttribute), false).Length != 0;
        }

        public CustomFunction GetMethod(string name)
        {
            var methods = _type.GetMethods();
            var function = new CustomFunction(name);

            for (int i = 0; i < methods.Length; i++)
            {
                if (methods[i].Name == name && methods[i].MemberType != MemberTypes.Property && methods[i].GetCustomAttributes(typeof(DescriptionAttribute), false).Length != 0)
                {
                    var parameters = methods[i].GetParameters();
                    var item = new YFunction.ReflectedFunctionItem(methods[i], _content);
                    function.AddOverload(item);
                }
            }

            return function;
        }

        public string[] Methods
        {
            get
            {
                var methods = _type.GetMethods();
                var ms = new List<string>();

                foreach (var m in methods)
                {
                    if (m.GetCustomAttributes(typeof(DescriptionAttribute), false).Length > 0)
                        ms.Add(m.Name);
                }

                return ms.ToArray();
            }
        }
    }
}
