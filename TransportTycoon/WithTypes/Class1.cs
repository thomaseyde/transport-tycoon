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
        public void State_transisions()
        {
            var factory = new Factory();

            var loading = new Loading(factory);

            var moving = loading.Transit();

            var unloading = moving.Transit();

            var returning = unloading.Transit();

            Assert.IsType<Loading>(returning.Transit());
        }

        [Fact]
        public void Load_one_container_from_factory()
        {
            var factory = new Factory();

            var loading = new Loading(factory);

            var moving = loading.Transit();

            Assert.NotNull(moving.Container);
        }
    }

    public class Factory
    {
        public Container LoadContainer()
        {
            return new Container();
        }
    }

    public class Loading
    {
        readonly Factory factory;

        public Loading(Factory factory)
        {
            this.factory = factory;
        }

        public Moving Transit()
        {
            return new Moving(factory.LoadContainer(), factory);
        }
    }

    public class Moving
    {
        readonly Factory factory;

        public Moving(Container container, Factory factory)
        {
            Container = container;
            this.factory = factory;
        }

        public Unloading Transit()
        {
            return new Unloading(factory);
        }

        public Container Container { get; }
    }

    public class Unloading
    {
        readonly Factory factory;

        public Unloading(Factory factory)
        {
            this.factory = factory;
        }

        public Returning Transit()
        {
            return new Returning(factory);
        }
    }

    public class Returning
    {
        readonly Factory factory;

        public Returning(Factory factory)
        {
            this.factory = factory;
        }

        public Loading Transit()
        {
            return new Loading(factory);
        }
    }

    public class Container { }
}