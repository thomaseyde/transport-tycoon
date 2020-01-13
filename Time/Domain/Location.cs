namespace TransportTycoon.Domain
{
    public class Location
    {
        public static readonly Location Factory = new Location("FACTORY");
        public static readonly Location Port = new Location("PORT");
        public static readonly Location A = new Location("A");
        public static readonly Location B = new Location("B");

        private Location(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}