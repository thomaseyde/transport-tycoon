namespace TransportTycoon.Domain
{
    public class Port : Storage
    {
        public Port() : base(Location.Port) { }
        public void Unload(Container container) => Containers.Add(container);
    }
}