Using the debugger, at least the first time, to ensure that queues are correctly created

Run projects in the following order:

* NSB04SampleReceiver
* NSB04SampleSender

To test the scale-out scenario, run the NSB04SampleSender multiple times and run multiple times the NSB04SampleReceiver, each instance can be shutdown and started up at any time without any issue and without losing any message.

Once queues are created correctly endpoints can be run in any order. Generally speaking in production queues are created upfront at deploy time.