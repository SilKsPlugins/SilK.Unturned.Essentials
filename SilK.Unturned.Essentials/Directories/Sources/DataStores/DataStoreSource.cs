using Microsoft.Extensions.DependencyInjection;
using Nito.AsyncEx;
using OpenMod.API.Persistence;
using SilK.Unturned.Essentials.API.Directories.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SilK.Unturned.Essentials.Directories.Sources.DataStores
{
    public abstract class DataStoreSource<TElement, TElementData, TElementIdentifier> : IElementSource<TElement>
        where TElementData : new()
    {
        private readonly IDataStore _dataStore;
        private readonly IServiceProvider _serviceProvider;

        private readonly AsyncLock _elementLock = new();
        private readonly List<TElementData> _elementDatas = new();
        private readonly List<TElement> _elements = new();

        protected abstract string DataStoreKey { get; }

        protected IReadOnlyCollection<TElement> Contents => _elements.AsReadOnly();

        protected DataStoreSource(IServiceProvider serviceProvider)
        {
            _dataStore = serviceProvider.GetRequiredService<IDataStore>();
            _serviceProvider = serviceProvider;
        }

        protected abstract TElementIdentifier GetId(TElementData elementData);

        protected abstract bool ElementMatchesData(TElement element, TElementData elementData);

        protected abstract bool IdentifiersMatch(TElementIdentifier id1, TElementIdentifier id2);

        private void RefreshElementInstances()
        {
            // remove all elements which are no longer in _elements
            var elementsToRemove = _elements
                .Where(element =>
                    !_elementDatas.Any(
                        elementData => ElementMatchesData(element, elementData)))
                .ToList();

            // create all elements which aren't already in _elements
            var elementsToCreate = _elementDatas
                .Where(elementData =>
                    !_elements.Any(
                        element => ElementMatchesData(element, elementData)))
                .ToList();

            // remove no longer existing elements
            elementsToRemove.ForEach(element => _elements.Remove(element));

            // add new elements
            foreach (var elementData in elementsToCreate)
            {
                var element = ActivatorUtilities.CreateInstance<TElement>(_serviceProvider, elementData);

                _elements.Add(element);
            }
        }

        public Task<IReadOnlyCollection<TElement>> GetContentsAsync()
        {
            return Task.FromResult(Contents);
        }

        public async Task LoadAsync()
        {
            var dataStoreElements =
                await _dataStore.LoadAsync<TElementData[]>(DataStoreKey) ?? Array.Empty<TElementData>();

            using (await _elementLock.LockAsync())
            {
                _elementDatas.Clear();
                _elementDatas.AddRange(dataStoreElements);

                RefreshElementInstances();
            }
        }

        private async Task SaveInternalAsync()
        {
            var dataStoreElements = _elementDatas.ToArray();

            await _dataStore.SaveAsync(DataStoreKey, dataStoreElements);
        }

        public async Task SaveAsync()
        {
            using (await _elementLock.LockAsync())
            {
                await SaveInternalAsync();
            }
        }

        /// <summary>
        /// Adds or updates the element of the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the element.</param>
        /// <param name="addMethod">The method to set the info when adding the element.</param>
        /// <param name="updateMethod">The method to update the info when updating the element.</param>
        /// <param name="save">Whether or not to force a save after adding/updating.</param>
        /// <returns><c>true</c> when the element is added. <c>false</c> when the element is updated.</returns>
        public async Task<bool> AddOrUpdateAsync(TElementIdentifier id, Action<TElementData> addMethod, Action<TElementData> updateMethod, bool save = true)
        {
            bool isAdded;

            using (await _elementLock.LockAsync())
            {
                var elementData = _elementDatas.FirstOrDefault(x => IdentifiersMatch(GetId(x), id));

                isAdded = elementData == null;

                elementData ??= new();

                if (isAdded)
                {
                    addMethod(elementData);
                    _elementDatas.Add(elementData);
                }
                else
                {
                    updateMethod(elementData);
                }

                if (save)
                {
                    await SaveInternalAsync();
                }

                RefreshElementInstances();
            }

            return isAdded;
        }

        /// <summary>
        /// Adds or updates the element of the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the element.</param>
        /// <param name="addOrUpdateMethod">The method to set the info when adding or updating the element.</param>
        /// <param name="save">Whether or not to force a save after adding/updating.</param>
        /// <returns><c>true</c> when the element is added. <c>false</c> when the element is updated.</returns>
        public Task<bool> AddOrUpdateAsync(TElementIdentifier id, Action<TElementData> addOrUpdateMethod, bool save = true)
        {
            return AddOrUpdateAsync(id, addOrUpdateMethod, addOrUpdateMethod, save);
        }

        /// <summary>
        /// Removes the element of the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the element.</param>
        /// <param name="save">Whether or not to force a save after adding/updating.</param>
        /// <returns><c>true</c> if the element was found and was deleted; <c>false</c> otherwise.</returns>
        public async Task<bool> RemoveAsync(TElementIdentifier id, bool save = true)
        {
            using (await _elementLock.LockAsync())
            {
                var elementData = _elementDatas.FirstOrDefault(x => IdentifiersMatch(GetId(x), id));

                if (elementData == null)
                {
                    return false;
                }

                _elementDatas.Remove(elementData);

                if (save)
                {
                    await SaveInternalAsync();
                }

                RefreshElementInstances();

                return true;
            }
        }
    }
}
