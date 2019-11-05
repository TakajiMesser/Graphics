using SpiceEngineCore.Entities;
using SpiceEngineCore.Game.Loading;
using SpiceEngineCore.Game.Loading.Builders;
using SpiceEngineCore.Rendering.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpiceEngineCore.Components.Animations
{
    // TODO - This class violates the boundary between game logic and rendering logic
    public class AnimationManager : ComponentLoader<IAnimatorBuilder>, IAnimationProvider
    {
        private IEntityProvider _entityProvider;

        private Dictionary<int, IAnimator> _animatorByEntityID = new Dictionary<int, IAnimator>();
        private Dictionary<int, IAnimatedModel> _modelByEntityID = new Dictionary<int, IAnimatedModel>();

        private List<Animation> _animations = new List<Animation>();

        public AnimationManager(IEntityProvider entityProvider) => _entityProvider = entityProvider;

        public override void AddComponent(int entityID, IAnimatorBuilder builder)
        {
            var animator = builder.ToAnimator();

            if (animator != null)
            {
                animator.DefaultAnimation = builder.Animations.First();
                _animatorByEntityID.Add(entityID, animator);
            }

            // TODO - Check for duplicate animations
            _animations.AddRange(builder.Animations);
        }

        private readonly object _lock = new object();

        // TODO - Unfortunately, we have a race condition between the Renderer loaders and the Animator loaders here... when do we want to bind the event handler?
        public void AddModel(int entityID, IAnimatedModel model)
        {
            lock (_lock)
            {
                _modelByEntityID.Add(entityID, model);

                if (_animatorByEntityID.ContainsKey(entityID))
                {
                    var animator = _animatorByEntityID[entityID];
                    animator.Animate += (s, args) => model.SetKeyFrame(args.KeyFrame);
                }
            }
        }

        protected override void LoadComponents()
        {
            foreach (var actor in _entityProvider.Actors)
            {
                // TODO - Come up with a better way than constantly locking and unlocking here
                lock (_lock)
                {
                    if (_animatorByEntityID.ContainsKey(actor.ID))
                    {
                        var animator = _animatorByEntityID[actor.ID];

                        if (_modelByEntityID.ContainsKey(actor.ID))
                        {
                            var model = _modelByEntityID[actor.ID];
                            animator.Animate += (s, args) => model.SetKeyFrame(args.KeyFrame);
                        }
                    }
                }
                
            }
        }

        protected override Task LoadInternal()
        {
            return Task.Run(() =>
            {

            });
        }

        protected override void Update()
        {
            // TODO - Replace this with a list of id's to avoid checking every actor each frame
            foreach (var actor in _entityProvider.Actors)
            {
                if (_animatorByEntityID.ContainsKey(actor.ID))
                {
                    var animator = _animatorByEntityID[actor.ID];

                    // TODO - Remove this -> setting to first animation by default
                    animator.CurrentAnimation = animator.DefaultAnimation;
                    animator.Tick(TickRate);
                }
            }
        }
    }
}
