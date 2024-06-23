using System;

namespace Sol3.Infrastructure.Patterns.Creational
{
    /// <summary>
    /// Inherit from this class to create a singleton instance of your class.  
    /// <para><seealso cref="http://www.dofactory.com/net/singleton-design-pattern"/></para>
    /// <para>NOTE:  &lt;T&gt; is the same class that inherits this object!</para>
    /// </summary>
    /// <example>
    /// <code>
    /// public MyClass : Singleton{MyClass}
    /// </code>
    /// </example>
    /// <typeparam name="T">The class that is implementing this base class</typeparam>
    public class Singleton<T> where T : new()
    {
        protected static readonly Lazy<T> Lazy = new Lazy<T>(() => new T());

        public static T Instance => Lazy.Value;

        protected Singleton() { }
    }
}
