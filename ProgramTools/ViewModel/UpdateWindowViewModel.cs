using CommunityToolkit.Mvvm.ComponentModel;

using ProgramTools.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramTools.ViewModel
{
    public partial class UpdateWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private IEnumerable<UpdatableInstalledApplication> _apps;

        public UpdateWindowViewModel(IEnumerable<UpdatableInstalledApplication> apps)
        {
            _apps = apps;
        }
    }
}
