namespace TransportTycoon.Domain
{
    public class Location
    {
        public static readonly Location Factory = new Location();
        public static readonly Location Port = new Location();
        public static readonly Location A = new Location();
        public static readonly Location B = new Location();

        public Location NextLocationTowards(Location destination)
        {
            return this == Factory && destination == A
                ? Port
                : destination;
        }

        Location()
        {
        }
    }
}