﻿using System.Linq;
using TransportTycoon.Domain;
using Xunit;

namespace TransportTycoon.Tests
{
    public class Transport_container
    {
        [Fact]
        public void From_port_to_warehouse()
        {
            var deliveries = new DeliveryReport();
            var warehouse = new Warehouse(deliveries, Location.A);
            var ship = new Ship();
            var port = new Port();
            var container = new Container(Destination.WarehouseA);

            port.Unload(container);

            Assert.NotEmpty(port.Containers);

            ship.LoadFrom(port);

            Assert.Empty(port.Containers);
            Assert.True(ship.Carries(container));

            for (var i = 0; i < 3; i++)
            {
                ship.Move();
                ship.UnloadTo(warehouse);
                Assert.True(ship.Carries(container));
            }
          
            ship.Move();
            ship.UnloadTo(warehouse);

            Assert.NotEmpty(warehouse.Containers);
            Assert.False(ship.Carries(container));

            Assert.Equal(4, deliveries.TotalTravelTime());
            Assert.Single(deliveries);
        }

        [Fact]
        public void From_factory_to_warehouse()
        {
            var deliveries = new DeliveryReport();
            var warehouse = new Warehouse(deliveries, Location.B);
            var truck = new Truck(Location.Factory);
            var factory = new Factory();
            var container = new Container(Destination.WarehouseB);

            factory.Produce(container);

            Assert.NotEmpty(factory.Containers);

            truck.LoadFrom(factory);

            Assert.Empty(factory.Containers);
            Assert.True(truck.Carries(container));

            for (var i = 0; i < 4; i++)
            {
                truck.Move();
                truck.UnloadTo(warehouse);
                Assert.True(truck.Carries(container));
            }

            truck.Move();
            truck.UnloadTo(warehouse);

            Assert.NotEmpty(warehouse.Containers);
            Assert.False(truck.Carries(container));

            Assert.Equal(5, deliveries.TotalTravelTime());
            Assert.Single(deliveries);
        }

        [Fact]
        public void From_factory_to_port()
        {
            var port = new Port();
            var truck = new Truck(Location.Factory);
            var factory = new Factory();
            var container = new Container(Destination.WarehouseA);

            factory.Produce(container);

            Assert.NotEmpty(factory.Containers);

            Assert.True(truck.AtOrigin());
            
            truck.LoadFrom(factory);

            Assert.Empty(factory.Containers);
            Assert.True(truck.Carries(container));

            truck.Move();
            truck.UnloadTo(port);

            Assert.NotEmpty(port.Containers);
            Assert.False(truck.Carries(container));
            Assert.False(truck.AtOrigin());

            truck.Move();
            Assert.True(truck.AtOrigin());

        }

        [Theory]
        [InlineData("A", 5)]
        [InlineData("B", 5)]
        [InlineData("AB", 5)]
        //[InlineData("ABB", 7)]
        public void All(string destinations, int time)
        {
            var deliveries = new DeliveryReport();

            var factory = new Factory();
            var port = new Port();
            var warehouseA = new Warehouse(deliveries, Location.A);
            var warehouseB = new Warehouse(deliveries, Location.B);
            
            var truck1 = new Truck(Location.Factory);
            var truck2 = new Truck(Location.Factory);
            var ship = new Ship();

            foreach (var destination in destinations)
            {
                Destination d = null;
                if (destination=='A')
                {
                    d = Destination.WarehouseA;
                }else if (destination == 'B')
                {
                    d = Destination.WarehouseB;
                }

                if (d != null) factory.Produce(new Container(d));
            }

            int iterations = 0;
            while (deliveries.Undelivered(destinations.Length))
            {
                truck1.LoadFrom(factory);
                truck1.Move();
                
                truck1.UnloadTo(port);
                truck1.UnloadTo(warehouseB);

                truck2.LoadFrom(factory);
                truck2.Move();
                truck2.UnloadTo(port);
                truck2.UnloadTo(warehouseB);

                ship.LoadFrom(port);
                ship.Move();
                ship.UnloadTo(warehouseA);

                if (iterations++ > 100)
                {
                    break;
                }
            }

            Assert.Equal(destinations.Length, deliveries.Count());
            Assert.Equal(time, deliveries.TotalTravelTime());
        }
    }
}