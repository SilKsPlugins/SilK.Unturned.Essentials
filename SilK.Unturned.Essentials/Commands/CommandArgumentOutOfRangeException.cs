using OpenMod.API.Commands;

namespace SilK.Unturned.Essentials.Commands
{
    public class CommandArgumentOutOfRangeException : UserFriendlyException
    {
        public CommandArgumentOutOfRangeException(string message) : base(message)
        {
        }
    }
}
