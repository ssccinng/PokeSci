using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PokemonDataAccess.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PokeUI3.PokeControl
{
    public sealed partial class PokeInfoDisplay : UserControl
    {


        
        public string GetPokeType()
        {
            if (Pokemon.Type2 == null)
            {
                return Pokemon.Type1.Name_Chs;
            }
            else
            {
                return $"{Pokemon.Type1.Name_Chs} / {Pokemon.Type2.Name_Chs}";
            }
        }
        public string GetPokeAbility()
        {
            if (Pokemon.Ability2 == null)
            {
                return Pokemon.Ability1.Name_Chs;
            }
            else
            {
                return $"{Pokemon.Ability1.Name_Chs} / {Pokemon.Ability2.Name_Chs}";
            }
        }

        public Pokemon Pokemon
        {
            get { return (Pokemon)GetValue(PokemonProperty); }
            set { SetValue(PokemonProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Pokemon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PokemonProperty =
            DependencyProperty.Register("Pokemon", typeof(Pokemon), typeof(PokeInfoDisplay), new PropertyMetadata(null));


        public PokeInfoDisplay()
        {
            this.InitializeComponent();
        }
    }
}
