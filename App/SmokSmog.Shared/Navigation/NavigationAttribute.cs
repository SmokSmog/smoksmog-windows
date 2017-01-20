using System;

namespace SmokSmog.Navigation
{
    public enum ContentType
    {
        Main,
        Second
    }

    [AttributeUsage(AttributeTargets.Class)]
    internal class NavigationAttribute : System.Attribute
    {
        public ContentType ContentType { get; set; }
    }
}