using Xamarin.Forms;
using XF.Material.Forms.Resources;

namespace collNotes.ColorThemes
{
    public partial class Light_Default : ResourceDictionary
    {
        public static Color PageBackgroundColor = Color.White;
        public static Color SecondaryBackgroundColor = Color.WhiteSmoke;
        public static Color NavigationBarColor = Color.FromHex("388e3c");

        public static Color PrimaryColor = Color.FromHex("388e3c");
        public static Color SecondaryColor = Color.FromHex("388a8e");

        public static Color PrimaryTextColor = Color.Black;
        public static Color SecondaryTextColor = Color.White;
        public static Color TertiaryTextColor = Color.Gray;

        public static Color PrimaryColorLight = Color.FromHex("6abf69");
        public static Color PrimaryColorDark = Color.FromHex("00600f");

        public static Color SecondaryColorLight = Color.FromHex("6bbabe");
        public static Color SecondaryColorDark = Color.FromHex("005c61");

        public Light_Default()
        {
            InitializeComponent();
        }
    }
}