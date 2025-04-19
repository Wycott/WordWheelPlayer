namespace WordWheelPlayer.Annotations
{
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AiGeneratedAttribute : Attribute
    {
        public string Disclaimer { get; } = string.Empty;

        public AiGeneratedAttribute(string disclaimer)
        {
            Disclaimer = disclaimer;
        }

        public AiGeneratedAttribute()
        {
            // Chew gum
        }
    }
}
