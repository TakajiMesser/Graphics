using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics.Materials
{
    public struct Material
    {
        public string Name { get; private set; }

        public Vector3 Ambient { get; set; }
        public Vector3 Diffuse { get; set; }
        public Vector3 Specular { get; set; }

        public float SpecularExponent { get; set; }

        public Material(string name)
        {
            Name = name;
            Ambient = Vector3.Zero;
            Diffuse = Vector3.Zero;
            Specular = Vector3.Zero;
            SpecularExponent = 0.0f;
        }

        public void SaveToFile(string path)
        {
            using (var writer = new StreamWriter(path))
            {
                writer.WriteLine("newmtl " + Name);
                writer.WriteLine("Ka " + Ambient.X + " " + Ambient.Y + " " + Ambient.Z);
                writer.WriteLine("Kd " + Diffuse.X + " " + Diffuse.Y + " " + Diffuse.Z);
                writer.WriteLine("Ks " + Specular.X + " " + Specular.Y + " " + Specular.Z);
                writer.WriteLine("Ns " + SpecularExponent);
            }
        }

        public static IEnumerable<Material> LoadFromFile(string path)
        {
            var material = new Material();

            foreach (var line in File.ReadLines(path))
            {
                var values = line.Split(' ');

                if (values.Length > 0)
                {
                    switch (values[0])
                    {
                        case "newmtl":
                            if (!string.IsNullOrEmpty(material.Name))
                            {
                                yield return material;
                                material = new Material();
                            }
                            material.Name = values[1];
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

            yield return material;
        }
    }
}
