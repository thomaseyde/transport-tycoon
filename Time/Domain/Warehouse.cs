
namespace TransportTycoon.Domain
{
    public class Warehouse : Storage
    {
        readonly DeliveryReport deliveries;

        public Warehouse(DeliveryReport deliveries) 
            => this.deliveries = deliveries;

        protected override void OnStocked(Container container) 
            => deliveries.Add(container);
    }
}