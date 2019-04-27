using OpenTK;
using SampleGameProject.Helpers;
using SampleGameProject.Helpers.Builders;
using SpiceEngine.Maps;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GameWindow = SpiceEngine.Game.GameWindow;

namespace SampleGameProject
{
    class Program
    {
        static void Main(string[] args)
        {
            GetDatFileOutput();
            ProjectBuilder.CreateTestProject();
            var map = Map.Load(FilePathHelper.MAP_PATH);

            using (var gameWindow = new GameWindow(map))
            {
                gameWindow.VSync = VSyncMode.Adaptive;
                gameWindow.Run(60.0, 0.0);
                //gameWindow.LoadMap(map);
            }
        }

        static void GetDatFileOutput()
        {
            var inputFilePath = @"C:\Users\Takaji\Documents\Visual Studio 2017\Projects\Graphics\SpiceEngine\Resources\Meshes\Sphere.obj";
            var outputFilePath = @"C:\Users\Takaji\Documents\Visual Studio 2017\Projects\Graphics\SpiceEngine\Resources\Meshes\SphereOutput.txt"; ;

            var inputLines = File.ReadAllLines(inputFilePath);
            var outputLines = new List<string>();

            var inputVertexLines = inputLines.Where(l => l.StartsWith("v ")).ToList();
            var inputNormalLines = inputLines.Where(l => l.StartsWith("vn ")).ToList();
            var inputTriangleIndexLines = inputLines.Where(l => l.StartsWith("f ")).ToList();

            /*var outputVertexLines = new List<string>();
            var outputNormalLines = new List<string>();
            var outputTriangleIndexLines = new List<string>();*/

            outputLines.Add("{");
            outputLines.Add("Vertices = new List<Vector3>");
            outputLines.Add("{");

            for (var i = 0; i < inputVertexLines.Count; i++)
            {
                var builder = new StringBuilder();
                builder.Append("new Vector3(");

                var regex = new Regex(@"v (?<x>-?[0-9]\d*(\.\d+)?) (?<y>-?[0-9]\d*(\.\d+)?) (?<z>-?[0-9]\d*(\.\d+)?)");
                var match = regex.Match(inputVertexLines[i]);

                builder.Append("radius * " + match.Groups["x"].Value + "f, ");
                builder.Append("radius * " + match.Groups["y"].Value + "f, ");
                builder.Append("radius * " + match.Groups["z"].Value + "f");
                // new Vector3(-width / 2.0f, -height / 2.0f, -depth / 2.0f),

                builder.Append(")");

                if (i < inputVertexLines.Count - 1)
                {
                    builder.Append(",");
                }

                outputLines.Add(builder.ToString());
            }

            outputLines.Add("},");
            outputLines.Add("TriangleIndices = new List<int>");
            outputLines.Add("{");

            for (var i = 0; i < inputTriangleIndexLines.Count; i++)
            {
                var builder = new StringBuilder();

                var regex = new Regex(@"f (?<a1>[0-9]\d*)//(?<a2>[0-9]\d*) (?<b1>[0-9]\d*)//(?<b2>[0-9]\d*) (?<c1>[0-9]\d*)//(?<c2>[0-9]\d*)");
                var match = regex.Match(inputTriangleIndexLines[i]);

                builder.Append(match.Groups["a1"] + ", ");
                builder.Append(match.Groups["b1"] + ", ");
                builder.Append(match.Groups["c1"]);
                // 8, 7, 5, 8, 5, 6,

                if (i < inputTriangleIndexLines.Count - 1)
                {
                    builder.Append(",");
                }

                outputLines.Add(builder.ToString());
            }

            outputLines.Add("}");
            outputLines.Add("};");

            outputLines.Add("{");
            outputLines.Add("normals = new List<Vector3>");
            outputLines.Add("{");

            for (var i = 0; i < inputNormalLines.Count; i++)
            {
                var builder = new StringBuilder();
                builder.Append("new Vector3(");

                var regex = new Regex(@"vn (?<x>-?[0-9]\d*(\.\d+)?) (?<y>-?[0-9]\d*(\.\d+)?) (?<z>-?[0-9]\d*(\.\d+)?)");
                var match = regex.Match(inputNormalLines[i]);

                builder.Append(match.Groups["x"].Value + "f, ");
                builder.Append(match.Groups["y"].Value + "f, ");
                builder.Append(match.Groups["z"].Value + "f");
                // new Vector3(-width / 2.0f, -height / 2.0f, -depth / 2.0f),

                builder.Append(")");

                if (i < inputNormalLines.Count - 1)
                {
                    builder.Append(",");
                }

                outputLines.Add(builder.ToString());
            }

            outputLines.Add("};");

            /*var normals = new List<Vector3>
            {
                new Vector3(1, 0, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, 0, 1),
                new Vector3(-1, 0, 0),
                new Vector3(0, -1, 0),
                new Vector3(0, 0, -1)
            };*/
            outputLines.Add("var normalIndices = new List<int>");
            outputLines.Add("{");

            for (var i = 0; i < inputTriangleIndexLines.Count; i++)
            {
                var builder = new StringBuilder();

                var regex = new Regex(@"f (?<a1>[0-9]\d*)//(?<a2>[0-9]\d*) (?<b1>[0-9]\d*)//(?<b2>[0-9]\d*) (?<c1>[0-9]\d*)//(?<c2>[0-9]\d*)");
                var match = regex.Match(inputTriangleIndexLines[i]);

                builder.Append(match.Groups["a2"] + ", ");
                builder.Append(match.Groups["b2"] + ", ");
                builder.Append(match.Groups["c2"]);
                // 8, 7, 5, 8, 5, 6,

                if (i < inputTriangleIndexLines.Count - 1)
                {
                    builder.Append(",");
                }

                outputLines.Add(builder.ToString());
            }

            outputLines.Add("};");

            File.WriteAllLines(outputFilePath, outputLines);
        }
    }
}
