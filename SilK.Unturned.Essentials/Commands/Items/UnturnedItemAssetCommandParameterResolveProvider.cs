using OpenMod.API.Commands;
using OpenMod.Extensions.Games.Abstractions.Items;
using OpenMod.Unturned.Items;
using SDG.Unturned;
using System;
using System.Threading.Tasks;

namespace SilK.Unturned.Essentials.Commands.Items
{
    public class UnturnedItemAssetCommandParameterResolveProvider : ICommandParameterResolveProvider
    {
        private readonly IItemDirectory _itemDirectory;

        public UnturnedItemAssetCommandParameterResolveProvider(IItemDirectory itemDirectory)
        {
            _itemDirectory = itemDirectory;
        }

        public bool Supports(Type type)
        {
            return type == typeof(UnturnedItemAsset) || type == typeof(IItemAsset) || type == typeof(ItemAsset);
        }

        public async Task<object?> ResolveAsync(Type type, string input)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (!Supports(type))
            {
                throw new ArgumentException("The given type is not supported", nameof(type));
            }

            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            var itemAsset = await _itemDirectory.FindByIdAsync(input) ??
                            await _itemDirectory.FindByNameAsync(input, false);

            if (type == typeof(IItemAsset))
            {
                return itemAsset;
            }

            var unturnedItemAsset = itemAsset as UnturnedItemAsset;

            if (type == typeof(UnturnedItemAsset))
            {
                return unturnedItemAsset;
            }

            if (type == typeof(ItemAsset))
            {
                return unturnedItemAsset?.ItemAsset;
            }

            throw new InvalidOperationException($"Unable to return type {type}");
        }
    }
}
