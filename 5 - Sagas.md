Using the debugger, at least the first time, to ensure that queues are correctly created

Run projects in the following order:

* NSB05WarehouseService
* NSB05ShippingService
* NSB05Customer
* NSB05OrderManager
* NSB05CustomerCare

## Details

### NSB05Customer

Represents a front-end system that each time is run publishes a new event IShoppingCartCheckedout

Once queues are created correctly endpoints can be run in any order. Generally speaking in production queues are created upfront at deploy time.