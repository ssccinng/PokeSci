using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SVDex.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PokemonDetail : ContentPage
	{
		public PokemonDetail ()
		{
			InitializeComponent ();
		}
	}
}