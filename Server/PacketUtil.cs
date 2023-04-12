using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server;

public static class PacketUtil
{
    public static bool IsPacketRight(Packet p)
    {
        string idCheckString = p.ID;
        if (idCheckString.Length < 6)
        {
            return false;
        }

        if (!idCheckString.Substring(0, 6).Equals("Player"))
        {
            return false;
        }

        var commandCheck = Command.GetCommandList();
        if (!commandCheck.Contains(p.Command))
        {
            return false;
        }

        return true;
    }
}
