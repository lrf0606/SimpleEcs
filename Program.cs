// See https://aka.ms/new-console-template for more information

using SimpleEcs;
using MyTest;



Test.Func();

namespace MyTest
{


    public class Test
    {
        public static void Func()
        {
            World world = new World();
            world.RegisterSystem(new MoveSystem());
            world.RegisterSystem(new PosSystem());
            world.RegisterSystem(new SpeedSystem());

            Entity entity1 = world.CreateEntity();
            entity1.AddComponent<MoveComponent>();

            Entity entity2 = world.CreateEntity();
            entity2.AddComponent<PosComponent>();

            Entity entity3 = world.CreateEntity();
            entity3.AddComponent<MoveComponent>();
            entity3.AddComponent<PosComponent>();

            world.Update();
            world.FixedUpdate();
            world.LateUpdate();
        }
    }

    class MoveComponent : ComponentBase
    {

    }

    class PosComponent : ComponentBase
    {

    }

    class MoveSystem : EntityUpdateSystemBase
    {
        public override int[] Matcher()
        {
            return new int[1] { EcsUtil.GetComponentId<MoveComponent>() };
        }

        public override void Update(World world, Entity entity)
        {
            Console.WriteLine("MoveSystem Update  entityId: " + entity.ID);
        }
    }

    class PosSystem : EntityFixedUpdateSystemBase
    {
        public override int[] Matcher()
        {
            return new int[1] { EcsUtil.GetComponentId<PosComponent>() };
        }

        public override void Update(World world, Entity entity)
        {
            Console.WriteLine("PosSystem Update  entityId: " + entity.ID);
        }
    }


    class SpeedSystem : EntityLateUpdateSystemBase
    {
        public override int[] Matcher()
        {
            return new int[2] { EcsUtil.GetComponentId<MoveComponent>(), EcsUtil.GetComponentId<PosComponent>() };
        }

        public override void Update(World world, Entity entity)
        {
            Console.WriteLine("SpeedSystem Update  entityId: " + entity.ID);
        }
    }
}


