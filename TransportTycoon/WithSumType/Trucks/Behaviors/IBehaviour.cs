namespace TransportTycoon.WithSumType.Trucks.Behaviors
{
    internal interface IBehaviour
    {
        IBehaviour TransitionFrom(Truck current);
    }
}