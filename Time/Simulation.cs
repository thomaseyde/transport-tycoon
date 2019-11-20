using System;
using System.Linq;
using TransportTycoon.Domain;

namespace TransportTycoon
{
    public class Simulation
    {
        readonly Storage factory;
        readonly Storage port;
        readonly Storage warehouseA;
        readonly Storage warehouseB;
        readonly TransporterPool truckPool;
        readonly TransporterPool shipPool;

        public Simulation()
        {
            factory = new Storage("factory");
            port = new Storage("port");
            warehouseA = new Storage("warehouse a");
            warehouseB = new Storage("warehouse b");

            var map = new Map();
            map.Route(factory, port, warehouseA);
            map.Route(factory, warehouseB);
            map.Time(factory, port, 1);
            map.Time(factory, warehouseB, 5);
            map.Time(port, warehouseA, 4);

            truckPool = new TransporterPool(2, "truck", map);
            shipPool = new TransporterPool(1, "ship", map);
        }


        public int CalculateDeliveryTime(string destinations)
        {
            var containers = destinations
                             .Select(SelectWarehouse)
                             .Select(location => new Container(location))
                             .ToList();

            foreach (var container in containers)
            {
                factory.Store(container);
            }

            truckPool.TransportCargo(factory);
            shipPool.TransportCargo(port);

            return containers.Select(container => container.Time).Max();
        }

        Storage SelectWarehouse(char label)
        {
            switch (label)
            {
                case 'A': return warehouseA;
                case 'B': return warehouseB;
                default: throw new InvalidOperationException();
            }
        }
    }
}