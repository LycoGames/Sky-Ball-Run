using System.Collections.Generic;

namespace _Game.Scripts.Base.Component
{
    public class ComponentContainer
    {
        private readonly Dictionary<string, object> components;

        public ComponentContainer()
        {
            components = new Dictionary<string, object>();
        }

        public void AddComponent(string componentKey, object component)
        {
            components.Add(componentKey, component);
        }

        public object GetComponent(string componentKey)
        {
            return !components.ContainsKey(componentKey) ? null : components[componentKey];
        }
    }
}