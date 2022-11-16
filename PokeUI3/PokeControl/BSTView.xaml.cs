using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using PokeUI3.MVVM.Model;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PokeUI3.PokeControl
{
    public sealed partial class BSTView : UserControl
    {
        public BSTView()
        {
            this.InitializeComponent();
            //this.DataContext = this;
        }
        public static DependencyProperty HPProperty =
        DependencyProperty.Register("HP", typeof(int), typeof(BSTView), new PropertyMetadata(null));



        public int HP
        {
            get
            {
                return (int)GetValue(HPProperty);
            }
            set
            {
                SetValue(HPProperty, value);
            }
        }



        public int Atk
        {
            get
            {
                return (int)GetValue(AtkProperty);
            }
            set
            {
                SetValue(AtkProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Atk.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AtkProperty =
            DependencyProperty.Register("Atk", typeof(int), typeof(BSTView), new PropertyMetadata(0));




        public int Def
        {
            get
            {
                return (int)GetValue(DefProperty);
            }
            set
            {
                SetValue(DefProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Def.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefProperty =
            DependencyProperty.Register("Def", typeof(int), typeof(BSTView), new PropertyMetadata(0));



        public int Spa
        {
            get
            {
                return (int)GetValue(SpaProperty);
            }
            set
            {
                SetValue(SpaProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Spa.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SpaProperty =
            DependencyProperty.Register("Spa", typeof(int), typeof(BSTView), new PropertyMetadata(0));




        public int Spd
        {
            get
            {
                return (int)GetValue(SpdProperty);
            }
            set
            {
                SetValue(SpdProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Spd.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SpdProperty =
            DependencyProperty.Register("Spd", typeof(int), typeof(BSTView), new PropertyMetadata(0));



        public int Spe
        {
            get
            {
                return (int)GetValue(SpeProperty);
            }
            set
            {
                SetValue(SpeProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Spe.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SpeProperty =
            DependencyProperty.Register("Spe", typeof(int), typeof(BSTView), new PropertyMetadata(0));





        public BSTValue BSTValue
        {
            get
            {
                return (BSTValue)GetValue(BSTValueProperty);
            }
            set
            {
                SetValue(BSTValueProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for BSTValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BSTValueProperty =
            DependencyProperty.Register("BSTValue", typeof(BSTValue), typeof(BSTView), new PropertyMetadata(null));





        //{
        //    get { return (BSTValue)GetValue(BSTValueProperty); }
        //    set { SetValue(BSTValueProperty, value); }
        //}
        //public int HP { get; set; } = 100;

    }
}
