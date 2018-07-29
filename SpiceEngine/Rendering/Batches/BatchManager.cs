using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpiceEngine.Entities;
using SpiceEngine.Entities.Models;
using SpiceEngine.Rendering.Buffers;
using SpiceEngine.Rendering.Vertices;

namespace SpiceEngine.Rendering.Batches
{
    public class BatchManager
    {
        public List<Batch<IVertex>> _batches = new List<Batch<IVertex>>();

        public BatchManager()
        {

        }

        public void AddEntities(EntityManager entityManager)
        {
            foreach (var actor in entityManager.Actors)
            {
                switch (actor.Model)
                {
                    case SimpleModel simple:
                        simple.Meshes.SelectMany(m => m.Vertices);
                        break;
                    case AnimatedModel animated:
                        animated.Meshes.SelectMany(m => m.Vertices);
                        break;
                }
            }

            foreach (var brush in entityManager.Brushes)
            {
                //brush.Mesh.Vertices;
            }
        }

        public void RenderBatches()
        {
            foreach (var batch in _batches)
            {
                batch.Draw();
            }
        }
    }
}
