namespace TransportTycoon.Domain
{
    public class Truck : Transporter
    {
        public Truck(Location origin) : base(origin)
        {
        }

        public void LoadFrom(Factory factory, Time currentTime) => Load(factory, currentTime);

        public void UnloadTo(Warehouse warehouse, Time currentTime) => Unload(warehouse, currentTime);

        public void UnloadTo(Port port, Time currentTime) => Unload(port, currentTime);
    }
}