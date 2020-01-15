using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using TransportTycoon.Time;
using Xunit;

namespace TransportTycoon.WithTypes
{
    public class Class1
    {
        /*
         * Ship and Truck are Transports
         * Loading site for truck is always factory. For ship, always port.
         * Unloading site for ship is always warehouse a
         * Unloading site for truck is port or warehouse b, decided by container's destination
         *
         * Truck or ship can be one of Loading, Moving or Unloading
         *
         * Loading =
         *  Site
         *
         * Delivering =
         *  Container
         *  Destination
         *  ArrivalTime
         *
         * Unloading
         *  Site
         *
         * Returning
         *  Site
         *  ArrivalTime
         */

        [Fact]
        public void State_transitions()
        {
            var factory = new Factory();
            var port = new Port();

            factory.Produce(new Container(port));

            var loading = new Loading(factory, () => new Unloading(factory, port));
            var moving = loading.Transit();
            var unloading = moving.Transit();
            var returning = unloading.Transit();

            Assert.IsType<Loading>(returning.Transit());
        }

        [Fact]
        public void Load_one_container_from_factory()
        {
            var port = new Port();
            var factory = new Factory();
            var loading = new Loading(factory, () => new Unloading(factory, port));
            var container = new Container(port);

            factory.Produce(container);
            var moving = (Moving) loading.Transit();

            Assert.NotNull(moving.Container);
        }

        [Fact]
        public void Load_from_empty_factory()
        {
            var factory = new Factory();
            var port = new Port();
            var loading = new Loading(factory, () => new Unloading(factory, port));
            var next = loading.Transit();

            Assert.IsType<Loading>(next);
        }

        [Fact]
        public void Move_container()
        {
            var factory = new Factory();
            var port = new Port();
            var loading = new Loading(factory, () => new Unloading(factory, port));
            var container = new Container(port);
            factory.Produce(container);

            var moving = (Moving) loading.Transit();

            Assert.Equal(Moment.From(1), moving.ArrivalTime);
        }

        [Fact]
        public void Unload_container_to_port()
        {
            //todo - simplify state design. Too many dependencies.
            var factory = new Factory();
            var port = new Port();

            factory.Produce(new Container(port));

            var loading = new Loading(factory, () => new Unloading(factory, port));
            var moving = loading.Transit();
            var unloading = moving.Transit();
            unloading.Transit();

            Assert.Single(port.Inventory);
        }

        [Fact]
        public void Unload_container_to_b()
        {
            var factory = new Factory();
            var b = new WarehouseB();

            factory.Produce(new Container(b));

            var loading = new Loading(factory, () => new Unloading(factory, b));
            var moving = loading.Transit();
            var unloading = moving.Transit();
            unloading.Transit();

            Assert.Single(b.Inventory);
        }
    }

    public abstract class Destination 
    {
        public IEnumerable<Container> Inventory => inventory;
        readonly Queue<Container> inventory = new Queue<Container>();

        public void Replenish(Container container)
        {
            inventory.Enqueue(container);
        }
    }

    public class WarehouseB : Destination
    {
    }

    public class Port : Destination
    {
    }

    public class Factory
    {
        public void Produce(Container container)
        {
            inventory.Enqueue(container);
        }

        public T LoadContainer<T>(Func<Container, T> one, Func<T> none)
        {
            return inventory.Any()
                ? one(inventory.Dequeue())
                : none();
        }

        readonly Queue<Container> inventory = new Queue<Container>();
    }

    public abstract class State
    {
        public abstract State Transit();
    }

    public class Loading : State
    {
        readonly Factory factory;
        readonly Func<State> unloading;

        public Loading(Factory factory, Func<Unloading> unloading)
        {
            this.factory = factory;
            this.unloading = unloading;
        }

        public override State Transit()
        {
            return factory.LoadContainer<State>(Move, Stay);
        }

        Moving Move(Container container)
        {
            //todo - derive from current time, container's travel time
            var arrivalTime = Moment.From(1);
            var unloading = this.unloading;
            return new Moving(container, arrivalTime, unloading);
        }

        Loading Stay() => this;
    }

    public class Moving : State
    {
        readonly Func<State> unloading;

        public Moving(Container container, Moment arrivalTime, Func<State> unloading)
        {
            Container = container;
            ArrivalTime = arrivalTime;
            this.unloading = unloading;
        }

        public override State Transit()
        {
            return unloading();
        }

        public Container Container { get; }
        public Moment ArrivalTime { get; }
    }

    public class Unloading : State
    {
        readonly Factory factory;
        readonly Destination destination;

        public Unloading(Factory factory, Destination destination)
        {
            this.factory = factory;
            this.destination = destination;
        }

        public override State Transit()
        {
            var container = new Container(destination);

            destination.Replenish(container);

            return new Returning(factory);
        }
    }

    public class Returning : State
    {
        readonly Factory factory;

        public Returning(Factory factory)
        {
            this.factory = factory;
        }

        public override State Transit()
        {
            //todo - move to Loading.Move
            Func<Unloading> unloading = () => new Unloading(factory, null);
            return new Loading(factory, unloading);
        }
    }

    public class Container
    {
        public Container(Destination destination)
        {
            Destination = destination;
        }

        public Destination Destination { get; }
    }
}