# Graphics

The Spice Engine is a work-in-progress game engine, written in C# and GLSL through the OpenTK library.
The intent is to produce a library that can act as a base game library for both 2D and 3D games, with the ability to parse the associated common file formats for game assets (obj's for meshes, dae's for animations, etc.).
It will, however, contain its own file formats for Maps that act as a way to store the "game world" that the engine will then load up.

This project is mostly meant to be a fun learning tool for myself, with pipe dream aspirations of eventually having a functional, competant, and open-source game engine.

## SpiceEngine

This is the library where all of the internal game engine code itself is being written.
This includes all classes representing game entities, AI behavior trees, and GLSL shader files.

Two main controls act as the entry points for the engine: GameWindow and GamePanel.
GameWindow leverages OpenTK's functionality for creating a native window and tying an OpenGL context with it.
GamePanel uses the provided GLControl WinForms control instead.

The engine's logic is split into two main classes: GameManager and RenderManager.
GameManager manages all internal logic within the engine.
RenderManager manages the rendering pipeline, including batching objects for GPU draws and the internal shaders being run.

Within the GameManager, the engine logic is further split into the following: EntityManager, TextureManager, InputManager, PhysicsManager, BehaviorManager, ScriptManager, and SoundManager.

The EntityManager is responsible for maintaining and tracking all entities currently loaded into the game world. It is also responsible for assigning a unique ID to each entity. Entities are split into four categories: Actors, Brushes, Volumes, and Lights.

Both GameWindow and GamePanel start by loading from a Map file.
The Map class helps to manage all data that needs to be stored in a Map file on disk, and how this data is converted into in-memory objects by the engine.

## SauceEditor

SauceEditor is a Windows WPF application that acts as a front-end interface for creating and testing maps within the SpiceEngine.

## Jidai

Jidai is a simple sample project for launching a window and loading a test map in the SpiceEngine.
All test resources are also stored here, such as textures, meshes, behaviors, etc.
Due to the volatile state of the Map file format for the (hopefully) near future, upon launching this sample project, the Map file is programatically created, written to disk, then loaded up in the engine.