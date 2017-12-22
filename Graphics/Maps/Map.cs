﻿using Graphics.Brushes;
using Graphics.GameObjects;
using Graphics.Helpers;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Graphics.Maps
{
    /// <summary>
    /// A map should consist of a collection of static brushes, gameObjects, a camera, and/or a player (cutscenes and menu's won't have a player)
    /// 
    /// </summary>
    public class Map
    {
        public MapPlayer Player { get; set; }
        public MapCamera Camera { get; set; }
        public List<MapGameObject> GameObjects { get; set; } = new List<MapGameObject>();
        public List<Brush> Brushes { get; set; } = new List<Brush>();

        public void Save(string path)
        {
            using (var writer = XmlWriter.Create(path))
            {
                var serializer = new DataContractSerializer(typeof(Map));
                serializer.WriteObject(writer, this);
            }
        }

        public static Map Load(string path)
        {
            using (var reader = XmlReader.Create(path))
            {
                var serializer = new DataContractSerializer(typeof(Map));
                return serializer.ReadObject(reader, true) as Map;
            }
        }

        public static void SaveTestMap()
        {
            var map = new Map()
            {
                Player = new MapPlayer()
                {
                    Name = "Player",
                    Position = new Vector3(0.0f, 0.0f, -1.0f),
                    Scale = Vector3.One,
                    Rotation = Quaternion.Identity,
                    MeshFilePath = FilePathHelper.PLAYER_MESH_PATH
                },
                Camera = new MapCamera()
                {
                    Name = "MainCamera",
                    AttachedGameObjectName = "Player",
                    Position = Vector3.Zero
                }
            };

            map.GameObjects.Add(new MapGameObject()
            {
                Name = "Triangle",
                Position = new Vector3(5.0f, 5.0f, -1.0f),
                Scale = Vector3.One,
                Rotation = Quaternion.Identity,
                MeshFilePath = FilePathHelper.TRIANGLE_MESH_PATH
            });
            //map.Brushes.Add(Brush.Rectangle(new Vector3(), 20.0f, 20.0f, program));
            /*map.GameObjects.Add(new MapGameObject()
            {
                Name = "Floor",
                Position = new Vector3(0.0f, 0.0f, -10.0f),
                Scale = Vector3.One,
                Rotation = Quaternion.Identity,
                MeshFilePath = FilePathHelper.FLOOR_MESH_PATH
            });*/

            map.Save(FilePathHelper.MAP_PATH);
        }
    }
}
