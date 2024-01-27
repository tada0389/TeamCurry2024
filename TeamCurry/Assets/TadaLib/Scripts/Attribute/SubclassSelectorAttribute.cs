using System;
using UnityEngine;

namespace TadaLib.Attribute
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class SubclassSelectorAttribute : PropertyAttribute
    {
        bool _includeMono;

        public SubclassSelectorAttribute(bool includeMono = false)
        {
            _includeMono = includeMono;
        }

        public bool IsIncludeMono()
        {
            return _includeMono;
        }
    }
}