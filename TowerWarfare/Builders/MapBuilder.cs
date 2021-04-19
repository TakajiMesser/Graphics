using SpiceEngine.Maps;
using SpiceEngine.Rendering.PostProcessing;
using SpiceEngineCore.Geometry;
using SpiceEngineCore.Maps;
using StarchUICore.Attributes.Positions;
using SweetGraphicsCore.Rendering.Meshes;
using SweetGraphicsCore.Rendering.Models;
using System.Collections.Generic;
using TowerWarfare.Entities.Actors.Towers;
using TowerWarfare.Entities.Actors.Troops;
using TowerWarfare.Entities.Cameras;
using static SpiceEngineCore.Maps.MapLight;

namespace TowerWarfare.Builders
{
    public static class MapBuilder
    {
        public static void GenerateTestMap(string filePath)
        {
            var map = new Map3D();

            map.Cameras.AddRange(GenerateCameras());
            map.Actors.AddRange(GenerateActors());
            map.Brushes.AddRange(GenerateBrushes());
            map.Lights.AddRange(GenerateLights());
            map.Volumes.AddRange(GenerateVolumes());
            map.UIItems.AddRange(GenerateUIItems());

            /*map.SkyboxTextureFilePaths = new List<string>
            {
                FilePathHelper.SPACE_01_TEXTURE_PATH,
                FilePathHelper.SPACE_02_TEXTURE_PATH,
                FilePathHelper.SPACE_03_TEXTURE_PATH,
                FilePathHelper.SPACE_04_TEXTURE_PATH,
                FilePathHelper.SPACE_05_TEXTURE_PATH,
                FilePathHelper.SPACE_06_TEXTURE_PATH,
            };*/

            map.Save(filePath);
        }

        private static IEnumerable<MapCamera> GenerateCameras()
        {
            yield return new Camera();
        }

        private static IEnumerable<MapActor> GenerateActors()
        {
            //yield return new Player();
            //yield return new Enemy();
            yield return new BasicTower(new Vector3(0.0f, 5.0f, 5.0f), "BasicTower");
            yield return new BasicTower(new Vector3(0.0f, -5.0f, 5.0f), "BasicTower");
            yield return new BasicTower(new Vector3(-5.0f, 0.0f, 5.0f), "BasicTower");

            yield return new BasicEnemy(new Vector3(0.0f, 10.0f, 5.0f));

            //yield return new BasicEnemy(new Vector3(10.0f, 10.0f, 5.0f));
        }

        private static MapBrush GenerateTile()
        {
            return new MapBrush();
        }

        private static IEnumerable<MapBrush> GenerateBrushes()
        {
            var floor = MapBrush.Rectangle(new Vector3(0.0f, 0.0f, -1.5f), 100.0f, 100.0f);
            floor.IsPhysical = true;
            //floor = MapBrush.Box(new Vector3(0.0f, 0.0f, -2.5f), 50.0f, 50.0f, 1.0f);
            //floor.TextureFilePath = FilePathHelper.GRASS_TEXTURE_PATH;
            //floor.NormalMapFilePath = FilePathHelper.GRASS_N_TEXTURE_PATH;
            yield return floor;

            var wallShape = ModelMesh.Box(5.0f, 10.0f, 5.0f);
            wallShape.UVMap = UVMap.Standard.Scaled(new Vector2(2.0f, 2.0f));
            var wall = new MapBrush(new ModelBuilder(wallShape))
            {
                Position = new Vector3(10.0f, 0.0f, 0.0f),
                IsPhysical = true,
                /*TexturesPaths = new List<TexturePaths>
                {
                    new TexturePaths()
                    {
                        DiffuseMapFilePath = FilePathHelper.BRICK_01_D_TEXTURE_PATH,
                        NormalMapFilePath = FilePathHelper.BRICK_01_N_NORMAL_PATH
                    }
                }*/
            };
            yield return wall;

            var wallShape2 = ModelMesh.Box(5.0f, 10.0f, 5.0f);
            wallShape2.UVMap = UVMap.Standard.Scaled(new Vector2(2.0f, 2.0f));
            var wall2 = new MapBrush(new ModelBuilder(wallShape2))
            {
                Position = new Vector3(-10.0f, 0.0f, 0.0f),
                IsPhysical = true,
                /*TexturesPaths = new List<TexturePaths>
                {
                    new TexturePaths()
                    {
                        DiffuseMapFilePath = FilePathHelper.BRICK_01_D_TEXTURE_PATH,
                        NormalMapFilePath = FilePathHelper.BRICK_01_N_NORMAL_PATH
                    }
                }*/
            };
            yield return wall2;
        }

