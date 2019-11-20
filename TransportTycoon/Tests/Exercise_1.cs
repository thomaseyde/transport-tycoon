using NFluent;
using Xunit;

namespace TransportTycoon.Tests
{
    public class Exercise_1
    {
        [Theory]
        [InlineData("A", 5)]
        [InlineData("AB", 5)]
        [InlineData("BB", 5)]
        [InlineData("ABB", 7)]
        [InlineData("AABABBAB", 29)]
        [InlineData("ABBBABAAABBB", 47)]
        public void Time_transports(string destinations, int time)
        {
            var simulation = new Simulation();
            var deliveryTime = simulation.CalculateDeliveryTime(destinations);
            Check.That(deliveryTime).IsEqualTo(time);
        }
    }
}