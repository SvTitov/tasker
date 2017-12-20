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
        public ObservableCollection<Task> Tasks
        {
            get => _tasks;
            set => SetProperty(ref _tasks, value);
        }

        readonly IContainer _container;
        private readonly INavigationService _navigationService;
        private ObservableCollection<Task> _tasks;
        private Task _task;

        public MainPageViewModel(IContainer container, INavigationService navigationService) 
            : base (navigationService)
        {
            this._container = container;
            _navigationService = navigationService;
            Title = "Главная";

            RefreshCommand = new DelegateCommand(Activate);
            AddCommand = new DelegateCommand(AddTask);
            LogoutCommand = new DelegateCommand(Logout);
        }

        private async void Edit(Task task)
        {
            await _navigationService.NavigateAsync($"EditTask?id={task.Guid}&edit=true");
        }

        private async  void Logout()
        {
            Settings.CurrentToken = string.Empty;

            await _navigationService.NavigateAsync("app:///NavigationPage/Login");
        }

        public Task SelectedItem
        {
            get => _task;
            set
            {
                _task = value;
                if (_task == null)
                    return;

                Edit(_task);

                SelectedItem = null;
            }
        }

        private void AddTask()
        {
            _navigationService.NavigateAsync("EditTask");
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            Activate();

            base.OnNavigatedTo(parameters);
        }

        public async void DeleteTask(string guid)
        {
            var result = await _container.Resolve<IRestRepository>()
                .DeleteTask(guid);

            RefreshCommand.Execute();
        }

        private async void Activate()
        {
            var repo = _container.Resolve<IRestRepository>();

            Tasks = new ObservableCollection<Task>(await repo.GetTasks());
        }


        public DelegateCommand RefreshCommand { get; set; }
        public DelegateCommand AddCommand { get; set; }
        public DelegateCommand LogoutCommand { get; set; }

    }
}
