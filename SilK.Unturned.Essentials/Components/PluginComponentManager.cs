using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenMod.API.Plugins;
using OpenMod.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SilK.Unturned.Essentials.Components
{
    public class PluginComponentManager
    {
        private readonly IOpenModPlugin _plugin;
        private readonly ILogger<PluginComponentManager> _logger;
        private readonly IServiceProvider _serviceProvider;

        private readonly List<IPluginComponent> _loadedComponents = new();

        private bool _loaded;

        public PluginComponentManager(IOpenModPlugin plugin)
        {
            _plugin = plugin;

            var lifetimeScope = _plugin.LifetimeScope;

            _logger = lifetimeScope.Resolve<ILogger<PluginComponentManager>>();
            _serviceProvider = lifetimeScope.Resolve<IServiceProvider>();
        }

        public IReadOnlyCollection<IPluginComponent> LoadedComponents => _loadedComponents;

        public async Task LoadAsync()
        {
            if (_loaded)
            {
                throw new Exception("Plugin components already loaded.");
            }

            _loaded = true;

            var componentTypes = GetComponentTypes();

            _logger.LogInformation("Loading {PluginName} plugin components...", _plugin.DisplayName);

            foreach (var componentType in componentTypes)
            {
                IPluginComponent pluginComponent;
                string componentName;

                try
                {
                    pluginComponent =
                        (IPluginComponent)ActivatorUtilities.CreateInstance(_serviceProvider, componentType);

                    componentName = pluginComponent.Name;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "An error occurred when constructing the plugin component of type {ComponentType}.",
                        componentType);

                    continue;
                }

                try
                {
                    await pluginComponent.LoadAsync();

                    _loadedComponents.Add(pluginComponent);

                    _logger.LogInformation("Loaded {ComponentName} plugin component.", componentName);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while loading the {ComponentName} plugin component.",
                        componentName);
                }
            }

            _logger.LogInformation(
                "Finished loading {PluginName} plugin components. {ComponentsCount} components were loaded.",
                _plugin.DisplayName, LoadedComponents.Count);
        }

        public async Task UnloadAsync()
        {
            if (!_loaded)
            {
                throw new Exception("Plugin components not loaded.");
            }

            _loaded = false;

            _logger.LogInformation("Unloading {PluginName} plugin components...", _plugin.DisplayName);

            var unloadedComponents = 0;

            foreach (var component in LoadedComponents.ToList())
            {
                try
                {
                    _loadedComponents.Remove(component);

                    await component.UnloadAsync();

                    unloadedComponents++;

                    _logger.LogInformation("Unloaded {ComponentName} plugin component.", component.Name);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex, "An error occurred when unloading the {ComponentName} plugin component.",
                        component.Name);
                }
            }

            _logger.LogInformation(
                "Finished unloading {PluginName} plugin components. {ComponentsCount} components were loaded.",
                _plugin.DisplayName, unloadedComponents);
        }

        private IEnumerable<Type> GetComponentTypes()
        {
            var pluginAssembly = _plugin.GetType().Assembly;

            return pluginAssembly.FindTypes<IPluginComponent>();
        }
    }
}
