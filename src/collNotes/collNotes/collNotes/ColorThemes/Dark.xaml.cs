using Xamarin.Forms;

namespace collNotes.ColorThemes
{
    public partial class Dark : ResourceDictionary
    {
        public static Color PageBackgroundColor = Color.FromHex("616161");
        public static Color SecondaryBackgroundColor = Color.FromHex("373737");
        public static Color NavigationBarColor = Color.FromHex("388e3c");

        public static Color PrimaryColor = Color.FromHex("388e3c");
        public static Color SecondaryColor = Color.FromHex("388a8e");

        public static Color PrimaryTextColor = Color.White;
        public static Color SecondaryTextColor = Color.White;
        public static Color TertiaryTextColor = Color.WhiteSmoke;

        public static Color PrimaryColorLight = Color.FromHex("6abf69");
        public static Color PrimaryColorDark = Color.FromHex("00600f");

        public static Color SecondaryColorLight = Color.FromHex("6bbabe");
        public static Color SecondaryColorDark = Color.FromHex("005c61");

        public Dark()
        {
            InitializeComponent();
        }
    }
}