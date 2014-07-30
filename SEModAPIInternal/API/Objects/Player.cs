using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SEModAPIInternal.API.Objects
{
    class Player
    {

        public ulong UserId { get; set; }
        public string Username { get; set; }
        public string Ip { get; set; }
        public string CommunityUrl { get; set; }

    }
}
