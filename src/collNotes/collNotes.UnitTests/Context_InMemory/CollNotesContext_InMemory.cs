using collNotes.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace collNotes.UnitTests.Context_InMemory
{
    public class CollNotesContext_InMemory
    {
        public CollNotesContext GetCollNotesContext_InMemory()
        {
            return new CollNotesContext(new DbContextOptionsBuilder<CollNotesContext>()
                .UseInMemoryDatabase(databaseName: "testing_db")
                .Options);
        }
    }
}