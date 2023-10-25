using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleEcs
{
    public class World
    {
        private int m_nextEntityId = 0;

        private Dictionary<int, Entity> m_entityDict;

        private Dictionary<int, List<SystemInfo>> m_systemDict;

        private Dictionary<int, List<SystemInfo>> m_componentSystemDict;

        public void RegisterSystem(SystemBase system)
        {
            SystemInfo systemInfo = new SystemInfo(system);
            int systemType = system.GetSystemType();

  
            if (m_systemDict.TryGetValue(systemType, out var systemInfoList1))
            {
                systemInfoList1.Add(systemInfo);
            }
            else
            {
                m_systemDict[systemType] = new List<SystemInfo> { systemInfo };
            }
            foreach (int compoentId in systemInfo.Matcher)
            {
                if (m_componentSystemDict.TryGetValue(compoentId, out var systemInfoList2))
                {
                    systemInfoList2.Add(systemInfo);
                }
                else
                {
                    m_componentSystemDict[compoentId] = new List<SystemInfo> { systemInfo };
                }
            }
        }

        public World()
        {
            m_entityDict = new Dictionary<int, Entity>();
            m_systemDict = new Dictionary<int, List<SystemInfo>>();
            m_componentSystemDict = new Dictionary<int, List<SystemInfo>>();
        }

        private void OnEntityComponentChangedDelegate(in Entity entity, int componentId, bool isAddComponent)
        {
            if (m_componentSystemDict.TryGetValue(componentId, out var systemInfoList))
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
                entityId = ++m_nextEntityId;
            }
            var entity = new Entity(entityId, entityName, this);
            entity.OnComponentChangedEvent += OnEntityComponentChangedDelegate;
            return entity;
        }
        
        public Entity? GetEntityById(int entityId)
        {
            m_entityDict.TryGetValue(entityId, out Entity? entity);
            return entity;
        }

        public void DestoryEntity(int entityId)
        {
            if (m_entityDict.ContainsKey(entityId))
            {
                m_entityDict.Remove(entityId);
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
            if (m_systemDict.TryGetValue(systemType, out var systemInfoList))
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
