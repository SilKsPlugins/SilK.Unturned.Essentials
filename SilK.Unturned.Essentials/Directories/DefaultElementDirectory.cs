using SilK.Unturned.Essentials.API.Directories.Sources;
using System;
using System.Collections.Generic;

namespace SilK.Unturned.Essentials.Directories
{
    public abstract class DefaultElementDirectory<TElementSource> : IDisposable
    {
        private readonly List<TElementSource> _sources = new();

        private bool _disposed;

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            lock (_sources)
            {
                _sources.Clear();
            }
        }

        public IReadOnlyCollection<TElementSource> Sources
        {
            get
            {
                lock (_sources)
                {
                    return _sources.ToArray();
                }
            }
        }

        public void AddSource(TElementSource source)
        {
            if (_disposed)
            {
                return;
            }

            lock (_sources)
            {
                _sources.Add(source);
            }
        }

        public void RemoveSource(TElementSource source)
        {
            if (_disposed)
            {
                return;
            }

            lock (_sources)
            {
                _sources.Remove(source);
            }
        }
    }
}
