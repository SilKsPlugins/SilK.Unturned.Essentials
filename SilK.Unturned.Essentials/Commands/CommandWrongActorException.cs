using OpenMod.API.Commands;

namespace SilK.Unturned.Essentials.Commands
{
    public class CommandWrongActorException : UserFriendlyException
    {
        public CommandWrongActorException(string message) : base(message)
        {
        }
    }
}
