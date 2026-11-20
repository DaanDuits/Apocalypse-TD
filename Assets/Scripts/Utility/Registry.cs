using System;
using System.Collections.Generic;
using System.Linq;

namespace TowerDefense.Utilities
{
    public static class Registry<T>
    {
        public static HashSet<T> Items { get; private set; } = new HashSet<T>();

        public static void Register(T item) 
        { 
            Items.Add(item); 
        }
        public static void Unregister(T item)
        {
            Items.Remove(item);
        }

        public static IEnumerable<T> Filter(Func<T, bool> filter)
        {
            return Items.Where(filter);
        }
    }
}
