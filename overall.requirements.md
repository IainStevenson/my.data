# Overall Requiremetns  

## Coding

Unless noted otherwise, coding will be in C# and BASH shell scripting as required.


## Security

Data should be secured in transit and at rest.

* Transit security should be covered by TLS (SSL).
* At rest data should be encrypted in the storage service.
* Identity security should be encrypted at rest and in transit.


Public facing environments should use services such as LetsEncrypt for certificates for thier individual domains, and use separate certificates for transport and at rest encryption purposes.


### Self signed certificates

Self-signed certificates are acceptable for local environments that have no public facing interfaces.



## Testing

All code should be tested at least once.

Unit tests are to be used.

Service integration testing is requried to ensure public interfaces work as designed.

API testing will be done using POSTMAN with the copllection saved as a file withing the solution.


## Build and deployment

The solution should run natively on docker for windows on a development host, and has a build and deployment support for 

```
QNAP QTS 4.5.1 NAS storage system.
```


## Configuration

Working configurations will be required for the local development host and a sample NAS environment.

The following configuration names should be used.

```
DEV
NAS
CLOUD
```

## Services

The following core services should be created

### Core services

* my.data.identity
* my.data.api
* my.data.storage

### Client services

* my.data.bandwidth
* my.data.ui


### Service dependencies

The following service, depends on ( --> ), relationships should be declared in the docker compose project; 

```
my.data.bandwidth	--> my.data.identity
my.data.bandwidth	--> my.data.api
my.data.ui			--> my.data.identity
my.data.ui			--> my.data.api
my.data.api			--> my.data.identity
my.data.identity	--> my.data.storage
my.data.api			--> my.data.storage
```

Note: A single common storage service is included for convenience and reduces resource consumption only. Ideally it should be segregated as an instance per dependency for performance, security and data segregation reasons.

