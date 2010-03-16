using System;
using System.Collections.Generic;

namespace TW.Core
{
    public class Factory<T> where T : class
    {
        private readonly Dictionary<Type, FactoryMethod> createObjectMethods = new Dictionary<Type, FactoryMethod>();

        private FactoryMethod FindFactoryMethod(Type type)
        {
            FactoryMethod method;
            return !createObjectMethods.TryGetValue(type, out method) ? null : method;
        }

        public void Add(Type type, FactoryMethod handler)
        {
            if (!createObjectMethods.ContainsKey(type))
                createObjectMethods.Add(type, handler);
            else
                createObjectMethods[type] = handler;
        }

        public T CreateObject(Type type, params object[] constructorArguments)
        {
            var method = FindFactoryMethod(type);
            return method != null ? method(type, constructorArguments) as T : default(T);
        }
    }
}