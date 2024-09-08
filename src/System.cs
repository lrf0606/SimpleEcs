namespace SimpleEcs
{
    internal enum SystemTypeEnum
    {
        UPDATE_SYSTEM,
        FIXED_UPDATE_SYSTEM,
        LATE_UPDATE_SYSTEM,
    }

    public abstract class SystemBase
    {
        public abstract int GetSystemType();
        public abstract List<int> Matcher();
        public abstract void Update(World world, Entity entity);
    }

    public abstract class EntityUpdateSystemBase: SystemBase
    {
        public override int GetSystemType()
        {
            return (int)SystemTypeEnum.UPDATE_SYSTEM;
        }
    }

    public abstract class EntityFixedUpdateSystemBase : SystemBase
    {
        public override int GetSystemType()
        {
            return (int)SystemTypeEnum.FIXED_UPDATE_SYSTEM;
        }
    }

    public abstract class EntityLateUpdateSystemBase : SystemBase
    {
        public override int GetSystemType()
        {
            return (int)SystemTypeEnum.LATE_UPDATE_SYSTEM;
        }
    }

    internal struct SystemInfo
    {
        private SystemBase m_System; // 绑定的系统
        private List<int> m_Matcher; // 系统的匹配组件
        private HashSet<Entity> m_EntitySet; // 系统关注的实体

        public IEnumerable<int> Matchers => m_Matcher;

        public SystemInfo(SystemBase system)
        {
            m_System = system;
            m_Matcher = system.Matcher();
            m_EntitySet = new HashSet<Entity>();
        }

        public void MatchEntity(in Entity entity)
        {
            if (entity.HasComponents(m_Matcher))
            {
                m_EntitySet.Add(entity);
            }
            else
            {
                m_EntitySet.Remove(entity);
            }
        }

        public void Update(World world)
        {
            foreach(var entity in m_EntitySet)
            {
                m_System.Update(world, entity);
            }
        }
    }
}
