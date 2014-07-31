using SEModAPIExtensions.API;
using SEServerExtender.Properties;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace SEServerExtender.Threads
{
    public class ChatAutoMessaging : Worker
    {

        private int current = 0;
        private volatile int max = 0;
        private volatile StringCollection messages = null;

        public void ReloadMessages()
        {
            messages = Settings.Default.Chat_Auto_Messages;
            max = messages.Count;
        }

        public override void Work()
        {
            if (current == max)
                current = 0;
            try
            {
                ChatManager.Instance.SendPublicChatMessage("Automessage: " + messages[current]);
            }
            catch (Exception) { }
            current++;
        }

    }
}
