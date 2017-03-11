using System;

namespace SmokSmog.Navigation
{
    public enum ContentType
    {
        Main,
        Second
    }

    [AttributeUsage(AttributeTargets.Class)]
    internal class NavigationAttribute : Attribute
    {
        public ContentType ContentType { get; set; }
    }
}