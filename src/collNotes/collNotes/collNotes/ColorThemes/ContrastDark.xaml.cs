
using Xamarin.Forms;

namespace collNotes.ColorThemes
{
    public partial class ContrastDark : ResourceDictionary
    {
        public static Color PageBackgroundColor = Color.FromHex("616161");
        public static Color SecondaryBackgroundColor = Color.FromHex("373737");
        public static Color NavigationBarColor = Color.FromHex("6002EE");

        public static Color PrimaryColor = Color.FromHex("6002EE");
        public static Color SecondaryColor = Color.FromHex("d602ee");

        public static Color PrimaryTextColor = Color.White;
        public static Color SecondaryTextColor = Color.White;
        public static Color TertiaryTextColor = Color.WhiteSmoke;

        public static Color PrimaryColorLight = Color.FromHex("9c47ff");
        public static Color PrimaryColorDark = Color.FromHex("0000ba");

        public static Color SecondaryColorLight = Color.FromHex("ff5bff");
        public static Color SecondaryColorDark = Color.FromHex("a000bb");

        public ContrastDark()
        {
            InitializeComponent();
        }
    }
}