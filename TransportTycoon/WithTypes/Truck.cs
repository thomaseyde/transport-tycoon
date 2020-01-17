using TransportTycoon.Time;
using TransportTycoon.WithTypes.Maps;
using TransportTycoon.WithTypes.Stores;

namespace TransportTycoon.WithTypes
{
    abstract class Truck
    {
        public static Truck Create(Stores.Factory factory, Port port)
        {
            return new Loading(factory, port);
        }

        public class Loading : Truck
        {
            public Truck Move()
            {
                return factory.Load(Any, None);
            }

            Truck Any(Container container)
            {
                var arrivalTime = Timetable.ArrivalTime(
                    factory.Location,
                    container.Destination);

                return new Delivering(
                    container.With(arrivalTime),
                    factory,
                    port);
            }

            Loading None() => this;

            public Loading(Stores.Factory factory, Port port)
            {
                this.factory = factory;
                this.port = port;
            }

            readonly Stores.Factory factory;
            readonly Port port;
        }

        public class Delivering : Truck
        {
            public Container Container { get; }
            public Moment DeliveryTime => Container.DeliveryTime;

            public Truck Move()
            {
                return new Unloading(Container, port, factory);
            }

            public Delivering(Container container, Stores.Factory factory, Port port)
            {
                Container = container;
                this.port = port;
                this.factory = factory;
            }

            readonly Port port;
            readonly Stores.Factory factory;
        }

        public class Unloading : Truck
        {
            public Container Container { get; }
            public Moment DeliveryTime => Container.DeliveryTime;

            public Truck Move()
            {
                port.Replenish(Container);

                var arrivalTime = Timetable.ArrivalTime(
                    Container.Destination,
                    factory.Location);

                return new Returning(arrivalTime, factory, port);
            }

            public Unloading(Container container, Port port, Stores.Factory factory)
            {
                Container = container;
                this.port = port;
                this.factory = factory;
            }

            readonly Port port;
            readonly Stores.Factory factory;
        }

        public class Returning : Truck
        {
            public Moment ArrivalTime { get; }

            public Truck Move()
            {
                return new Loading(factory, port);
            }

            public Returning(Moment arrivalTime, Stores.Factory factory, Port port)
            {
                ArrivalTime = arrivalTime;
                this.factory = factory;
                this.port = port;
            }

            readonly Stores.Factory factory;
            readonly Port port;
        }

    }
}