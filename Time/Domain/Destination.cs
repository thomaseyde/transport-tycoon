namespace TransportTycoon.Domain
{
    public class Destination
    {
        public static readonly Destination Port = new Destination(Location.Port);
        public static readonly Destination WarehouseA = new Destination(Location.A);
        public static readonly Destination WarehouseB = new Destination(Location.B);

        private Destination(Location location)
        {
            Location = location;
        }

        public Location Location { get; }
    }
}