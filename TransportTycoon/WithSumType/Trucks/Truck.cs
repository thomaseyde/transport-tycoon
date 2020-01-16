using TransportTycoon.WithSumType.Maps;
using TransportTycoon.WithSumType.Stores;
using TransportTycoon.WithSumType.Trucks.Behaviors;

namespace TransportTycoon.WithSumType.Trucks
{
    class Truck
    {
        readonly Factory factory;
        readonly Location origin;
        readonly Port port;

        IBehaviour behaviour;

        public Truck(Factory factory, Port port)
        {
            this.factory = factory;
            this.port = port;
            behaviour = new Loading();
            origin = factory.Location;
        }

        public IBehaviour Loading(Loading loading)
        {
            return factory.Load(container =>
            {
                var destination = container.Destination;
                var arrivalTime = Timetable.ArrivalTime(origin, destination);
                var loadedContainer = container.With(arrivalTime);

                return new Delivering(loadedContainer);
            }, () => behaviour);
        }

        public IBehaviour Delivering(Delivering delivering)
        {
            var container = delivering.Container;
            return new Unloading(container);
        }

        public IBehaviour Unloading(Unloading unloading)
        {
            var container = unloading.Container;
            var destination = container.Destination;
            var arrivalTime = Timetable.ArrivalTime(destination, origin);
            port.Replenish(container);
            return new Returning(arrivalTime);
        }

        public IBehaviour Returning(Returning returning)
        {
            return new Loading();
        }

        public void Move()
        {
            behaviour = behaviour.TransitionFrom(this);
        }

        public T StateAs<T>() where T : IBehaviour
        {
            return (T) behaviour;
        }
    }
}