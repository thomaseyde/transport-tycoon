namespace TransportTycoon.Domain
{
    public class Location
    {
        public static readonly Location Factory = new Location("FACTORY");
        public static readonly Location Port = new Location("PORT");
        public static readonly Location A = new Location("A");
        public static readonly Location B = new Location("B");
        readonly string name;

        public Location NextLocationTowards(Location destination) =>
            this == Factory && destination == A
                ? Port
                : destination;

        Location(string name) => this.name = name;

        public override string ToString() => name;
    }
}