using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SimpleEcs
{
    public class Entity
    {
        // ID
        private int m_id;
        public int ID { get { return m_id; } }

        // Name use for debug
        private string m_name;
        public string Name { get { return m_name; } }

        // belong to the world
        private World? m_world;
        public World? World { get { return m_world; } }
        
        // A dict to save Entity's all Components, key is component id, value is component object
        private Dictionary<int, ComponentBase> m_ComponentDict;

        // entity add or remove component event
        public delegate void ComponentChangedDelegate(in Entity entity, int componentId, bool isAddComponent);
        public event ComponentChangedDelegate? OnComponentChangedEvent;

        // Constructor, make sure id is unique
        public Entity(int id, string name ="", in World? world = null)
        {
            m_id = id;
            if (string.IsNullOrEmpty(name))
            {
                m_name = $"Entity-{id}";
            }
            else
            {
                m_name = name;
            }
            m_world = world;
            
            m_ComponentDict = new Dictionary<int, ComponentBase>();
        }

        public TComponent AddComponent<TComponent>() where TComponent : ComponentBase, new()
        {
            int componentId = ComponentIdHelper<TComponent>.GetCompoentId();
            if (m_ComponentDict.TryGetValue(componentId, out var oldComponent))
            {
                return (TComponent)oldComponent;
            }
            var component = new TComponent();
            m_ComponentDict[componentId] = component;
            OnComponentChangedEvent?.Invoke(this, componentId, true);
            return component;
        }

        public void RemoveComponent<TComponent>() where TComponent : ComponentBase, new()
        {
            int componentId = ComponentIdHelper<TComponent>.GetCompoentId();
            if (m_ComponentDict.TryGetValue(componentId, out var oldComponent))
            {   
                m_ComponentDict.Remove(componentId);
                OnComponentChangedEvent?.Invoke(this, componentId, false);
            }
        }
        
        public TComponent? GetComponent<TComponent>() where TComponent : ComponentBase, new()
        {
            int componentId = ComponentIdHelper<TComponent>.GetCompoentId();
            if (m_ComponentDict.TryGetValue(componentId, out var oldComponent))
            {
                return (TComponent)oldComponent;
            }
            return null;
        }

        public bool HasComponent<TComponent>() where TComponent : ComponentBase, new()
        {
            int componentId = ComponentIdHelper<TComponent>.GetCompoentId();
            return m_ComponentDict.ContainsKey(componentId);
        }

        public bool HasComponents(int[] componentIds)
        {
            foreach(int componentId in componentIds)
            {
                if (!m_ComponentDict.ContainsKey(componentId))
                {
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            return m_id;
        }

    }
}
