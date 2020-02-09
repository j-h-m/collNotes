namespace collNotes.Data.Models
{
    public enum MenuItemType
    {
        About,
        Settings,
        ExportImport,
        Trips,
        Sites,
        Specimen
    }

    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}