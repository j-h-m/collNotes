
using Xamarin.Forms;

namespace collNotes.ColorThemes
{
    public partial class ContrastDark : ResourceDictionary
    {
        public static readonly Color PageBackgroundColor = Color.FromHex("616161");
        public static readonly Color SecondaryBackgroundColor = Color.FromHex("373737");
        public static readonly Color NavigationBarColor = Color.FromHex("6002EE");

        public static readonly Color PrimaryColor = Color.FromHex("6002EE");
        public static readonly Color SecondaryColor = Color.FromHex("d602ee");

        public static readonly Color PrimaryTextColor = Color.White;
        public static readonly Color SecondaryTextColor = Color.White;
        public static readonly Color TertiaryTextColor = Color.WhiteSmoke;

        public static readonly Color PrimaryColorLight = Color.FromHex("9c47ff");
        public static readonly Color PrimaryColorDark = Color.FromHex("0000ba");

        public static readonly Color SecondaryColorLight = Color.FromHex("ff5bff");
        public static readonly Color SecondaryColorDark = Color.FromHex("a000bb");

        public ContrastDark()
        {
            InitializeComponent();
        }
    }
}