using OpenTK;
using SpiceEngineCore.Entities.Actors;
using SpiceEngineCore.Entities.Brushes;
using SpiceEngineCore.Entities.Volumes;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Rendering.Matrices;
using SpiceEngineCore.Rendering.Shaders;
using SpiceEngineCore.Rendering.Textures;
using System;

namespace SpiceEngineCore.Entities
{
    // TODO - This is terrible, much more ideal to call out to the MapComponent and recreate an entity from there
    public class EntityFactory
    {
        /// <summary>
        /// Duplicate an entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Either a duplicated entity with an ID of zero,
        /// or null to indicate that the entity cannot be duplicated.</returns>
        public static IEntity Duplicate(IEntity entity)
        {
            var duplicatedEntity = GetDuplicate(entity);

            if (duplicatedEntity != null)
            {
                duplicatedEntity.Position = entity.Position;

                if (entity is IRotate rotator && duplicatedEntity is IRotate duplicatedRotator)
                {
                    duplicatedRotator.Rotation = rotator.Rotation;
                }

                if (entity is IScale scaler && duplicatedEntity is IScale duplicatedScaler)
                {
                    duplicatedScaler.Scale = scaler.Scale;
                }

                if (entity is ITextureBinder binder && duplicatedEntity is ITextureBinder duplicatedBinder)
                {
                    foreach (var material in binder.Materials)
                    {
                        duplicatedBinder.AddMaterial(material);
                    }

                    foreach (var textureMapping in binder.TextureMappings)
                    {
                        duplicatedBinder.AddTextureMapping(textureMapping);
                    }
                }
            }

            return null;
        }

        private static IEntity GetDuplicate(IEntity entity)
        {
            switch (entity)
            {
                case IBrush brush:
                    return Duplicate(brush);
                case ILight light:
                    return Duplicate(light);
                case IVolume volume:
                    return Duplicate(volume);
                case IActor actor:
                    return Duplicate(actor);
            }

            return null;
        }

        private static IBrush Duplicate(IBrush brush)
        {
            if (brush is Brush baseBrush)
            {
                return new Brush();
            }

            return null;
        }

        private static ILight Duplicate(ILight light)
        {
            return null;
        }

        private static IVolume Duplicate(IVolume volume)
        {
            if (volume is Volume baseVolume)
            {
                return new Volume();
            }

            return null;
        }

        private static IActor Duplicate(IActor actor)
        {
            if (actor is Actor baseActor)
            {
                return new Actor()
                {
                    Orientation = baseActor.Orientation
                };
            }
            else if (actor is AnimatedActor animatedActor)
            {
                return new AnimatedActor()
                {
                    Orientation = animatedActor.Orientation
                };

                /*public override IEntity Duplicate()
                {
                    var animatedActor = new AnimatedActor();
                    animatedActor.FromActor(this);

                    foreach (var kvp in _jointTransformsByMeshIndex)
                    {
                        animatedActor._jointTransformsByMeshIndex.Add(kvp.Key, kvp.Value);
                    }

                    return animatedActor;
                }*/
            }

            return null;
        }
    }
}
