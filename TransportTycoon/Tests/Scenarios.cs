using NFluent;
using TransportTycoon.Domain;
using Xunit;

namespace TransportTycoon.Tests
{
    public class Scenarios
    {
        readonly Storage factory;
        readonly Storage port;
        readonly Storage warehouseA;
        readonly Transporter truck1;
        readonly Transporter ship;
        readonly Storage warehouseB;
        readonly TransporterPool pool;

        public Scenarios()
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

            pool = new TransporterPool(2, "truck", map);
            truck1 = new Transporter("truck 1", map);
            ship = new Transporter("ship", map);
        }

        [Fact]
        public void Move_cargo_from_factory_to_port()
        {
            var first = new Container(port);
            var second = new Container(port);

            factory.Store(first);
            factory.Store(second);

            truck1.Transport(factory);


            Check.That(truck1.Time).IsEqualTo(1);


            truck1.Transport(factory);

            Check.That(first.Time).IsEqualTo(1);
            Check.That(second.Time).IsEqualTo(3);
        }

        [Fact]
        public void Move_cargo_from_port_to_warehouse()
        {
            var first = new Container(warehouseA);
            var second = new Container(warehouseA);

            port.Store(first);
            port.Store(second);

            ship.Transport(port);


            Check.That(ship.Time).IsEqualTo(4);


            ship.Transport(port);

            Check.That(first.Time).IsEqualTo(4);
            Check.That(second.Time).IsEqualTo(12);
        }

        [Fact]
        public void Move_cargo_from_factory_to_warehouse()
        {
            var first = new Container(warehouseA);
            var second = new Container(warehouseA);

            factory.Store(first);
            factory.Store(second);

            truck1.Transport(factory);

            truck1.Transport(factory);

            ship.Transport(port);

            ship.Transport(port);

            Check.That(first.Time).IsEqualTo(5);
            Check.That(second.Time).IsEqualTo(13);
        }

        [Fact]
        public void Automate_route_a()
        {
            var first = new Container(warehouseA);
            var second = new Container(warehouseA);

            factory.Store(first);
            factory.Store(second);

            while (factory.HasInventory)
            {
                truck1.Transport(factory);
            }

            while (port.HasInventory)
            {
                ship.Transport(port);
            }

            Check.That(first.Time).IsEqualTo(5);
            Check.That(second.Time).IsEqualTo(13);
        }

        [Fact]
        public void Automate_route_b()
        {
            var first = new Container(warehouseB);
            var second = new Container(warehouseB);

            factory.Store(first);
            factory.Store(second);

            while (factory.HasInventory)
            {
                truck1.Transport(factory);
            }

            Check.That(first.Time).IsEqualTo(5);
            Check.That(second.Time).IsEqualTo(15);
        }

        [Fact]
        public void Automate_route_b_with_two_trucks()
        {
            var first = new Container(warehouseB);
            var second = new Container(warehouseB);

            factory.Store(first);
            factory.Store(second);

            pool.TransportCargo(factory);

            Check.That(first.Time).IsEqualTo(5);
            Check.That(second.Time).IsEqualTo(5);
        }

        [Fact]
        public void Do_nothing_when_empty()
        {
            pool.TransportCargo(factory);
        }
    }
}