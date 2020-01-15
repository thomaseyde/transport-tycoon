
using TransportTycoon.Time;

namespace TransportTycoon.Domain
{
    public class Ship : Transporter
    {
        public Ship() : base(Location.Port)
        {
        }

        public void LoadFrom(Port port, Moment currentTime) => Load(port, currentTime);

        public void UnloadTo(Warehouse warehouse, Moment currentTime) => Unload(warehouse, currentTime);
    }
}