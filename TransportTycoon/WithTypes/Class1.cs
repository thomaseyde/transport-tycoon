using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

            factory.Produce(new Container());

            var loading = new Loading(factory, port);
            var moving = loading.Transit();
            var unloading = moving.Transit();
            var returning = unloading.Transit();

            Assert.IsType<Loading>(returning.Transit());
        }

        [Fact]
        public void Load_one_container_from_factory()
        {
            var container = new Container();
            var factory = new Factory();
            var port = new Port();
            var loading = new Loading(factory, port);
            
            factory.Produce(container);
            var moving = (Moving) loading.Transit();

            Assert.NotNull(moving.Container);
        }

        [Fact]
        public void Load_from_empty_factory()
        {
            var factory = new Factory();
            var port = new Port();
            var loading = new Loading(factory, port);
            var next = loading.Transit();

            Assert.IsType<Loading>(next);
        }

        [Fact]
        public void Move_container()
        {
            var container = new Container();
            var factory = new Factory();
            var port = new Port();
            var loading = new Loading(factory, port);
            factory.Produce(container);

            var moving = (Moving) loading.Transit();

            Assert.Equal(Moment.From(1), moving.ArrivalTime);
        }

        [Fact]
        public void Unload_container_to_port()
        {
            var factory = new Factory();
            var port = new Port();

            factory.Produce(new Container());

            var loading = new Loading(factory, port);
            var moving = loading.Transit();
            var unloading = moving.Transit();
            unloading.Transit();

            Assert.Single(port.Inventory);
        }
    }

    public class Port {
        public IEnumerable<Container> Inventory => inventory;
        readonly Queue<Container> inventory = new Queue<Container>();

        public void Replenish(Container container)
        {
            inventory.Enqueue(container);
        }
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

    public abstract class State {
        public abstract State Transit();
    }

    public class Loading : State
    {
        readonly Factory factory;
        Port port;

        public Loading(Factory factory, Port port)
        {
            this.factory = factory;
            this.port = port;
        }

        public override State Transit()
        {
            return factory.LoadContainer<State>(Move, Stay);
        }

        Moving Move(Container container)
        {
            var arrivalTime = Moment.From(1);
            return new Moving(container, factory, port, arrivalTime);
        }

        Loading Stay() => this;
    }

    public class Moving : State
    {
        readonly Factory factory;
        Port port;

        public Moving(Container container, Factory factory, Port port, Moment arrivalTime)
        {
            Container = container;
            this.factory = factory;
            ArrivalTime = arrivalTime;
            this.port = port;
        }

        public override State Transit()
        {
            return new Unloading(factory, port);
        }

        public Container Container { get; }
        public Moment ArrivalTime { get; }
    }

    public class Unloading : State
    {
        readonly Factory factory;
        Port port;

        public Unloading(Factory factory, Port port)
        {
            this.factory = factory;
            this.port = port;
        }

        public override State Transit()
        {
            port.Replenish(new Container());

            return new Returning(factory, port);
        }
    }

    public class Returning : State
    {
        readonly Factory factory;
        Port port;

        public Returning(Factory factory, Port port)
        {
            this.factory = factory;
            this.port = port;
        }

        public override State Transit()
        {
            return new Loading(factory, port);
        }
    }

    public class Container { }
}