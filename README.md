# UserController
A microservice which keeps track of how many users are online aswell as the details of those users. The information is stored inside a redis store using dapr and docker kubernetes. It assigns each user a key in the form of a GUID (Globally Unique Identifier) where its information is stored. A list of all the user keys is also stored for ease of access.

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

# User Account
### Running useraccount microservice
To run the useraccount microservice you need to run it via dapr.

> dapr run --app-id "account-service" --app-port "5001" --dapr-grpc-port "50010" --dapr-http-port "5010" -- dotnet run --urls="http://+:5001"

# Commands
## Add
Adds a user to the server and creates a unique id as the key to locate this user. Adds one to the number of users online.

    GET http://localhost:5010/v1.0/invoke/account-service/method/add HTTP/1.1
    content-type: application/json

    {
        "Name": "",
        "Age" : ""
    }

## Delete
Removes a user from the server using the unique id as the key to locate this user. Removes one from the number of users online.

    GET http://localhost:5010/v1.0/invoke/account-service/method/delete HTTP/1.1
    content-type: application/json

    {
        "Id": ""
    }

## Get
Gets a users name from the server using the unique id as the key to locate this user.

    GET http://localhost:5010/v1.0/invoke/account-service/method/get HTTP/1.1
    content-type: application/json

    {
        "Id": ""
    }

## Check
Checks how many users are online.

    GET http://localhost:5010/v1.0/invoke/account-service/method/check HTTP/1.1

## List
Prints out the id's of all the online users.

    GET http://localhost:5010/v1.0/invoke/account-service/method/list HTTP/1.1
    
