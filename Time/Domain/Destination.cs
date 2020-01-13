namespace TransportTycoon.Domain
{
    public class Destination
    {
        public static readonly Destination WarehouseA =
            new Destination(Location.A);

        public static readonly Destination WarehouseB =
            new Destination(Location.B);

        public Location Location { get; }

        Destination(Location location) 
            => Location = location;
    }
}