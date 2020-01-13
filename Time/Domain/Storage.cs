using System;
using System.Collections.Generic;
using Optional;

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

        public Option<Container> LoadContainer()
        {
            if (Containers.Count == 0)
            {
                return Option.None<Container>();
            }
            var container = Containers[0];
            Containers.RemoveAt(0);
            return Option.Some(container);
        }

        protected virtual void OnStocked(Container container)
        {
        }

        public void LoadContainer(Action<Container> load)
        {
            LoadContainer().MatchSome(load);
        }
    }
}