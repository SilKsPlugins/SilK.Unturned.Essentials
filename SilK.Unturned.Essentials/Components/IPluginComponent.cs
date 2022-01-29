using System.Threading.Tasks;

namespace SilK.Unturned.Essentials.Components
{
    public interface IPluginComponent
    {
        string Name { get; }

        Task LoadAsync();

        Task UnloadAsync();
    }
}
