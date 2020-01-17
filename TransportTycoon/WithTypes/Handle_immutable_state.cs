using System;
using Xunit;

namespace TransportTycoon.WithTypes
{
    public class Handle_immutable_state
    {
        [Fact]
        public void Produce_containers()
        {
            var empty = Factory.Build();

            Assert.Equal(0, empty.Count);

            var one = empty.Produce(new Container());
            var two = one.Produce(new Container());

            Assert.Equal(1, one.Count);
            Assert.Equal(2, two.Count);
        }

        [Fact]
        public void Unload_containers_using_tuples()
        {
            /*
             * This may be a proper use of tuples:
             * - We need to update both factory and truck
             * - And it's pretty obvious we do so
             */
            var factory = Factory.Build()
                                 .Produce(new Container())
                                 .Produce(new Container());

            var truck = Truck.Lease();

            Assert.Equal(0, truck.Count);

            (factory, truck) = truck.LoadFrom(factory);

            Assert.Equal(1, factory.Count);
            Assert.Equal(1, truck.Count);

            (factory, truck) = truck.LoadFrom(factory);

            Assert.Equal(0, factory.Count);
            Assert.Equal(2, truck.Count);
        }

        [Fact]
        public void Unload_containers_using_result()
        {
            /*
             * This is ugly. Factory and truck is now hidden inside Result.
             * There is also the problem of where to put LoadContainer(result)
             */
            var factory = Factory.Build()
                             .Produce(new Container())
                             .Produce(new Container());

            var truck = Truck.Lease();

            Assert.Equal(0, truck.Count);

            var result = new Result<Truck>(factory, truck);
            
            result = LoadContainer(result);

            Assert.Equal(1, result.Factory.Count);
            Assert.Equal(1, result.Value.Count);

            result = LoadContainer(result);

            Assert.Equal(0, result.Factory.Count);
            Assert.Equal(2, result.Value.Count);
        }

        #region Ugly...

        private static Result<Truck> LoadContainer(Result<Truck> result)
        {
            return result.Map((factory, truck) =>
            {
                var (f, t) = factory.Ship(truck.Load);
                return new Result<Truck>(f, t);
            });
        }

        class Result<T>
        {
            public Result(Factory factory, T value)
            {
                Factory = factory;
                Value = value;
            }

            public Factory Factory { get; }
            public T Value { get; }

            public TResult Map<TResult>(Func<Factory, T, TResult> selector)
            {
                return selector(Factory, Value);
            }
        }

        #endregion

        abstract class Factory
        {
            public static Factory Build()
            {
                return new Empty();
            }

            public abstract int Count { get; }

            public abstract (Factory factory, T value) Ship<T>(Func<Container, T> selector);

            public Factory Produce(Container container)
            {
                return new Nonempty(container, this);
            }

            class Empty : Factory
            {
                public override int Count => 0;

                public override (Factory factory, T value) Ship<T>(Func<Container, T> selector)
                {
                    return (this, default);
                }
            }

            class Nonempty : Factory
            {
                private readonly Container _container;
                private readonly Factory _previous;

                public Nonempty(Container container, Factory previous)
                {
                    _container = container;
                    _previous = previous;
                }

                public override int Count => _previous.Count + 1;

                public override (Factory factory, T value) Ship<T>(Func<Container, T> selector)
                {
                    return (_previous, selector(_container));
                }
            }
        }

        abstract class Truck
        {
            public abstract int Count { get; }
            public abstract Truck Load(Container container);

            public static Truck Lease()
            {
                return new Unloaded();
            }

            public (Factory factory, Truck truck) LoadFrom(Factory factory)
            {
                return factory.Ship(Load);
            }

            class Unloaded : Truck
            {
                public override int Count => 0;
                public override Truck Load(Container container)
                {
                    return new Loaded(container, this);
                }
            }

            class Loaded : Truck
            {
                private readonly Container _container;
                private readonly Truck _previous;
                public override int Count => _previous.Count + 1;

                public Loaded(Container container, Truck previous)
                {
                    _container = container;
                    _previous = previous;
                }

                public override Truck Load(Container container)
                {
                    return new Loaded(container, this);
                }
            }
        }

        public class Container
        {
        }
    }

}