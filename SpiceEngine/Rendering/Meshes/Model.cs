using SpiceEngine.Rendering.Vertices;
using System;
using System.Collections.Generic;

namespace SpiceEngine.Rendering.Meshes
{
    public class Model
    {
        private List<IMesh> _meshes = new List<IMesh>();
        public IEnumerable<IMesh> Meshes => _meshes;

        public void Add(IMesh mesh)
        {
            _meshes.Add(mesh);
        }

        public void Load()
        {
            foreach (var mesh in _meshes)
            {
                mesh.Load();
            }
        }

        public Model Duplicate()
        {
            var model = new Model();

            foreach (var mesh in _meshes)
            {
                model.Add(mesh.Duplicate());
            }

            return model;
        }
    }
}
