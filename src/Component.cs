namespace SimpleEcs
{
    public abstract class ComponentBase
    {
        
    }

    internal class CompoentIdData
    {
        public const int COMPONENT_ID_START = 0;
        public static int CUR_COMPONENT_ID = COMPONENT_ID_START;
    }

    public class ComponentIdHelper<TCompnent>
    {
        // every TCompnent has a static field as ComponentId
        private static int m_ComponentId = CompoentIdData.COMPONENT_ID_START; 
        public static int GetCompoentId()
        {
            if (m_ComponentId == CompoentIdData.COMPONENT_ID_START)
            {
                m_ComponentId = ++CompoentIdData.CUR_COMPONENT_ID;
            }
            return m_ComponentId;
        }
    }

}