        private static IEnumerable<MapLight> GenerateLights()
        {
            yield return new MapLight()
            {
                LightType = LightTypes.Point,
                Position = new Vector3(0.0f, 0.0f, 30.0f),
                Radius = 50.0f,
                Color = new Color4(1.0f, 1.0f, 1.0f, 1.0f),
                Intensity = 0.7f
            };

            yield return new MapLight()
            {
                LightType = LightTypes.Point,
                Position = new Vector3(0.0f, 20.0f, 3.0f),
                Radius = 30.0f,
                Color = new Color4(1.0f, 1.0f, 1.0f, 1.0f),
                Intensity = 0.25f
            };

            yield return new MapLight()
            {
                LightType = LightTypes.Spot,
                Position = new Vector3(-17.0f, -2.0f, 3.0f),
                Radius = 10.0f,
                Color = new Color4(1.0f, 1.0f, 1.0f, 1.0f),
                Intensity = 0.5f,
                Rotation = new Vector3(-45.0f, -45.0f, 0.0f),//Quaternion.FromAxisAngle(Vector3.UnitZ, UnitConversions.ToRadians(-45.0f)) * Quaternion.FromAxisAngle(Vector3.UnitY, UnitConversions.ToRadians(-45.0f)),
                Height = 20.0f
            };
        }

        private static IEnumerable<MapVolume> GenerateVolumes()
        {
            var physicsVolume = MapVolume.Box(Vector3.Zero, 100.0f, 100.0f, 100.0f);
            physicsVolume.VolumeType = MapVolume.VolumeTypes.Physics;
            physicsVolume.Gravity = -0.3f * Vector3.UnitZ;
            yield return physicsVolume;
        }

        /*
            Units = [Pixels, Percents, Auto]
            Anchor = Position at [Start, Center, End]
            Dock = Size

            Position = [X, Y]
                (Pixels, Start) => Offsets from Anchor by this hardcoded amount
                (Pixels, Center) => 
                (Pixels, End) => 
                (Percents, Start) => Offsets from Anchor by this % of anchor size (or of available space?)
                (Percents, Center) => 
                (Percents, End) => 
                (Auto, Start) => Ignores Anchor, and accepts Group's suggested position. For Groups, 
                (Auto, Center) => 
                (Auto, End) => 

            Size = [Width, Height]
                (Pixels) => Ignores Dock, hardcodes size
                (Percents) => Set to % of dock size
                (Auto) => For Groups, sets to size of content. For Views, identical to 100%
             
        */

