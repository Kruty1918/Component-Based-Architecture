using UnityEngine;


namespace SGS29.CBA
{
    public abstract class CBAFilterRule : ScriptableObject
    {
        public abstract bool IsMatch(string component);
    }
}