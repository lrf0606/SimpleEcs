using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
