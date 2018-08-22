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
	public partial class CollectingPage : TabbedPage
	{
        private Project project;

        public CollectingPage ()
		{
			InitializeComponent ();
		}

        public CollectingPage (Project project)
        {
            this.project = project;
            InitializeComponent();
        }
	}
}