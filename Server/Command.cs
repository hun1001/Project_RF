
namespace Server
{
    public static class Command
    {
        public const string COMMAND_MOVE = "Move";
        public const string COMMAND_LEFT = "Left";
        public const string COMMAND_ENTER = "Enter";
        public const string COMMAND_REGISTER = "Register";
        public const string COMMAND_ATTACK = "Attack";
        public const string COMMAND_DAMAGED = "Damaged";

        public static string[] GetCommandList() => new string[] { COMMAND_MOVE, COMMAND_LEFT, COMMAND_ENTER, COMMAND_REGISTER, COMMAND_ATTACK, COMMAND_DAMAGED };
    }
}
