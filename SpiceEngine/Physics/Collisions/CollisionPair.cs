namespace SpiceEngine.Physics.Collisions
{
    public struct CollisionPair
    {
        public int FirstEntityID { get; }
        public int SecondEntityID { get; }

        public CollisionPair(int firstEntityID, int secondEntityID)
        {
            FirstEntityID = firstEntityID;
            SecondEntityID = secondEntityID;
        }

        public override bool Equals(object obj)
        {
            if (obj is CollisionPair collisionPair)
            {
                return FirstEntityID == collisionPair.FirstEntityID && SecondEntityID == collisionPair.SecondEntityID
                    || FirstEntityID == collisionPair.SecondEntityID && SecondEntityID == collisionPair.FirstEntityID;
            }

            return false;
        }

        public override int GetHashCode() => FirstEntityID.GetHashCode() ^ SecondEntityID.GetHashCode();
    }
}
