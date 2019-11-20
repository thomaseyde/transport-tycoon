using System.Linq;

namespace TransportTycoon.Domain
{
    public class TransporterPool
    {
        readonly Transporter[] trucks;

        public TransporterPool(int capacity, string type, Map map)
        {
            trucks = Enumerable
                     .Range(1, capacity)
                     .Select(number => new Transporter(type + number, map))
                     .ToArray();
        }

        public void TransportCargo(Storage storage)
        {
            while (storage.HasInventory)
            {
                foreach (var truck in trucks)
                {
                    truck.Transport(storage);
                }
            }
        }
    }
}