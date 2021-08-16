# UserController
These two Microservice keep track of logged in users as well as the number of users on a server. Assigns each new user a GUID (Globally Unique Identifier) as a key to access the data from a redis store.

The functions in the UserController microservice are the following:
## Add
    POST http://localhost:5010/v1.0/invoke/account-service/method/add HTTP/1.1
    content-type: application/json

    {
        "Name": ""
    }

## Delete
    POST http://localhost:5010/v1.0/invoke/account-service/method/delete HTTP/1.1
    content-type: application/json

    {
        "Id": ""
    }

## Get
    POST http://localhost:5010/v1.0/invoke/account-service/method/get HTTP/1.1
    content-type: application/json

    {
        "Id": ""
    }

## Check
    POST http://localhost:5010/v1.0/invoke/account-service/method/check HTTP/1.1
    
# Prerequisites
#### Installation of Docker 
![Docker](https://github.com/AlexanderAzzopardi/UnitConvertor/blob/main/Saved%20Pictures/DockerLogo.jfif)
> <https://docs.docker.com/engine/install/>

#### Installation of Dapr 
![Dapr](https://github.com/AlexanderAzzopardi/UnitConvertor/blob/main/Saved%20Pictures/DaprLogo.jfif)
> <https://docs.microsoft.com/en-us/dotnet/architecture/dapr-for-net-developers/getting-started>

#### Installation of RestClient
Search **RestClient** in the extension tab and install. 

# Redis Store
### Setting up the Redis Store
When setting up a redis store you need to create a .yaml file 

    apiVersion: dapr.io/v1alpha1
    kind: Component
    metadata:
      name: statestore
      namespace: default
    spec:
      type: state.redis
      version: v1
      metadata:
      - name: redisHost
        value: localhost:6379
      - name: redisPassword
        value: ""
      - name: enableTLS
        value: true # Optional. Allowed: true, false.
      - name: failover
        value: true # Optional. Allowed: true, false.
  
The name of the redis store is set by the tag *name:* and in this case is *statestore*.

### Running the Redis Store
To run the redis store you need you need to run the microservice via dapr.

> cd sample.microservice.rediscache

> dapr run --app-id myapp --dapr-http-port 3500 dotnet run

# User Account
### Running useraccount microservice
To run the useraccount microservice you need to run it via dapr.

> dapr run --app-id "account-service" --app-port "5002" --dapr-grpc-port "50010" --dapr-http-port "5010" -- dotnet run --urls="http://+:5002"
