extern alias JetBrainsAnnotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using OpenMod.API.Commands;
using OpenMod.Core.Commands;
using OpenMod.Core.Ioc;
using OpenMod.Extensions.Games.Abstractions.Items;
using OpenMod.Extensions.Games.Abstractions.Players;
using OpenMod.Extensions.Games.Abstractions.Vehicles;
using OpenMod.Unturned.Items;
using OpenMod.Unturned.Locations;
using OpenMod.Unturned.Players;
using OpenMod.Unturned.Users;
using OpenMod.Unturned.Vehicles;
using SilK.OpenMod.Unturned.Extended.Commands;
using SilK.Unturned.Essentials.Configuration;
using SilK.Unturned.Essentials.Localization;
using SilK.Unturned.Extras.Configuration;
using Steamworks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Threading.Tasks;

namespace SilK.Unturned.Essentials.Commands
{
    [DontAutoRegister]
    [LocalizationSection("Commands")]
    public abstract class EssentialsCommand : UnturnedParameterizedCommand
    {
        protected readonly IStringLocalizer GlobalStringLocalizer;

        protected readonly IStringLocalizer CommandsStringLocalizer;

        protected readonly IStringLocalizer ErrorsStringLocalizer;

        protected readonly IStringLocalizer StringLocalizer;

        protected readonly IConfigurationParser<UnturnedEssentialsConfiguration> Configuration;

        protected EssentialsCommand(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            GlobalStringLocalizer = serviceProvider.GetRequiredService<IStringLocalizer>();
            CommandsStringLocalizer = GetSubSectionStringLocalizer(GlobalStringLocalizer, typeof(EssentialsCommand));
            ErrorsStringLocalizer = new SubStringLocalizer(CommandsStringLocalizer, "Errors");
            StringLocalizer = GetSubSectionStringLocalizer(GlobalStringLocalizer, GetType());

            Configuration = serviceProvider.GetRequiredService<IConfigurationParser<UnturnedEssentialsConfiguration>>();
        }

        protected UnturnedUser UnturnedUser => Context.Actor as UnturnedUser ??
                                               throw new Exception(
                                                   $"Command actor is not of type {nameof(UnturnedUser)}");

        protected UnturnedPlayer UnturnedPlayer => UnturnedUser.Player;

        protected override Task ExecuteMethod(MethodInfo method, object[] parameters)
        {
            void VerifyParameter(ParameterInfo parameterInfo, object parameterValue)
            {
                var attributes = parameterInfo.GetCustomAttributes();

                foreach (var attribute in attributes)
                {
                    switch (attribute)
                    {
                        case RangeAttribute rangeAttribute:
                            if (!rangeAttribute.IsValid(parameterValue))
                            {
                                var hasUpperBound = rangeAttribute is { Maximum: int.MaxValue } or
                                    { Maximum: float.MaxValue };

                                var hasLowerBound = rangeAttribute is { Minimum: int.MinValue } or
                                    { Minimum: float.MinValue };

                                var translationName = hasUpperBound switch
                                {
                                    true when hasLowerBound => "Input:OutOfRange:BothBounds",
                                    true => "Input:OutOfRange:UpperBound",
                                    _ => "Input:OutOfRange:LowerBound"
                                };

                                throw new CommandArgumentOutOfRangeException(
                                    ErrorsStringLocalizer[translationName,
                                        new
                                        {
                                            Input = parameterValue,
                                            ParameterName = parameterInfo.Name,
                                            LowerBound = rangeAttribute.Minimum,
                                            UpperBound = rangeAttribute.Maximum
                                        }]);
                            }

                            break;
                    }
                }
            }

            var parameterInfos = method.GetParameters();

            // Verify parameters based on custom needs
            for (var i = 0; i < parameterInfos.Length && i < parameters.Length; i++)
            {
                VerifyParameter(parameterInfos[i], parameters[i]);
            }

            try
            {
                return base.ExecuteMethod(method, parameters);
            }
            catch (CommandIndexOutOfRangeException)
            {
                throw new CommandWrongUsageException(Context);
            }
            catch (CommandParameterParseException ex)
            {
                var customLocalizedMessageNames = new Dictionary<Type, string>()
                {
                    { typeof(IItemAsset), "NoItem" },
                    { typeof(UnturnedItemAsset), "NoItem" },
                    { typeof(IVehicleAsset), "NoVehicle" },
                    { typeof(UnturnedVehicleAsset), "NoVehicle" },
                    { typeof(IPlayer), "NoPlayer" },
                    { typeof(UnturnedPlayer), "NoPlayer" },
                    { typeof(UnturnedUser), "NoPlayer" },
                    { typeof(UnturnedLocation), "NoLocation" },
                    { typeof(CSteamID), "NoSteamID" },
                    { typeof(bool), "NoBoolean" },
                    { typeof(short), "NoShort" },
                    { typeof(int), "NoInteger" },
                    { typeof(long), "NoLong" },
                    { typeof(ushort), "NoUnsignedShort" },
                    { typeof(uint), "NoUnsignedInteger" },
                    { typeof(ulong), "NoUnsignedLong" },
                    { typeof(float), "NoFloat" },
                    { typeof(double), "NoDouble" },
                    { typeof(decimal), "NoDecimal" }
                };

                if (customLocalizedMessageNames.TryGetValue(ex.ExpectedType, out var messageName))
                {
                    throw new UserFriendlyException(ErrorsStringLocalizer[messageName, new { Input = ex.Argument }]);
                }

                throw new CommandWrongUsageException(Context);
            }
        }

        public virtual Task PrintLocalizedAsync(string name, params object[] arguments)
        {
            return PrintAsync(StringLocalizer[name, arguments]);
        }

        public UserFriendlyException GetLocalizedFriendlyException(string name, params object[] arguments)
        {
            return new UserFriendlyException(StringLocalizer[name, arguments]);
        }

        private IStringLocalizer GetSubSectionStringLocalizer(IStringLocalizer baseStringLocalizer, Type type)
        {
            var subSections = LocalizationSectionHelper.GetSubSectionsFromType(type);

            return new SubStringLocalizer(baseStringLocalizer, subSections);
        }
    }
}
