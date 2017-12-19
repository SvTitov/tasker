using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Tasker.Models.System;
using DryIoc;
using Tasker.Repositories;

namespace Tasker.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public ObservableCollection<Task> Tasks { get; set; }

        readonly IContainer _container;

        public MainPageViewModel(IContainer container, INavigationService navigationService) 
            : base (navigationService)
        {
            this._container = container;
            Title = "Main Page";

            System.Threading.Tasks.Task.Factory.StartNew(Activate);
        }

        private async void Activate()
        {
            var repo = _container.Resolve<IRestRepository>();

            var result = await repo.GetTasks();
        }
    }
}
