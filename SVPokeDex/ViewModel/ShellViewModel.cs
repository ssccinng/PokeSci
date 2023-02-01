using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SVPokeDex.ViewModel;
public partial class ShellViewModel: ObservableObject
{
    [ObservableProperty]
    string _titleSource = "story_img_01.png";
}
