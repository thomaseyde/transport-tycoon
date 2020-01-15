using System.Linq;
using TransportTycoon.Domain;
using TransportTycoon.Time;
using Xunit;

namespace TransportTycoon.Tests
{
    public class Transport_container
    {
        [Fact]
        public void From_port_to_warehouse()
        {
            var clock = new Clock();
            var deliveries = new DeliveryReport();
            var warehouse = new Warehouse(deliveries, Location.A);
            var ship = new Ship();
            var port = new Port();
            var container = new Container(Destination.WarehouseA);

            port.Unload(container);

            Assert.NotEmpty(port.Containers);

            ship.LoadFrom(port, clock.Now);

            Assert.Empty(port.Containers);
            Assert.True(ship.Carries(container));

            for (var i = 0; i < 3; i++)
            {
                clock.Tick();
                ship.Move(clock.Now);
                ship.UnloadTo(warehouse, clock.Now);
                Assert.True(ship.Carries(container));
            }

            clock.Tick();

            ship.Move(clock.Now);
            ship.UnloadTo(warehouse, clock.Now);

            Assert.NotEmpty(warehouse.Containers);
            Assert.False(ship.Carries(container));

            Assert.Equal(4, deliveries.LongestDeliveryTime());
            Assert.Single(deliveries);
        }

        [Fact]
        public void From_factory_to_warehouse()
        {
            var clock = new Clock();
            var deliveries = new DeliveryReport();
            var warehouse = new Warehouse(deliveries, Location.B);
            var truck = new Truck(Location.Factory);
            var factory = new Factory();
            var container = new Container(Destination.WarehouseB);

            factory.Produce(container);

            Assert.NotEmpty(factory.Containers);

            truck.LoadFrom(factory, clock.Now);

            Assert.Empty(factory.Containers);
            Assert.True(truck.Carries(container));

            for (var i = 0; i < 4; i++)
            {
                clock.Tick();
                truck.Move(clock.Now);
                truck.UnloadTo(warehouse, clock.Now);
                Assert.True(truck.Carries(container));
            }

            clock.Tick();
            truck.Move(clock.Now);
            truck.UnloadTo(warehouse, clock.Now);

            Assert.NotEmpty(warehouse.Containers);
            Assert.False(truck.Carries(container));

            Assert.Equal(5, deliveries.LongestDeliveryTime());
            Assert.Single(deliveries);
        }

        [Fact]
        public void From_factory_to_port()
        {
            var clock = new Clock();
            var port = new Port();
            var truck = new Truck(Location.Factory);
            var factory = new Factory();
            var container = new Container(Destination.WarehouseA);

            factory.Produce(container);
            Assert.NotEmpty(factory.Containers);

            truck.LoadFrom(factory, clock.Now);
            Assert.Empty(factory.Containers);
            Assert.True(truck.Carries(container));

            clock.Tick();

            truck.Move(clock.Now);
            
            truck.UnloadTo(port, clock.Now);

            Assert.NotEmpty(port.Containers);
            Assert.False(truck.Carries(container));

            clock.Tick();
        }

        [Theory]
        [InlineData("A", 5)]
        [InlineData("B", 5)]
        [InlineData("AB", 5)]
        [InlineData("ABB", 7)]
        [InlineData("AABABBAB", 29)]
        [InlineData("ABBBABAAABBB", 41)]
        //[InlineData("ABBBABAAABBB", 47)]
        public void All(string destinations, int time)
        {
            var clock = new Clock();
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
                // todo - unload/load/tick/move order is important
                truck1.UnloadTo(port, clock.Now);
                truck1.UnloadTo(warehouseB, clock.Now);

                truck2.UnloadTo(port, clock.Now);
                truck2.UnloadTo(warehouseB, clock.Now);

                ship.UnloadTo(warehouseA, clock.Now);

                truck1.LoadFrom(factory, clock.Now);
                truck2.LoadFrom(factory, clock.Now);
                ship.LoadFrom(port, clock.Now);

                /*
                 * todo - remove clock.Now
                 *
                 * clock.Tick() should preferably happen last, ending the turn
                 * clock.Now should only be passed to Move, but then all state
                 * transitions should happen only there.
                 */
                 
                clock.Tick();

                truck1.Move(clock.Now);
                truck2.Move(clock.Now);
                ship.Move(clock.Now);

                if (iterations++ > 1_000)
                {
                    break;
                }
            }

            Assert.Equal(destinations.Length, deliveries.Count());
            Assert.Equal(time, deliveries.LongestDeliveryTime());
        }
    }
}