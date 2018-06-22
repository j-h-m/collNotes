using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PDSkeleton
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Help : ContentPage
	{
		public Help ()
		{
			InitializeComponent ();
		}

        public void btnHelp_Clicked(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri(@"https://j-h-m.github.io/pd-project-xamarin/"));
        }

    }
}