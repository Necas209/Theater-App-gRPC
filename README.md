# Theater App - gRPC

App developed for Distributed Systems class, for selling and buying theater tickets.

Communication between client and server was achieved using the .NET implementation of the gRPC framework, an open-source RPC framework developed by Google.

Solution is comprised of three projects:
- __ClientApp__, client-side application developed in C#, using WPF and following the _MVVM_ architecture
- __Server__, gRPC service, featuring all of the needed server-side logic for database interaction and client request processing
- __GrpcLibrary__, a shared library containing the database-equivalent class models, shared services and protocolo buffer (_protobuf_) definitions

The app supports three types of users:
- __Admin__, for user and log management
- __Manager__, which includes the back-office for all theater-related management
- __Client__, regular user, with options to search for theaters, shows and sessions, buy tickets and add funds to a virtual account