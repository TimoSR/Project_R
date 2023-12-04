# Project_R

Deployed Services
* https://authentication-service-s4qgcf5egq-ew.a.run.app/
* https://user-management-service-s4qgcf5egq-ew.a.run.app/
* https://game-cms-service-s4qgcf5egq-ew.a.run.app

To Run the Code:
'This step requires the env file, that we don't provide.'

Requires .Net 7 and .Net 8 SDK's

Micro Service Frame (With Examples):

* x_service

Client:
'Use this program to test the interaction with the services'
Headless_Client 

Go into the application folder.

* dotnet restore
* dotnet run

Test endpoints at:

Register

https://user-management-service-s4qgcf5egq-ew.a.run.app/graphql/

```jsx
mutation {
  registerUser(newUserDto: {
    email: "selma@gmail.com",
    userName: "00tir2009",
    password: "Software1!"
  }) {
    isSuccess
    messages
    errorType
  }
}
```

Login

https://authentication-service-s4qgcf5egq-ew.a.run.app/graphql/

```jsx
mutation {
  login(requestDto: {
    email: "selma@gmail.com",
    password: "Software1!"
  }) {
    data {
      accessToken
      refreshToken
    }
    isSuccess
    messages
    errorType
  }
}
```