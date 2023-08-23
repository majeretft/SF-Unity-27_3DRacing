using UnityEngine;

namespace SF3DRacing
{
    public abstract class SettingBase : ScriptableObject
    {
        public string Title;

        public virtual bool IsMinValue { get; set; }
        public virtual bool IsMaxValue { get; set; }

        public virtual void SetNextValue() { }
        public virtual void SetPrevValue() { }
        public virtual object GetValue() { return default; }
        public virtual string GetStringValue() { return string.Empty; }

        public virtual void Apply() { }
        public virtual void Load() { }
    }
}
