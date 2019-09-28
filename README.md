# Spice-Engine

The Spice Engine is a work-in-progress game engine, written in C# and GLSL through the OpenTK library.
The intent is to produce a library that can act as a base game library for both 2D and 3D games, with the ability to parse the associated common file formats for game assets (OBJ for meshes, DAE for animations, etc.).
It will, however, contain its own file formats for Maps that act as a way to store the "game world" that the engine will then load up.

This project is mostly meant to be a fun learning tool for myself, with pipe dream aspirations of eventually having a functional, competent, and open-source game engine.

## SpiceEngine

This is the library where all of the internal game engine code itself is being written.
This includes all classes representing game entities, AI behavior trees, and GLSL shader files.

Two main controls act as the entry points for the engine: **GameWindow** and **GamePanel**.
**GameWindow** leverages OpenTK's functionality for creating a native window and tying an OpenGL context with it.
**GamePanel** uses the provided GLControl WinForms control instead.

The engine's logic is split into two main classes: **GameManager** and **RenderManager**.
**GameManager** manages all internal logic within the engine - in a nutshell, every piece of logic not directly related to the rendering pipeline.
**RenderManager** manages the rendering pipeline, including batching objects for GPU draws and the internal shaders being run.

Within the **GameManager**, the engine logic is further split into the following management classes: 

1. EntityManager
  The **EntityManager** is responsible for maintaining and tracking all entities currently loaded into the game world. It is also responsible for assigning a unique ID to each entity.
  
  An **Entity** represents some object within the game world. They are only guaranteed to have an integer ID and a position vector.
  
  *Entities* are split into four categories:
  
  1. Brush
    * A **Brush** is a static geometric shape that is baked into the game world. They cannot transform, and cannot be deformed.
    * A **Brush** must be visible in some way. They may or may not have a physical presence in the world outside of their visibility.
  2. Actor
    * **Actor**s are dynamic objects within the game world.
    * An **Actor** can be visible or invisible.
    * An **Actor** can have a **Behavior** associated with it (representing scripts, responses, AI, etc).
  3. Volume
    * A **Volume** is a static geometric shape this is baked into the game world. They cannot transform, and cannot be deformed.
    * A **Volume** must be invisible. It must have a physical presence in the world.
    * A **Volume** will either allow triggers upon entry, physics rules, or can act as a physical blocker for other entities.
  4. Light
    * A **Light** provides lighting and shadows to the game world. They can be static or dynamic.
    * A **Light** will have no physical presence in the world outside of its visibility.
    * **Light**s can be *Point*, *Spot*, or *Directional*.
2. TextureManager
3. InputManager
4. PhysicsManager
5. BehaviorManager
6. ScriptManager
7. SoundManager

Both **GameWindow** and **GamePanel** start by loading from a **Map** file.
The **Map** class helps to manage all data that needs to be stored in a **Map** file on disk, and how this data is converted into in-memory objects by the engine.

## SauceEditor

*SauceEditor* is a Windows WPF application that acts as a front-end interface for creating and testing maps within the *SpiceEngine*.

## SpiceEngineCore

*SpiceEngineCore* is meant to act as a transitional project. While the *SpiceEngine* project targets the .NET Framework, this project instead targets .NET Standard, which will allow for more of the codebase to be cross-platform. I will be transferring appropriate portions of the SpiceEngine project to this one.

## SauceEditorCore

Similar to the *SpiceEngineCore* project, the *SauceEditorCore* project is also meant to aid in transitioning as much of the editor model classes as possible to target .NET Standard. Since much of the editor is written in WPF, it is likely that less of the *SauceEditor* project will be able to move to this project.

## SampleGameProject

*SampleGameProject* is a simple sample project for launching a window and loading a test map in the *SpiceEngine*.
All test resources are also stored here, such as textures, meshes, behaviors, etc.
Due to the volatile state of the *Map* file format for the (hopefully) near future, upon launching this sample project, the *Map* file is programmatically created, written to disk, then loaded up in the engine.