        private static IEnumerable<MapUIItem> GenerateUIItems()
        {
            yield return new MapUIItem()
            {
                Name = "RowGroup 1A",
                UIType = UITypes.RowGroup,
                X = "0",
                Y = "0",
                Width = "100%",
                Height = "Auto",
                HorizontalAnchorSelfType = AnchorTypes.Start,
                HorizontalAnchorRelativeType = AnchorTypes.Start,
                VerticalAnchorSelfType = AnchorTypes.End,
                VerticalAnchorRelativeType = AnchorTypes.End,
                ChildElementNames = new List<string> { "View 2A", "LayerGroup 2B" },
                PaddingLeft = "20",
                PaddingTop = "20",
                PaddingRight = "20",
                PaddingBottom = "20",
            };

            yield return new MapUIItem()
            {
                Name = "View 2A",
                UIType = UITypes.View,
                X = "0",
                Y = "0",
                Width = "100",
                Height = "200",
                HorizontalAnchorSelfType = AnchorTypes.Start,
                HorizontalAnchorRelativeType = AnchorTypes.Start,
                VerticalAnchorSelfType = AnchorTypes.Center,
                VerticalAnchorRelativeType = AnchorTypes.Center,
                DoesHorizontalAnchorRespectChanges = true,
                DoesVerticalAnchorRespectChanges = true,
                Color = Color4.LightSeaGreen,
                CornerXRadius = 15,
                CornerYRadius = 10,
                BorderThickness = 5,
                BorderColor = Color4.DarkSeaGreen
            };

            yield return new MapUIItem()
            {
                Name = "LayerGroup 2B",
                UIType = UITypes.LayerGroup,
                X = "0",
                Y = "0",
                Width = "220",
                Height = "220",
                HorizontalAnchorSelfType = AnchorTypes.Start,
                HorizontalAnchorRelativeType = AnchorTypes.Start,
                VerticalAnchorSelfType = AnchorTypes.Center,
                VerticalAnchorRelativeType = AnchorTypes.Center,
                ChildElementNames = new List<string> { "Button 3A", "Label 3B" },
                PaddingLeft = "10",
                PaddingTop = "10",
                PaddingRight = "10",
                PaddingBottom = "10"
            };

            yield return new MapUIItem()
            {
                Name = "Button 3A",
                UIType = UITypes.Button,
                X = "0",
                Y = "0",
                Width = "200",
                Height = "100",
                HorizontalAnchorSelfType = AnchorTypes.Start,
                HorizontalAnchorRelativeType = AnchorTypes.Start,
                VerticalAnchorSelfType = AnchorTypes.Center,
                VerticalAnchorRelativeType = AnchorTypes.Center,
                DoesHorizontalAnchorRespectChanges = true,
                DoesVerticalAnchorRespectChanges = true,
                Color = Color4.LightCyan,
                CornerXRadius = 15,
                CornerYRadius = 10,
                BorderThickness = 5,
                BorderColor = Color4.DarkCyan,
                PushScript = new UmamiScriptingCore.Scripts.Script()
                {
                    Name = "ButtonPushNode",
                    SourcePath = Helpers.FilePathHelper.BUTTON_PUSH_NODE_PATH
                }
            };

            yield return new MapUIItem()
            {
                Name = "Label 3B",
                UIType = UITypes.Label,
                X = "0",
                Y = "0",
                Width = "Auto",
                Height = "Auto",
                HorizontalAnchorSelfType = AnchorTypes.Center,
                HorizontalAnchorRelativeType = AnchorTypes.Center,
                VerticalAnchorSelfType = AnchorTypes.Center,
                VerticalAnchorRelativeType = AnchorTypes.Center,
                DoesHorizontalAnchorRespectChanges = true,
                DoesVerticalAnchorRespectChanges = true,
                Color = Color4.Black,
                FontFilePath = TextRenderer.FONT_PATH,
                FontSize = 14
            };

            /*yield return new MapUIItem()
            {
                Name = "View 2B",
                UIType = UITypes.View,
                X = "0",
                Y = "0",
                Width = "10%",
                Height = "10%",
                Color = Color4.Cyan
            };

            yield return new MapUIItem()
            {
                Name = "View 3A",
                UIType = UITypes.View,
                X = "0",
                Y = "0",
                Width = "200",
                Height = "300",
                Color = Color4.Coral
            };

            yield return new MapUIItem()
            {
                Name = "View 3B",
                UIType = UITypes.View,
                X = "0",
                Y = "0",
                Width = "100%",
                Height = "Auto",
                Color = Color4.Beige
            };

            yield return new MapUIItem()
            {
                Name = "RowGroup 2C",
                UIType = UITypes.RowGroup,
                X = "0",
                Y = "0",
                Width = "50%",
                Height = "200",
                ChildElementNames = new List<string>() { "View 3A", "View 3B" }
            };

            yield return new MapUIItem()
            {
                Name = "View 2D",
                UIType = UITypes.View,
                X = "0",
                Y = "0",
                Width = "100%",
                Height = "100%",
                Color = Color4.Aqua
            };

            yield return new MapUIItem()
            {
                Name = "RowGroup 1A",
                UIType = UITypes.RowGroup,
                X = "0",
                Y = "0",
                Width = "800",
                Height = "Auto",
                ChildElementNames = new List<string>() { "View 2A", "View 2B", "RowGroup 2C", "View 2D" }
            };

            /*var simpleButton = new MapUIItem()
            {
                Name = "Test Button",
                XUnitType = MapUIItem.UnitTypes.Pixels,
                YUnitType = MapUIItem.UnitTypes.Pixels,
                WidthUnitType = MapUIItem.UnitTypes.Pixels,
                HeightUnitType = MapUIItem.UnitTypes.Pixels,
                XUnits = "400",
                YUnits = "400",
                WidthUnits = "600",
                HeightUnits = "300",
                UIType = MapUIItem.UITypes.Button
            };

            yield return simpleButton;*/
        }
    }
}
