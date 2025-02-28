using System.Collections.Generic;
using UnityEngine;

namespace SGS29.CBA
{
    public abstract class CBAFilterRule : ScriptableObject
    {
        public abstract bool IsMatch(string component);
        public abstract void FilterBy<T>(T types) where T : IFilterCollection;
    }
}