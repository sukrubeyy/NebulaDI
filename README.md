# Nebula Dependency Injection Framework for Unity

Nebula is a Dependency Injection (DI) framework designed for Unity. It supports injecting dependencies into `MonoBehaviour`, `ScriptableObject`, and plain C# classes. Dependencies can be injected into fields, properties, constructors, or methods, providing a versatile and powerful system for Unity developers.

## Features

- **Injectable Targets:** MonoBehaviour, ScriptableObject, and plain C# classes.
- **Injection Methods:** Fields, Properties, Constructors, and Methods.
- **Lifecycle Management:** Services can be registered as Singleton or Transient.
- **Global DI System:** Automatically persists across scenes using `DontDestroyOnLoad`.
- **Editor Tools:** Includes an editor window to inspect injected dependencies and their associated classes.

## Installation

1. Right-click in the Unity Hierarchy.
2. Navigate to `Nebula` → `Create Nebula Installer` to set up the DI system.
3. The DI system is created globally and persists across scene transitions, ensuring that you only need to set it up once.

## Usage

### Registering Services

Dependencies are registered within a class inheriting from `NebulaInstaller`. Below are examples of how to register services:

#### Register as Singleton

'''
public class Nebula : NebulaInstaller
{
    public override void OverrideBindings()
    {
        Services.AsSingleton<RandomGuidGenerator>();
    }
}
'''

#### Register as Transient

'''
public class Nebula : NebulaInstaller
{
    public override void OverrideBindings()
    {
        Services.AsTransient<RandomGuidGenerator>();
    }
}
'''

#### MonoBehaviour as a Service

MonoBehaviour classes can also be registered as services by referencing them directly.

'''
public class RandomGuidGenerator : MonoBehaviour
{
    public Guid MyProperty { get; set; } = Guid.NewGuid();
}

public class Nebula : NebulaInstaller
{
    public RandomGuidGenerator guidGenerator;
    public override void OverrideBindings()
    {
        Services.AsTransient(guidGenerator);
    }
}
'''

### Injecting Dependencies

#### MonoBehaviour Example

'''
public class GameManager : MonoBehaviour
{
    [Inject]
    public RandomGuidGenerator _randomGuid1;

    [Inject]
    public RandomGuidGenerator MyProperty { get; set; }

    [Inject]
    public void Initialize(RandomGuidGenerator _randomGuidGenerator) { }
}
'''

#### Plain C# Class Example

'''
public class GameManager
{
    public GameManager()
    {
    }

    [Inject]
    public GameManager(RandomGuidGenerator _randomGuidGenerator) { }
}
'''

> **Note:** Plain C# classes must define a parameterless constructor to avoid injection errors if no parameters are provided.

### Interface Injection

You can also inject interfaces and their implementations:

'''
public interface ILogger
{
    void Log(string msg);
}

public class ConsoleLogger : ILogger
{
    public void Log(string msg)
    {
        Debug.Log(msg);
    }
}

public class GameManager
{
    public GameManager()
    {
    }

    [Inject]
    public GameManager(ILogger consoleLogger)
    {
        consoleLogger.Log("[CONSOLE] MESSAGE");
    }
}

public class Nebula : NebulaInstaller
{
    public override void OverrideBindings()
    {
        Services.AsTransient<ILogger, ConsoleLogger>();
    }
}
'''

## How It Works

1. During initialization, Nebula scans all MonoBehaviour scripts in the scene.
2. Any fields, properties, methods, or constructors annotated with the `[Inject]` attribute are resolved automatically.
3. The DI system uses ObjectInstanceID to track injected objects, preventing duplicate injections across scene transitions.

## Editor Tools

An editor window is included to visualize all injected classes and their dependencies. This tool helps developers debug and manage their DI setup efficiently.

---

Start using Nebula to simplify and streamline dependency management in your Unity projects!
