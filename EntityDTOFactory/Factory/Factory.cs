using System;
using System.Collections.Generic;
using System.Reflection;

namespace EntityDTOFactory.Factory
{
    public class Factory<Product>
    {
        public readonly Dictionary<string, Type> map = new Dictionary<string, Type>();
        public Factory()
        {
            Type[] types = Assembly.GetAssembly(typeof(Product)).GetTypes();
            foreach (Type type in types)
            {
                if (!typeof(Product).IsAssignableFrom(type) || type == typeof(Product))
                {
                    continue;
                }

                map.Add(type.Name, type);
            }
        }

        public Product CreateObject(string productName, params object[] args)
        {
            return (Product)Activator.CreateInstance(map[productName], args);
        }
    }
}
