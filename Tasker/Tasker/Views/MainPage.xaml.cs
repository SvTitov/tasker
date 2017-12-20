using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasker.ViewModels;
using Xamarin.Forms;
using Task = Tasker.Models.System.Task;

namespace Tasker.Views
{
	public partial class MainPage : ContentPage
	{
		public MainPage ()
		{
			InitializeComponent ();
		}

	    public void OnDelete(object sender, EventArgs e)
	    {
	        var mi = ((MenuItem)sender);
	        ((MainPageViewModel)BindingContext).DeleteTask((mi.CommandParameter as Task)?.Guid);
	    }
    }
}