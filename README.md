# ML.NET ASP.NET Core Web API Akka.NET Sample

This sample application utilizes Akka.NET actors to safely scale ML.NET predictions made through HTTP Requests an ASP.NET Core Web API.

## Prerequisites

This sample was build on an Ubuntu 18.04 PC but should work cross-platform on both Windows and Mac.

- [.NET Core SDK 2.1](https://dotnet.microsoft.com/download/dotnet-core/2.1)

## Get Code

In the command prompt enter the following command:

```bash
git clone https://github.com/lqdev/MlNetAspNetAkkaSample.git
```

## Install Dependencies

Navigate to the *WebApi* directory by entering the following command into the command prompt:

```bash
cd WebApi
dotnet restore
```

## Run Application

From the *WebApi* directory enter the following command in the command prompt:

```bash
dotnet run
```

## Make A Prediction

Using a tool like Postman or Insomnia, make a POST HTTP request with `Content-Type` equal to `application/json` to the `http://localhost:5000/api/Predict` endpoint. For the body, use something like the snippet below:

```json
{
  "SepalLength":3.3,
  "SepalWidth":1.6,
  "PetalLength":0.2,
  "PetalWidth":5.1
}
```

If successful, the response should look similar to the output below:

```bash
Iris-virginica
```