using OpenTK;
using SpiceEngine.Entities;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

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
