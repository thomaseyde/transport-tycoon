using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TransportTycoon.Domain
{
    public class DeliveryReport : IEnumerable
    {
        private readonly List<Container> _containers = new List<Container>();

        public IEnumerator GetEnumerator()
        {
            return _containers.GetEnumerator();
        }

        public void Add(Container container)
        {
            _containers.Add(container);
        }

        public int TotalTravelTime()
        {
            return _containers
                   .Select(container => container.TravelTime)
                   .Select(travelTime => travelTime.Map(time => time.Value))
                   .Sum(time => time.Reduce(0));
        }

        public bool Undelivered(int count)
        {
            return _containers.Count < count;
        }
    }
}