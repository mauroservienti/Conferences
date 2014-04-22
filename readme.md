RavenDB
- download RavenDB from http://ravendb.net/download#builds
(the project is built on top of build #2711 but should work with any later version: http://hibernatingrhinos.com/builds/ravendb-unstable-v2.5/2711-unstable)
- Unzip the RavenDB folder and run Start.cmd

NServiceBus
- Ensure MSMQ is installed on your system (Windows Feature), a basic installation is enought (Microsoft Message Queue QUEUE Server Core)
- Download NServiceBus installer from http://particular.net/downloads (no specific version)
- Run the installer, chose advanced install, deselect all except "Prepare My Machine" (and subcomponents)

Run the solution.
- WebApiServer is the web application;
- WebApiServer.backend is the backend service;
- WebApiServer.Lurker is a sample of event listener on the queue