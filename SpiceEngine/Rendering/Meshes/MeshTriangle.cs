namespace SpiceEngine.Rendering.Meshes
{
    public struct MeshTriangle
    {
        public int VertexIndexA { get; set; }
        public int VertexIndexB { get; set; }
        public int VertexIndexC { get; set; }

        public MeshTriangle(int vertexIndexA, int vertexIndexB, int vertexIndexC)
        {
            VertexIndexA = vertexIndexA;
            VertexIndexB = vertexIndexB;
            VertexIndexC = vertexIndexC;
        }
    }
}
