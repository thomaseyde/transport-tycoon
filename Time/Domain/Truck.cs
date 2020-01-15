using TransportTycoon.Time;

namespace TransportTycoon.Domain
{
    public class Truck : Transporter
    {
        public Truck(Location origin) : base(origin)
        {
        }

        public void LoadFrom(Factory factory, Moment currentTime) => Load(factory, currentTime);

        public void UnloadTo(Warehouse warehouse, Moment currentTime) => Unload(warehouse, currentTime);

        public void UnloadTo(Port port, Moment currentTime) => Unload(port, currentTime);
    }
}