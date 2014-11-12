How to setup your machine to run the sample:

- Configure your Visual Studio to access a MyGet repository at: https://www.myget.org/gallery/ravendb3
- Download the pre-configured RavenDB servers from: http://1drv.ms/1uahGJG
- In the zip there are 4 RavenDB standalone servers pre-configured to listen on 4 different ports: 
  * 8381
  * 8382
  * 8383
  * 8384

- Extract the zip on your disk, e.g. c:\temp; be sure to extract to a path with no spaces
- Run the included Start.cmd;
- if requested confirm the UAC prompt that ask to setup URL ACL
- Confirm that 4 console applications, hosting 4 different RavenDB servers are running;

Open the solution and play with the included samples.