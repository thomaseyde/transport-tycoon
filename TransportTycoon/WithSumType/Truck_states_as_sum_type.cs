using TransportTycoon.Time;
using TransportTycoon.WithSumType.Stores;
using TransportTycoon.WithSumType.Trucks.Behaviors;
using Xunit;

namespace TransportTycoon.WithSumType
{
    public class Truck_states_as_sum_type
    {
        [Fact]
        public void Test()
        {
            var factory = new Factory();
            var port = new Port();

            var loading = (Loading)Truck.Create(factory, port);

            var delivering = (Delivering) loading.Move();

            Assert.NotNull(delivering.Container);
            Assert.Equal(Moment.From(1), delivering.DeliveryTime);

            var unloading = (Unloading) delivering.Move();

            Assert.Equal(Moment.From(1), unloading.DeliveryTime);

            var returning = (Returning) unloading.Move();

            Assert.True(port.Holds(unloading.Container));

            Assert.Equal(Moment.From(2), returning.ArrivalTime);

            var loadingAgain = (Loading) returning.Move();

            Assert.IsType<Loading>(loadingAgain);
        }
    }
}