namespace TransportTycoon.Domain
{
    public class Port : Storage
    {
        public void Unload(Container container) => Containers.Add(container);
    }
}