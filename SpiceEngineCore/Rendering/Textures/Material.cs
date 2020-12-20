using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;

namespace SpiceEngineCore.Rendering.Materials
{
    public struct Material
    {
        /// <summary>
        /// Ambient light is the light that enters a room and bounces around multiple times
        /// </summary>
        public Vector3 Ambient { get; set; }

        /// <summary>
        /// Diffuse light is the direct light hitting a surface
        /// </summary>
        public Vector3 Diffuse { get; set; }

        /// <summary>
        /// Specular light is the white highlight reflection seen on smooth, shiny objects
        /// </summary>
        public Vector3 Specular { get; set; }

        public float SpecularExponent { get; set; }

        public void SaveToFile(string name, string path)
        {
            using (var writer = new StreamWriter(path))
            {
                writer.WriteLine("newmtl " + name);
                writer.WriteLine("Ka " + Ambient.X + " " + Ambient.Y + " " + Ambient.Z);
                writer.WriteLine("Kd " + Diffuse.X + " " + Diffuse.Y + " " + Diffuse.Z);
                writer.WriteLine("Ks " + Specular.X + " " + Specular.Y + " " + Specular.Z);
                writer.WriteLine("Ns " + SpecularExponent);
            }
        }

        public static IEnumerable<Tuple<string, Material>> LoadFromFile(string path)
        {
            string name = null;
            var material = new Material();

            foreach (var line in File.ReadLines(path))
            {
                var values = line.Split(' ');

                if (values.Length > 0)
                {
                    switch (values[0])
                    {
                        case "newmtl":
                            if (!string.IsNullOrEmpty(name))
                            {
                                yield return new Tuple<string, Material>(name, material);
                                material = new Material();
                            }
                            name = values[1];
                            break;
                        case "Ka":
                            material.Ambient = new Vector3(float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3]));
                            break;
                        case "Kd":
                            material.Diffuse = new Vector3(float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3]));
                            break;
                        case "Ks":
                            material.Specular = new Vector3(float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3]));
                            break;
                        case "Ns":
                            material.SpecularExponent = float.Parse(values[1]);
                            break;
                    }
                }
            }

            yield return new Tuple<string, Material>(name, material);
        }
    }
}
