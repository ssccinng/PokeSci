using SVPokeDex.ViewModel;

namespace SVPokeDex;

public partial class AppShell : Shell
{

    public AppShell()
    {
        BindingContext = this;
        InitializeComponent();
    }
}
