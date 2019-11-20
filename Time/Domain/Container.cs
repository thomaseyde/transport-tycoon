namespace TransportTycoon.Domain
{
    public class Container
    {
        public Container(Storage destination)
        {
            Destination = destination;
        }

        public void LogDrop(in int time)
        {
            Time = time;
        }

        public Storage Destination { get; }

        public int Time { get; private set; }

        public int SyncronizeTime(int time)
        {
            return Time > time ? Time : time;
        }

    }
}