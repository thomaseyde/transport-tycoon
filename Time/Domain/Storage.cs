using System.Collections.Generic;
using System.Linq;

namespace TransportTycoon.Domain
{
    public abstract class Storage
    {
        protected Storage(Location location)
        {
            Location = location;
            Containers = new List<Container>();
        }

        public List<Container> Containers { get; }
        public Location Location { get; }

        public void Stock(Container container)
        {
            Containers.Add(container);
            OnStocked(container);
        }

        protected virtual void OnStocked(Container container)
        {
        }

        public void Unload(Container container)
        {
            Containers.Add(container);
        }

        public Option<Container> LoadContainer()
        {
            var container = Containers[0];
            Containers.RemoveAt(0);
            return Option.Some(container);
        }
    }
}