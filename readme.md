# Disclaimer
These samples, as most samples, are focused on a specific problem, or a set of, in an oversimplified way.

All the code in these samples is not intended to be in any way production code and is not intended to be a definitive solution, nor the best neither the correct one, to a problem.

All these samples are under constant evolution and are all thought to be used during talks at conferences or at least during courses, this is the main reason of their oversimplification.

---
## Requirements

### RavenDB
* download RavenDB from http://ravendb.net/download#builds
(the project is built on top of build #2711 but should work with any later version: http://hibernatingrhinos.com/builds/ravendb-unstable-v2.5/2711-unstable)
* Unzip the RavenDB folder and run Start.cmd

### NServiceBus
* follow the instructions here: http://docs.particular.net/NServiceBus/preparing-your-machine-to-run-nservicebus
* The RavenDB part is not required to run this sample since it relies on the RavenDB used in the initial step;

### Run the solution.
* WebApiServer is the web application;
* WebApiServer.backend is the backend service;
* WebApiServer.Lurker is a sample of event listener on the queue
