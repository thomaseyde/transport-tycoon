namespace TransportTycoon.Domain
{
    public class Port : Storage
    {
        public Port() : base(Location.Port)
        {
        }

        public void Unload(Container container)
        {
            Containers.Add(container);
        }

        public Option<Container> LoadContainer()
        {
            var c = Containers[0];
            Containers.RemoveAt(0);
            return Option.Some(c);
        }
    }
}