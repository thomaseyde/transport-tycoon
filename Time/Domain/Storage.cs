using System.Collections.Generic;
using TransportTycoon.Optional;

namespace TransportTycoon.Domain
{
    public abstract class Storage
    {
        public List<Container> Containers { get; }

        protected Storage()
        {
            Containers = new List<Container>();
        }

        public void Stock(Container container)
        {
            Containers.Add(container);
            OnStocked(container);
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

        protected virtual void OnStocked(Container container)
        {
        }
    }
}