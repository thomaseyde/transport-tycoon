namespace TransportTycoon.Domain
{
    public class Truck : Transporter
    {
        public Truck(Location origin) : base(origin)
        {
        }

        public void LoadFrom(Factory factory) => Load(factory);

        public void UnloadTo(Warehouse warehouse) => Unload(warehouse);

        public void UnloadTo(Port port) => Unload(port);
    }
}