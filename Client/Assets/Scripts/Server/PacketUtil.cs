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

        return true;
    }
}
