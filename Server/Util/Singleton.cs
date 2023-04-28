using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class Singleton<T> where T : new()
{
    private static readonly Lazy<T> _instance = new Lazy<T>(() => new T());
    public static T Instance => _instance.Value;
}
