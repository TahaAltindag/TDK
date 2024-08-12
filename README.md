
# Taha Development Kit (TDK)

## Overview

The **Taha Development Kit (TDK)** is a robust Unity-based framework designed to streamline development by providing a set of reusable services, utilities, and design patterns. The TDK emphasizes the use of SOLID principles and design patterns, making your codebase more maintainable, scalable, and easy to understand.

## Table of Contents

- [Getting Started](#getting-started)
- [Dependencies](#dependencies)
- [Core](#core)
  - [Service Locator](#service-locator)
  - [EventBus](#eventbus)
  - [Utilities](#utilities)
- [Common](#common)
  - [Bootstrapper](#bootstrapper)
  - [Universal Technic Service](#universal-technic-service)
  - [Remote Config Service](#remote-config-service)
  - [Unity Cloud Service](#unity-cloud-service)
  - [Internet Connection Checking Service](#internet-connection-checking-service)
  - [Unity Authentication Service](#unity-authentication-service)
  - [Analytic Service](#analytic-service)
- [Examples](#examples)

## Getting Started

To get started with the TDK, clone the repository and ensure that you have the required dependencies installed in your Unity project.

```bash
git clone https://github.com/yourusername/tdk.git
```

### Prerequisites

- Unity 2020.3 or later
- Unity Package Manager (for managing dependencies)
- A valid Unity account for accessing Unity Gaming Services

### Installation

1. **Clone the repository** to your local machine.
2. **Open the project in Unity** and let Unity import all assets and dependencies.
3. **Configure the services** as needed in your Unity scenes.

## Dependencies

The TDK leverages several third-party packages and Unity services to enhance functionality:

- **Odin Inspector**: Custom Unity editor tools.
- **UniTask**: Asynchronous operations using async/await patterns.
- **Unity Analytics**: Real-time data insights and analytics.
- **FMOD**: Advanced audio features like 3D sound and event-based audio.
- **Addressables**: Efficient asset management and loading.
- **Remote Config**: Dynamic configuration and A/B testing.
- **DOTween**: Animation engine for tweening Unity components.

## Core

### Service Locator

The Service Locator pattern is central to the TDK, providing a global access point for services used throughout the application.

- **Service Locator**: A system that allows easy access to various services registered at runtime.
- **Bootstrapper**: A component responsible for registering and initializing global services during the application startup.

#### Example Usage

```csharp
ServiceLocator.Global
    .Register<IInternetConnectionCheckingService>(new InternetConnectionCheckingService())
    .Register<IUnityCloudService>(new UnityCloudService())
    .Register<IAnalyticService>(new AnalyticService());
```

### EventBus

The EventBus pattern enables decoupled communication between different parts of the system by raising and handling events.

- **EventBus**: A publish-subscribe system where events are raised and listened to by different components.

#### Example Usage

```csharp
EventBus<MyCustomEvent>.Raise(new MyCustomEvent());
```

### Utilities

A collection of utility classes and extension methods designed to simplify common tasks within Unity projects.

- **Extensions**: General-purpose extension methods for various types.
- **GameObjectExtensions**: Extensions for working with `GameObject` instances.
- **TransformExtensions**: Extensions for manipulating `Transform` components.
- **Logman**: A lightweight logging utility for development and debugging.

## Common

### Bootstrapper

The Bootstrapper is responsible for initializing and managing global services. It registers services like the Unity Cloud Service, Remote Config Service, and Universal Technic Service, making them available globally through the Service Locator.

#### Example Usage

```csharp
private void Awake()
{
    ServiceLocator.Global
        .Register<IInternetConnectionCheckingService>(new InternetConnectionCheckingService())
        .Register<IUnityCloudService>(new UnityCloudService())
        .Register<IAnalyticService>(new AnalyticService());
}
```

### Universal Technic Service

The Universal Technic Service manages the instantiation and control of breathing techniques, known as Universal Technics, in the Mindway app. Each technic is packaged as an addressable prefab and must implement the `IUniversalTechnic` interface.

- **InstantiateUniversalTechnic**: Instantiates a technic based on its `UTType` and configures it using the Builder Pattern.
- **UnloadAllAddressableUniversalTechnics**: Unloads all instantiated technics to free up resources.

#### Example Usage

```csharp
var technic = await _universalTechnicService.InstantiateUniversalTechnic<ImmersiveBreathing>(UTType.ImmersiveBreathing);

technic.SetLoopCount(5)
       .SetBreathInDuration(3)
       .SetHoldDuration(2)
       .SetBreathOutDuration(6)
       .StartImmersiveBreathing();
```

### Remote Config Service

The Remote Config Service manages dynamic configuration using Unity's Remote Config service, allowing for real-time updates without requiring an app update.

- **Init**: Initializes the service and fetches the latest configurations.
- **FetchConfigs**: Retrieves configuration data from the server.

#### Example Usage

```csharp
await _remoteConfigService.Init();
```

### Unity Cloud Service

The Unity Cloud Service handles the initialization of Unity Gaming Services and manages the environment (Development or Production) to ensure safe testing and deployment.

- **SetTargetCloudEnvironment**: Sets the target environment for Unity Gaming Services.
- **InitUnityServices**: Initializes Unity Gaming Services based on the selected environment.

#### Example Usage

```csharp
unityCloudService.SetTargetCloudEnvironment();
await unityCloudService.InitUnityServices();
```

### Internet Connection Checking Service

This service checks the availability of an internet connection by sending requests to specific URLs.

- **IsInternetValid**: Checks if the internet connection is valid by sending a request to a predefined URL.

#### Example Usage

```csharp
bool isConnected = await _internetConnectionService.IsInternetValid();
```

### Unity Authentication Service

The Unity Authentication Service manages player authentication using Unity's Authentication Service, supporting features like anonymous sign-in.

- **Init**: Initializes the authentication service and handles sign-in events.

#### Example Usage

```csharp
await _unityAuthService.Init();
```

### Analytic Service

The Analytic Service handles the logging and reporting of various analytic events using Unity Analytics.

- **SendActionButtonPressedEvent**: Logs when an action button is pressed.
- **SendPlayerLoginEvent**: Logs when a player logs in.

#### Example Usage

```csharp
_analyticService.SendPlayerLoginEvent();
```

## Examples

### Instantiating and Configuring a Universal Technic

```csharp
var technic = await _universalTechnicService.InstantiateUniversalTechnic<ImmersiveBreathing>(UTType.ImmersiveBreathing);

technic.SetLoopCount(5)
       .SetBreathInDuration(3)
       .SetHoldDuration(2)
       .SetBreathOutDuration(6)
       .StartImmersiveBreathing();
```

### Registering and Using Services

```csharp
ServiceLocator.Global
    .Register<IUnityAuthService>(new UnityAuthService())
    .Register<IInternetConnectionCheckingService>(new InternetConnectionCheckingService());

await _unityAuthService.Init();
bool isConnected = await _internetConnectionService.IsInternetValid();
```

### Using the EventBus

```csharp
EventBus<MyCustomEvent>.Raise(new MyCustomEvent());

EventBus<MyCustomEvent>.Subscribe(OnCustomEvent);

void OnCustomEvent(MyCustomEvent evt)
{
    // Handle the event
}
```

## Conclusion

The **Taha Development Kit (TDK)** provides a robust framework for Unity development, leveraging design patterns and best practices to ensure maintainable and scalable code. With its comprehensive set of services and utilities, TDK streamlines the development process, allowing you to focus on creating high-quality, efficient, and reliable Unity applications.

For more detailed examples and documentation, please refer to the individual modules in this README or explore the source code.
