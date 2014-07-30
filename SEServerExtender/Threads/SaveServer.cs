using SEModAPIInternal.API.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SEServerExtender.Threads
{
    public class SaveServer : Worker
    {

        public override void Work()
        {
            WorldManager.Instance.SaveWorld();
        }

    }
}
