
namespace TransportTycoon.Domain
{
    public class Warehouse : Storage
    {
        private readonly DeliveryReport _deliveries;

        public Warehouse(DeliveryReport deliveries) 
            => _deliveries = deliveries;

        protected override void OnStocked(Container container) 
            => _deliveries.Add(container);
    }
}