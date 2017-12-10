using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Materials
{
    public struct Material
    {
        // Ambient light is the light that enters a room and bounces around multiple times
        // Ambient light = illuminance * light ambient color * ambient material
        public Vector4 Ambient { get; set; }

        // Diffuse light is the direct light hitting a surface
        // Diffuse light = illuminance * max(0, dot(light direction, surface normal vector)) * light diffuse color * diffuse material
        public Vector4 Diffuse { get; set; }

        // Specular light is the white highlight reflection seen on smooth, shiny objects
        // Specular light = illuminance * max(0, dot(perfect reflection vector, vector towards viewer)) ^ shininess coefficient * light specular color * specular material
        // NOTE if dot(light direction, surface normal vector) is <= 0, then specular light color is zero
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
                            material.Ambient = new Vector4(float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3]), 1.0f);
                            break;
                        case "Kd":
                            material.Diffuse = new Vector4(float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3]), 1.0f);
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
