namespace SimpleEcs
{
    public class Entity
    {
        // ID
        private int m_Id;
        public int ID => m_Id;

        // Name use for debug
        private string m_Name;
        public string Name => m_Name;

        // belong to the world
        private World? m_World;
        public World? World => m_World;
        
        // A dict to save Entity's all Components, key is component id, value is component object
        private Dictionary<int, ComponentBase> m_ComponentDict;

        // entity add or remove component event
        public delegate void ComponentChangedDelegate(in Entity entity, int componentId, bool isAddComponent);
        public event ComponentChangedDelegate? OnComponentChangedEvent;

        // Constructor, make sure id is unique
        public Entity(int id, string name ="", in World? world = null)
        {
            m_Id = id;
            if (string.IsNullOrEmpty(name))
            {
                m_Name = $"Entity-{id}";
            }
            else
            {
                m_Name = name;
            }
            m_World = world;
            
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

        public bool HasComponents(IEnumerable<int> componentIds)
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
            return m_Id;
        }

    }
}
