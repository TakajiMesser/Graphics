using CitrusAnimationCore;
using CitrusAnimationCore.Animations;
using OpenTK.Graphics.OpenGL;
using SavoryPhysicsCore;
using SmokyAudioCore.Sounds;
using SpiceEngine.Scripting;
using SpiceEngineCore.Game;
using SpiceEngineCore.Helpers;
using SpiceEngineCore.Rendering;
using StarchUICore;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using TangyHIDCore;
using TangyHIDCore.Inputs;
using UmamiScriptingCore;

namespace SpiceEngine.Game
{
    public class SimulationManager : GameManager, ISimulate
    {
        private Resolution _resolution;

        public SimulationManager(Resolution resolution) => _resolution = resolution;

        public InputManager InputManager { get; private set; }
        public PhysicsSystem PhysicsSystem { get; private set; }
        public BehaviorSystem BehaviorSystem { get; private set; }
        public AnimationSystem AnimationSystem { get; private set; }
        public UISystem UISystem { get; private set; }
        public SoundManager SoundManager { get; private set; }

        public bool IsLoaded { get; private set; }

        public override void Load()
        {
            IsLoaded = false;

            base.Load();
            //EntityManager.ClearEntities();

            PhysicsSystem = new PhysicsSystem();
            AddGameSystem<IPhysicsProvider>(PhysicsSystem);
            AddComponentProvider<IBody>(PhysicsSystem);

            BehaviorSystem = new BehaviorSystem(new ScriptManager());
            AddGameSystem(BehaviorSystem);
            AddComponentProvider<IBehavior>(BehaviorSystem);

            AnimationSystem = new AnimationSystem();
            AddGameSystem<IAnimationProvider>(AnimationSystem);
            AddComponentProvider<IAnimator>(AnimationSystem);

            UISystem = new UISystem(_resolution);
            AddGameSystem<IUIProvider>(UISystem);
            AddComponentProvider<IElement>(UISystem);

            InputManager = new InputManager();
            AddGameSystem<IInputProvider>(InputManager);

            IsLoaded = true;
        }

        public void SaveToFile(string path) => throw new NotImplementedException();

        public static SimulationManager LoadFromFile(string path) => throw new NotImplementedException();

        private void TakeScreenshot()
        {
            var bitmap = new Bitmap(_resolution.Width, _resolution.Height);
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, _resolution.Width, _resolution.Height), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.ReadPixels(0, 0, _resolution.Width, _resolution.Height, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            GL.Finish();

            bitmap.UnlockBits(data);
            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);

            string fileName = FilePathHelper.SCREENSHOT_PATH + "\\"
                + DateTime.Now.Year.ToString("0000") + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00") + "_"
                + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + ".png";

            bitmap.Save(fileName, ImageFormat.Png);
            bitmap.Dispose();
        }
    }
}
