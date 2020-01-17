using TransportTycoon.Time;
using TransportTycoon.WithTypes.Stores;
using Xunit;

namespace TransportTycoon.WithTypes
{
    public class Truck_states_as_sum_type
    {
        [Fact]
        public void Test()
        {
            var factory = new Stores.Factory();
            var port = new Port();

            var loading = (Truck.Loading)Truck.Create(factory, port);

            var delivering = (Truck.Delivering) loading.Move();

            Assert.NotNull(delivering.Container);
            Assert.Equal(Moment.From(1), delivering.DeliveryTime);

            var unloading = (Truck.Unloading) delivering.Move();

            Assert.Equal(Moment.From(1), unloading.DeliveryTime);

            var returning = (Truck.Returning) unloading.Move();

            Assert.True(port.Holds(unloading.Container));

            Assert.Equal(Moment.From(2), returning.ArrivalTime);

            var loadingAgain = (Truck.Loading) returning.Move();

            Assert.NotNull(loadingAgain);
        }
    }
}