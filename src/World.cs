namespace SimpleEcs
{
    public class World
    {
        private int m_NextEntityId = 0;

        private Dictionary<int, Entity> m_EntityDict;

        private Dictionary<int, List<SystemInfo>> m_SystemDict;

        private Dictionary<int, List<SystemInfo>> m_ComponentSystemDict;

        public void RegisterSystem(SystemBase system)
        {
            SystemInfo systemInfo = new SystemInfo(system);
            int systemType = system.GetSystemType();
            if (m_SystemDict.TryGetValue(systemType, out var systemInfoList1))
            {
                systemInfoList1.Add(systemInfo);
            }
            else
            {
                m_SystemDict[systemType] = new List<SystemInfo> { systemInfo };
            }
            foreach (int compoentId in systemInfo.Matchers)
            {
                if (m_ComponentSystemDict.TryGetValue(compoentId, out var systemInfoList2))
                {
                    systemInfoList2.Add(systemInfo);
                }
                else
                {
                    m_ComponentSystemDict[compoentId] = new List<SystemInfo> { systemInfo };
                }
            }
        }

        public World()
        {
            m_EntityDict = new Dictionary<int, Entity>();
            m_SystemDict = new Dictionary<int, List<SystemInfo>>();
            m_ComponentSystemDict = new Dictionary<int, List<SystemInfo>>();
        }

        private void OnEntityComponentChangedDelegate(in Entity entity, int componentId, bool isAddComponent)
        {
            if (m_ComponentSystemDict.TryGetValue(componentId, out var systemInfoList))
            {
                foreach (var systemInfo in systemInfoList)
                {
                    systemInfo.MatchEntity(entity);
                } 
            }
        }

        public Entity CreateEntity(int entityId=0, string entityName="")
        {
            if (entityId == 0)
            {
                entityId = ++m_NextEntityId;
            }
            var entity = new Entity(entityId, entityName, this);
            entity.OnComponentChangedEvent += OnEntityComponentChangedDelegate;
            return entity;
        }
        
        public Entity? GetEntityById(int entityId)
        {
            m_EntityDict.TryGetValue(entityId, out Entity? entity);
            return entity;
        }

        public void DestoryEntity(int entityId)
        {
            if (m_EntityDict.ContainsKey(entityId))
            {
                m_EntityDict.Remove(entityId);
            }
        }

        public void DestoryEntity(in Entity entity)
        {
            if (entity != null)
            {
                DestoryEntity(entity.ID);
            }
        }

        private void SystemUpdate(int systemType)
        {
            if (m_SystemDict.TryGetValue(systemType, out var systemInfoList))
            {
                foreach (var systemInfo in systemInfoList)
                {
                    systemInfo.Update(this);
                }
            }
        }

        public void Update()
        {
            SystemUpdate((int)SystemTypeEnum.UPDATE_SYSTEM);
        }

        public void FixedUpdate()
        {
            SystemUpdate((int)SystemTypeEnum.FIXED_UPDATE_SYSTEM);
        }

        public void LateUpdate()
        {
            SystemUpdate((int)SystemTypeEnum.LATE_UPDATE_SYSTEM);
        }

    }


}
