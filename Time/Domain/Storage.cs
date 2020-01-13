using System;
using System.Collections.Generic;

namespace TransportTycoon.Domain
{
    public abstract class Storage
    {
        public List<Container> Containers { get; }
        public Location Location { get; }

        protected Storage(Location location)
        {
            Location = location;
            Containers = new List<Container>();
        }

        public void Stock(Container container)
        {
            Containers.Add(container);
            OnStocked(container);
        }

        public void LoadContainer(Action<Container> load)
        {
            if (Containers.Count == 0) return;
            load(Containers[0]);
            Containers.RemoveAt(0);
        }

        protected virtual void OnStocked(Container container) { }
    }
}