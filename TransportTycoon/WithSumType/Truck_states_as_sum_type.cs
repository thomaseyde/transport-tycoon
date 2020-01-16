using TransportTycoon.Time;
using TransportTycoon.WithSumType.Trucks;
using TransportTycoon.WithSumType.Trucks.Behaviors;
using Xunit;

namespace TransportTycoon.WithSumType
{
    public class Truck_states_as_sum_type
    {
        [Fact]
        public void Test()
        {
            var factory = new Stores.Factory();
            var port = new Stores.Port();

            var truck = new Truck(factory, port);

            truck.Move();

            var delivering = truck.StateAs<Delivering>();

            Assert.NotNull(delivering.Container);
            Assert.Equal(Moment.From(1), delivering.Container.DeliveryTime);

            truck.Move();

            var unloading = truck.StateAs<Unloading>();
            var deliveredContainer = unloading.Container;

            Assert.Equal(Moment.From(1), deliveredContainer.DeliveryTime);

            truck.Move();

            Assert.True(port.Holds(deliveredContainer));

            var returning = truck.StateAs<Returning>();

            Assert.IsType<Returning>(returning);
            Assert.Equal(Moment.From(2), returning.ArrivalTime);

            truck.Move();

            var loading = truck.StateAs<Loading>();

            Assert.IsType<Loading>(loading);
        }
    }
}