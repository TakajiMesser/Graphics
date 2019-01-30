using OpenTK;
using System.Collections.Generic;
using System.Linq;
using SpiceEngine.Entities.Cameras;
using SpiceEngine.Entities.Lights;
using SpiceEngine.Entities.Models;
using SpiceEngine.Game;
using SpiceEngine.Inputs;
using SpiceEngine.Physics.Collision;
using SpiceEngine.Rendering.Shaders;
using SpiceEngine.Rendering.Textures;
using SpiceEngine.Scripting.Behaviors;
using SpiceEngine.Scripting.StimResponse;
using SpiceEngine.Rendering.Matrices;
using SpiceEngine.Rendering.Materials;

namespace SpiceEngine.Entities.Actors
{
    public class ActorArchetype
    {
        // This class should act as a base Archetype for Actors to be spawned off of
        // So you can have a standalone Actor, with its own scripts etc.
        // I'll need to decide what should be handled in the Archetype, versus the Actor
        // For instance, I could map the object's collider to the Archetype, then set up the mapping
        // in the PhysicsManager to get the collider from the Archetype
        // OR I could still give each Actor its own collider, but when the Archetype collider changes,
        // we would have to go find all Actors that are derived from this Archetype, and update their
        // colliders accordingly

        // Archetypes get their own Entity ID, since that is how the PhysicsManager, ScriptManager, 
        // and SoundManager map from the IEntityProvider interface
        public int ID { get; set; }
        public string Name { get; private set; }

        // Define how this object's state will be saved, if desired
        public virtual void OnSaveState() { }
    }
}
