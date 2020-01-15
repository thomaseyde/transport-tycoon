using System;
using System.Collections.Generic;

namespace TransportTycoon.Domain
{
    public abstract class Storage
    {
        readonly List<Container> containers;

        public IEnumerable<Container> Containers => containers;

        public Location Location { get; }

        protected Storage(Location location)
        {
            Location = location;
            containers = new List<Container>();
        }

        public void Replenish(Container container)
        {
            containers.Add(container);
            OnStocked(container);
        }

        public void Deplete(Action<Container> load)
        {
            if (containers.Count == 0) return;
            var container = containers[0];
            containers.RemoveAt(0);
            load(container);
        }

        protected virtual void OnStocked(Container container) { }
    }
}