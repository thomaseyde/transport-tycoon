using TransportTycoon.Domain;
using Xunit;

namespace TransportTycoon.Tests
{
    public class Transport_container
    {
        [Fact]
        public void From_port_to_warehouse()
        {
            var warehouse = new Warehouse(Location.A);
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

            Assert.Equal(4, warehouse.TotalTravelTime());
        }
    }
}