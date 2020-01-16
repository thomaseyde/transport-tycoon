namespace TransportTycoon.WithSumType.Trucks.Behaviors
{
    class Loading : IBehaviour
    {
        public IBehaviour TransitionFrom(Truck current)
        {
            return current.Loading(this);
        }
    }
}