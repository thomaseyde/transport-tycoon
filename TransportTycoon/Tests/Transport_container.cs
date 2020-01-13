using TransportTycoon.Domain;
using Xunit;

namespace TransportTycoon.Tests
{
    public class Transport_container
    {
        [Fact]
        public void From_port_to_warehouse()
        {
            var delivered = new ContainerList();
            var warehouse = new Warehouse(Location.A, delivered);
            var ship = new Ship();
            var port = new Port();
            var container = new Container(Destination.WarehouseA);

            port.Unload(container);

            Assert.NotEmpty(port.Containers);

            ship.LoadFrom(port);

            Assert.Empty(port.Containers);
            Assert.Equal(container, ship.Container);

            for (int i = 0; i < 3; i++)
            {
                ship.Move();
                ship.UnloadTo(warehouse);
                Assert.Equal(container, ship.Container);
            }
          
            ship.Move();
            ship.UnloadTo(warehouse);

            Assert.NotEmpty(warehouse.Containers);
            Assert.Equal(Option.None, ship.Container);

            Assert.Equal(4, delivered.TotalTravelTime());
            Assert.Single(delivered);
        }

        [Fact]
        public void From_factory_to_warehouse()
        {
            var delivered = new ContainerList();
            var warehouse = new Warehouse(Location.B, delivered);
            var truck = new Truck(Location.Factory);
            var factory = new Factory();
            var container = new Container(Destination.WarehouseB);

            factory.Produce(container);

            Assert.NotEmpty(factory.Containers);

            truck.LoadFrom(factory);

            Assert.Empty(factory.Containers);
            Assert.Equal(container, truck.Container);

            for (int i = 0; i < 4; i++)
            {
                truck.Move();
                truck.UnloadTo(warehouse);
                Assert.Equal(container, truck.Container);
            }

            truck.Move();
            truck.UnloadTo(warehouse);

            Assert.NotEmpty(warehouse.Containers);
            Assert.Equal(Option.None, truck.Container);

            Assert.Equal(5, delivered.TotalTravelTime());
            Assert.Single(delivered);
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
            Assert.Equal(container, truck.Container);

            truck.Move();
            truck.UnloadTo(port);

            Assert.NotEmpty(port.Containers);
            Assert.Equal(Option.None, truck.Container);
        }
    }
}