
```
Container = ProducedContainer or TransportedContainer

ProducedContainer = 
    Destination

TransportedContainer = 
    Destination and
    TransportationTime

Transporter = Ship or Truck

Ship =
    Cargo
    Destination
    TravelTime
    WaitTime

Truck =
    Cargo
    Destination
    TravelTime
    WaitTime

Cargo = 
    TransportedContainer

Destination = 
    string of "A", "B", "PORT"

TravelTime = 
    int > 0

TravelTime = 
    int > 0

```
