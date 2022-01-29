using OpenMod.API.Commands;
using OpenMod.Core.Cooldowns;
using System;
using System.Threading.Tasks;

namespace SilK.Unturned.Essentials.Helpers
{
    public static class CommandCooldownStoreExtensions
    {
        public static async Task<TimeSpan> GetTimeLeft(this ICommandCooldownStore cooldownStore, ICommandActor actor,
            string cooldownId, TimeSpan cooldown)
        {
            var lastExecuted = await cooldownStore.GetLastExecutedAsync(actor, cooldownId) ?? DateTime.MinValue;

            var timeLeft = cooldown - (DateTime.Now - lastExecuted);

            return timeLeft;
        }
    }
}
