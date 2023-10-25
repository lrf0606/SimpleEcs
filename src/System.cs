using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public abstract int[] Matcher();
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
        public SystemBase System; // 绑定的系统
        public int[] Matcher; // 系统的匹配组件
        public HashSet<Entity> EntitySet; // 系统关注的实体

        public SystemInfo(SystemBase system)
        {
            System = system;
            Matcher = system.Matcher();
            EntitySet = new HashSet<Entity>();
        }

        public void MatchEntity(in Entity entity)
        {
            if (entity.HasComponents(Matcher))
            {
                EntitySet.Add(entity);
            }
            else
            {
                EntitySet.Remove(entity);
            }
        }

        public void Update(World world)
        {
            foreach(var entity in EntitySet)
            {
                System.Update(world, entity);
            }
        }
    }

 

}
