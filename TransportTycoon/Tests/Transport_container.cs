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
            var warehouse = new Warehouse(deliveries);
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
            var warehouse = new Warehouse(deliveries);
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

            truck.LoadFrom(factory);

            Assert.Empty(factory.Containers);
            Assert.True(truck.Carries(container));

            truck.Move();
            truck.UnloadTo(port);

            Assert.NotEmpty(port.Containers);
            Assert.False(truck.Carries(container));
        }
    }
}