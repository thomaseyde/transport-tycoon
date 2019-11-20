namespace TransportTycoon.Domain
{
    public class Transporter
    {
        Storage currentLocation;
        readonly string type;
        readonly Map map;

        public Transporter(string type, Map map)
        {
            this.type = type;
            this.map = map;
            Time = 0;
        }

        public void Transport(Storage pickupPoint)
        {
            Time += map.TravelTimeBetween(currentLocation, pickupPoint);
            currentLocation = pickupPoint;

            pickupPoint.Pickup(DeliverToDrop);
        }

        void DeliverToDrop(Container container)
        {
            var dropPoint = map.NextDropPoint(currentLocation, container.Destination);

            var time = container.SyncronizeTime(Time)
                     + map.TravelTimeBetween(currentLocation, dropPoint);

            dropPoint.Drop(container);
            container.LogDrop(time);
            
            currentLocation = dropPoint;
            Time = time;
        }

        public int Time { get; private set; }

        public override string ToString() => type;
    }
}