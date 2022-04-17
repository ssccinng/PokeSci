namespace PokeMaui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //Shell.Current.CurrentItem = PhoneTabs;
            //if (DeviceInfo.Idiom == DeviceIdiom.Phone)
            //    Shell.Current.CurrentItem = PhoneTabs;
        }

        private void PhoneTabs_Appearing(object sender, EventArgs e)
        {

        }
    }
}