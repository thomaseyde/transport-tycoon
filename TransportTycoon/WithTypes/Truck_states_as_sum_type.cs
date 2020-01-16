using System;
using System.Collections.Generic;
using TransportTycoon.Time;
using Xunit;

namespace TransportTycoon.WithTypes
{
    public class Truck_states_as_sum_type
    {
        [Fact]
        public void Test()
        {
            var factory = new Factory();
            var port = new Port();

            var truck = new Truck(factory, port);

            truck.Move();

            var delivering = truck.StateAs<State.Delivering>();

            Assert.NotNull(delivering.Container);
            Assert.Equal(Moment.From(1), delivering.ArrivalTime);

            truck.Move();

            var unloading = truck.StateAs<State.Unloading>();
            var deliveredContainer = unloading.Container;

            Assert.Equal(Moment.From(1), deliveredContainer.DeliveryTime);

            truck.Move();

            Assert.True(port.Holds(deliveredContainer));

            var returning = truck.StateAs<State.Returning>();

            Assert.IsType<State.Returning>(returning);
            Assert.Equal(Moment.From(2), returning.ArrivalTime);

            truck.Move();

            var loading = truck.StateAs<State.Loading>();

            Assert.IsType<State.Loading>(loading);

        }

        class Truck
        {
            public void Move()
            {
                state = state.Match(
                    Loading, 
                    Delivering, 
                    Unloading, 
                    Returning);
            }

            private State Loading(State.Loading loading)
            {
                State Load(Container container)
                {
                    var arrivalTime = Timetable.ArrivalTime(origin, container.Destination);
                    return new State.Delivering(container, arrivalTime);
                }

                State Stay() => state;

                return factory.Load(Load, Stay);
            }

            private State Delivering(State.Delivering delivering)
            {
                var container = delivering.Container.With(delivering.ArrivalTime);
                return new State.Unloading(container);
            }

            private State Unloading(State.Unloading unloading)
            {
                var container = unloading.Container;
                var arrivalTime = Timetable.ArrivalTime(container.Destination, origin);
                port.Replenish(container);
                return new State.Returning(arrivalTime);
            }

            private State Returning(State.Returning returning)
            {
                return new State.Loading();
            }

            public Truck(Factory factory, Port port)
            {
                this.factory = factory;
                this.port = port;
                state = new State.Loading();
                origin = factory.Location;
            }

            readonly Factory factory;
            readonly Port port;
            readonly Location origin;

            State state;

            public T StateAs<T>() where T : State
            {
                return (T)state;
            }
        }

        abstract class State
        {
            public abstract T Match<T>(
                Func<Loading, T> loading,
                Func<Delivering, T> delivering,
                Func<Unloading, T> unloading,
                Func<Returning, T> returning);

            public class Loading : State
            {
                public override T Match<T>(
                    Func<Loading, T> loading,
                    Func<Delivering, T> delivering,
                    Func<Unloading, T> unloading,
                    Func<Returning, T> returning)
                {
                    return loading(this);
                }
            }

            public class Delivering : State
            {
                public Delivering(Container container, Moment arrivalTime)
                {
                    Container = container;
                    ArrivalTime = arrivalTime;
                }

                public Container Container { get; }
                public Moment ArrivalTime { get; }

                public override T Match<T>(
                    Func<Loading, T> loading,
                    Func<Delivering, T> delivering,
                    Func<Unloading, T> unloading,
                    Func<Returning, T> returning)
                {
                    return delivering(this);
                }
            }

            public class Unloading : State
            {
                public Unloading(Container container)
                {
                    Container = container;
                }

                public Container Container { get; }

                public override T Match<T>(
                    Func<Loading, T> loading,
                    Func<Delivering, T> delivering,
                    Func<Unloading, T> unloading,
                    Func<Returning, T> returning)
                {
                    return unloading(this);
                }
            }

            public class Returning : State
            {
                public Returning(Moment arrivalTime)
                {
                    ArrivalTime = arrivalTime;
                }

                public Location Origin { get; private set; }
                public Moment ArrivalTime { get; }

                public override T Match<T>(
                    Func<Loading, T> loading,
                    Func<Delivering, T> delivering,
                    Func<Unloading, T> unloading,
                    Func<Returning, T> returning)
                {
                    return returning(this);
                }
            }
        }

        class Container
        {
            public Container(Moment deliveryTime, Location destination)
            {
                DeliveryTime = deliveryTime;
                Destination = destination;
            }

            public Moment DeliveryTime { get; }
            public Location Destination { get; }

            public Container With(Moment deliveryTime)
            {
                return new Container(deliveryTime, Destination);
            }
        }

        class Location
        {
            public static readonly Location Factory = new Location();
            public static readonly Location Port = new Location();

            private Location()
            {
            }
        }

        class Factory
        {
            public Factory()
            {
                Location = Location.Factory;
            }

            public Location Location { get; }

            public T Load<T>(Func<Container, T> one, Func<T> none)
            {
                return one(new Container(Moment.Zero, Location.Port));
            }
        }

        class Port
        {
            readonly Queue<Container> inventory = new Queue<Container>();

            public bool Holds(Container container)
            {
                return inventory.Contains(container);
            }

            public void Replenish(Container container)
            {
                inventory.Enqueue(container);
            }
        }

        class Timetable
        {
            public static Moment ArrivalTime(
                Location origin,
                Location destination)
            {
                //todo - need a clock

                if (origin == Location.Factory)
                {
                    return Moment.From(1);
                }

                return Moment.From(2);
            }
        }
    }
}