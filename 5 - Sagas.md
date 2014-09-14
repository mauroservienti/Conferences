Using the debugger, at least the first time, to ensure that queues are correctly created

Run projects in the following order:

* NSB05WarehouseService
* NSB05ShippingService
* NSB05Customer
* NSB05OrderManager
* NSB05CustomerCare

Once queues are created correctly endpoints can be run in any order. Generally speaking in production queues are created upfront at deploy time.

## Details

### NSB05Customer

Represents a front-end system that each time is run publishes a new event IShoppingCartCheckedout.

### NSB05OrderManager

Handles all the orchestration around the processing of an order, the creation, the item collection done by the warehouse service and the shipping done by the shipping service.

Also shows how to use timeouts to take decisions in an async world.

### NSB05CustomerCare

Reacts to IOrderProcessingDelayed and IOrderShipped events so to keep track of the order progress and react accordingly.

### NSB05ShippingService

Handles the order shipping, publishing the IOrderShipped at ship time.

### NSB05WarehouseService

Handles the item collection publishing IItemCollected event when collection is completed.