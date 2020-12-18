using OpenTK;

namespace SweetGraphicsCore.Rendering.Meshes
{
    public struct UVMap
    {
        public Vector2 Translation { get; private set; }
        public float Rotation { get; private set; }
        public Vector2 Scale { get; private set; }

        public UVMap(Vector2 translation, float rotation, Vector2 scale)
        {
            Translation = translation;
            Rotation = rotation;
            Scale = scale;
        }

        /*public UVMap(IList<ITextureVertex> vertices)
        {
            var globalPositionToUVRatio = 0.0f;

            for (var i = 0; i < vertices.Count - 1; i++)
            {
                var positionChange = vertices[i + 1].Position - vertices[i].Position;
                var uvChange = vertices[i + 1].UV - vertices[i].UV;

                positionToUVRatio = positionChange / uvChange;

                if (i == 0)
                {
                    globalPositionToUVRatio = positionToUVRatio;
                }
                else if (globalPositionToUVRatio.IsSignificantDifference(positionToUVRatio))
                {

                    break;
                }
            }
        }*/

        public UVMap Duplicated() => new UVMap(Translation, Rotation, Scale);

        public UVMap Translated(Vector2 translation) => new UVMap(Translation + translation, Rotation, Scale);

        public UVMap Rotated(float rotation) => new UVMap(Translation, Rotation + rotation, Scale);

        public UVMap Scaled(Vector2 scale) => new UVMap(Translation, Rotation, Scale + scale);

        /*private bool IsTextureScaleUniform(IList<ITextureVertex> vertices)
        {
            var scaleX = 0.0f;
            var scaleY = 0.0f;

            // TODO - This will be a difficult algorithm to figure out, since the texture could be rotated...
            // We need to find the position change and UV change for each pair of points
            // THEN, we need to find the relationship between the positionChange and uvChange for that pair
            // THEN, we need to ensure that this relationship holds for all other pairs of points
            // How do we check this relationship? We need to compare each combination of components for P vs UV
            // We need this relationship to exist for TWO components of P and both components of UV
            // This relationship needs to be a scalar
            // Hmm, this 2/3 components matching deal requires the face to be axis-aligned... so this won't do
            float? xxChangeRatio;
            float? xyChangeRatio;
            float? yxChangeRatio;
            float? yyChangeRatio;
            float? zxChangeRatio;
            float? zyChangeRatio;

            for (var i = 0; i < vertices.Count - 1; i++)
            {
                var positionChange = vertices[i + 1].Position - vertices[i].Position;
                var uvChange = vertices[i + 1].UV - vertices[i].UV;

                var currentXXChangeRatio = positionChange.X / uvChange.X;
                var currentXYChangeRatio = positionChange.X / uvChange.Y;
                var currentYXChangeRatio = positionChange.Y / uvChange.X;
                var currentYYChangeRatio = positionChange.Y / uvChange.Y;
                var currentZXChangeRatio = positionChange.Z / uvChange.X;
                var currentZYChangeRatio = positionChange.Z / uvChange.Y;

                if (i == 0)
                {
                    xxChangeRatio = currentXXChangeRatio;
                    xyChangeRatio = currentXYChangeRatio;
                    yxChangeRatio = currentYXChangeRatio;
                    yyChangeRatio = currentYYChangeRatio;
                    zxChangeRatio = currentZXChangeRatio;
                    zyChangeRatio = currentZYChangeRatio;
                }
                else
                {
                    // We need to compare our current??ChangeRatio to the ??ChangeRatio, and we need at least two ratios to match
                    if (xxChangeRatio.HasValue && xxChangeRatio.Value.IsSignificantDifference(currentXXChangeRatio))
                    {
                        xxChangeRatio = null;
                    }

                    if (xyChangeRatio.HasValue && xyChangeRatio.Value.IsSignificantDifference(currentXYChangeRatio))
                    {
                        xyChangeRatio = null;
                    }

                    if (yxChangeRatio.HasValue && yxChangeRatio.Value.IsSignificantDifference(currentYXChangeRatio))
                    {
                        yxChangeRatio = null;
                    }

                    if (yyChangeRatio.HasValue && yyChangeRatio.Value.IsSignificantDifference(currentYYChangeRatio))
                    {
                        yyChangeRatio = null;
                    }

                    if (zxChangeRatio.HasValue && zxChangeRatio.Value.IsSignificantDifference(currentZXChangeRatio))
                    {
                        zxChangeRatio = null;
                    }

                    if (zyChangeRatio.HasValue && zyChangeRatio.Value.IsSignificantDifference(currentZYChangeRatio))
                    {
                        zyChangeRatio = null;
                    }
                }
            }

            // 
            if (xxChangeRatio.HasValue)
            {

            }

            for (var i = 0; i < vertices.Count - 1; i++)
            {
                var positionChange = vertices[i + 1].Position - vertices[i].Position;
                var uvChange = vertices[i + 1].UV - vertices[i].UV;

                // We want the POS distance vs UV distance between the two points to compare
                // Unfortunately, POS distance is in XYZ, while UV distance is in XY
                // How do we compare these two?
                var currentScaleX = positionChange.X;
                var currentScaleY = positionChange.Y;

                if (i == 0)
                {
                    scaleX = currentScaleX;
                    scaleY = currentScaleY;
                }
                else
                {
                    if (scaleX.IsSignificantDifference(currentScaleX))
                    {

                    }
                }
            }

                // Iterate through each pair of vertices, checking if the Texture Scale is uniform throughout
                for (var i = 0; i < vertices.Count - 1; i++)
                {
                    var positionChange = vertices[i + 1].Position - vertices[i].Position;
                    var uvChange = vertices[i + 1].UV - vertices[i].UV;

                    currentPositionToUVRatio = positionChange / uvChange;

                    if (i == 0)
                    {
                        positionToUVRatio = currentPositionToUVRatio;
                    }
                    else if (positionToUVRatio.IsSignificantDifference(currentPositionToUVRatio))
                    {
                        // In this case, we want to set the texture scale based on this uniform ratio
                        // Texture scale should be 1.0f if 1-1 between Position and UV
                        // Texture scale should be 2.0f if 2-1 between Position and UV
                        return true;
                    }
                }
        }*/

        public static UVMap Standard => new UVMap(Vector2.Zero, 0.0f, Vector2.One);
        public static UVMap Varying => new UVMap();
    }
}
