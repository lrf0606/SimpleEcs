using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleEcs
{
    public class ComponentBase
    {
 
    }

    internal class CompoentIdData
    {
        public const int COMPONENT_ID_START = 0;
        public static int CUR_COMPONENT_ID = COMPONENT_ID_START;
    }

    public class ComponentIdHelper<TCompnent>
    {
        // must be static, every TCompnent has a static field
        private static int m_componentId = CompoentIdData.COMPONENT_ID_START; 
        public static int GetCompoentId()
        {
            if (m_componentId == CompoentIdData.COMPONENT_ID_START)
            {
                m_componentId = ++CompoentIdData.CUR_COMPONENT_ID;
            }
            return m_componentId;
        }
    }

}
