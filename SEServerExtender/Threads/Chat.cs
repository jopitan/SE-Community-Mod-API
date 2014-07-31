using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEServerExtender.Threads
{
    public class Chat : Worker
    {

        public SEServerExtender sese { get; set; }

        public Chat() : base(1000) { }

        public override void Work()
        {
            if (sese.m_server.CommandLineArgs.noGUI) {
                Shutdown();
                return;
            }

            sese.LST_Chat_Messages.BeginUpdate();
            sese.LST_Chat_Messages.EndUpdate();
        }

    }
}
