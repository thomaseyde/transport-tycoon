using System;
using TransportTycoon.Domain;

namespace TransportTycoon
{
    class Program
    {
        static void Main(string[] args)
        {
            var destinations = args[0].ToUpper();
            var simulation = new Simulation();
            var deliveryTime = simulation.CalculateDeliveryTime(destinations);
            Console.WriteLine(deliveryTime);
        }
    }
}
