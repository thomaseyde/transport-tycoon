using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TransportTycoon.Domain
{
    public class DeliveryReport : IEnumerable<object>
    {
        readonly List<Container> containers = new List<Container>();

      
        public void Add(Container container)
        {
            containers.Add(container);
        }

        public int TotalTravelTime()
        {
            return containers
                   .Select(container => container.TravelTime)
                   .Select(travelTime => travelTime)
                   .Sum(time => time.Value);
        }

        public bool Undelivered(int count)
        {
            return containers.Count < count;
        }

        public IEnumerator<object> GetEnumerator()
        {
            return containers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}