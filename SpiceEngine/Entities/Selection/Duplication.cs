namespace SpiceEngine.Entities.Selection
{
    public class Duplication
    {
        public int OriginalID { get; private set; }
        public int DuplicatedID { get; private set; }

        public Duplication(int originalID, int duplicatedID)
        {
            OriginalID = originalID;
            DuplicatedID = duplicatedID;
        }
    }
}
