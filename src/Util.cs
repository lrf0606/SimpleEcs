namespace SimpleEcs
{
    public class EcsUtil
    {
        public static int GetComponentId<TComponent>()
        {
            return ComponentIdHelper<TComponent>.GetCompoentId();
        }
    }
}
