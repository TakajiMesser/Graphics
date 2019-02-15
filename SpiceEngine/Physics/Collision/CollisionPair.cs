namespace SpiceEngine.Physics.Collision
{
    public class CollisionPair
    {
        public int FirstEntityID { get; }
        public int SecondEntityID { get; }

        public CollisionPair(int firstEntityID, int secondEntityID)
        {
            FirstEntityID = firstEntityID;
            SecondEntityID = secondEntityID;
        }
    }
}
