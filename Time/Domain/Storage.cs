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
        }

        public int TotalTravelTime()
        {
            return Containers
                   .Select(container => container.TravelTime)
                   .Select(travelTime => travelTime.Map(time => time.Value))
                   .Sum(time => time.Reduce(0));
        }
    }
}