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
        private Trip trip;
        private Site site;
        private Specimen specimen;

        public CollectingPage ()
        {
			InitializeComponent ();
		}

        // overloaded constructor that accepts a Project as an argument
        // this should be passed in MainPage once the Project is chosen from the prompt
        public CollectingPage (Project project)
        {
            this.project = project;
            InitializeComponent();
        }
	}
}