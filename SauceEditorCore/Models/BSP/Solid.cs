using OpenTK;
using SpiceEngine.Rendering.Meshes;
using SpiceEngineCore.Physics.Shapes;
using System.Collections.Generic;

namespace SauceEditorCore.Models.BSP
{
    public class Solid : INode
    {
        public int ID { get; set; }
        public Vector3 Position { get; set; }
        public Shape3D Shape { get; set; }
        public ModelMesh Mesh { get; set; }

        /*
        Shape3D
            Represents a basic primitive shape, so that we can perform easy collision detections
            Has no position in world-space

            Implemented:
                Box - Width, Height, Depth
                Sphere - Radius
                Polyhedron - Vertices (list)

            To Do:
                Spheroid - 
                Cone - SideCount, Length
                Convex Hull - (this is a subset of Polyhedron, technically)

            Body3D
                Contains Shape3D with Position & Restitution
                Can be RigidBody3D, SoftBody3D, or StaticBody3D

        ModelBuilder
            Contains Positions, Normals, Tangents, UVs, BoneIDs, BoneWeights, and TriangleIndices
            Takes in an IModelShape
            Spits out vertices and triangle indices

            ModelMesh
                Contains Faces (ModelFace), UVMap

            ModelFace
                Contains Vertices (ModelVertex), Normal, Tangent, UVMap

            ModelTriangle
                Contains VertexA, VertexB, VertexC (ModelVertex), Normal, Tangent

            ModelVertex
                Contains Position, Normal, Tangent, UV, Color, BoneIDs, BoneWeights, and Origin

        MeshBatch
            Contains Mesh, offsets by EntityID, and counts by EntityID
            Contains data for multiple meshes

            Mesh
                Vertices (list), TriangleIndices, Alpha (also contains vertex buffer, index buffer, etc.)
        

        */

        public IEnumerable<int> GetIDs()
        {
            yield return ID;
        }

        public IEnumerable<Face> GetFaces()
        {
            yield return null;
        }

        //Shape3D ToShape();
    }
}
