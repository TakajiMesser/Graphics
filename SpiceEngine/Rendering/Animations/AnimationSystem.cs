﻿using SpiceEngineCore.Components.Animations;
using SpiceEngineCore.Components.Builders;
using SpiceEngineCore.Entities;
using SpiceEngineCore.Game;
using SweetGraphicsCore.Rendering.Models;
using System.Collections.Generic;

namespace SpiceEngine.Rendering.Animations
{
    // TODO - This class violates the boundary between game logic and rendering logic
    public class AnimationSystem : ComponentSystem<IAnimator, IAnimatorBuilder>, IAnimationProvider
    {
        private List<IAnimation> _animations = new List<IAnimation>();
        private Dictionary<int, IAnimate> _animatedByEntityID = new Dictionary<int, IAnimate>();

        public AnimationSystem(IEntityProvider entityProvider) => SetEntityProvider(entityProvider);

        private readonly object _lock = new object();

        // TODO - Unfortunately, we have a race condition between the Renderer loaders and the Animator loaders here... when do we want to bind the event handler?
        public void AddAnimated(int entityID, IAnimate animated)
        {
            lock (_lock)
            {
                if (!_animatedByEntityID.ContainsKey(entityID))
                {
                    _animatedByEntityID.Add(entityID, animated);
                }

                if (_componentByID.ContainsKey(entityID))
                {
                    var animator = _componentByID[entityID];
                    animator.Animate += (s, args) => animated.SetKeyFrame(args.KeyFrame);
                }
            }
        }

        private Dictionary<int, int> _defaultAnimationIndexByID = new Dictionary<int, int>();

        public override void LoadBuilderSync(int entityID, IAnimatorBuilder builder)
        {
            base.LoadBuilderSync(entityID, builder);

            // TODO - Check for duplicate animations
            _defaultAnimationIndexByID.Add(entityID, _animations.Count);
            _animations.AddRange(builder.Animations);

            /*var animator = builder.ToAnimator();

            if (animator != null)
            {
                animator.DefaultAnimation = builder.Animations.First();
                _animatorByEntityID.Add(entityID, animator);
            }*/
        }

        protected override void LoadComponents()
        {
            base.LoadComponents();

            foreach (var animator in _components)
            {
                var animationIndex = _defaultAnimationIndexByID[animator.EntityID];
                animator.DefaultAnimation = _animations[animationIndex];

                if (_animatedByEntityID.ContainsKey(animator.EntityID))
                {
                    var animated = _animatedByEntityID[animator.EntityID];
                    animator.Animate += (s, args) => animated.SetKeyFrame(args.KeyFrame);
                }
            }
        }

        protected override void Update()
        {
            foreach (var animator in _components)
            {
                // TODO - Remove this -> setting to first animation by default
                animator.CurrentAnimation = animator.DefaultAnimation;
                animator.Tick(TickRate);
            }
        }
    }
}
