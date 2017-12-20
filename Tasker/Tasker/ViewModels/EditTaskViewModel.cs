using System;
using Prism.Commands;
using Prism.Navigation;

namespace Tasker.ViewModels
{
    public class EditTaskViewModel : ViewModelBase
    {
        readonly INavigationService navigationService;
        private string _data;
        private DateTime _date = DateTime.Now;

        public EditTaskViewModel(INavigationService navigationService)
            :base(navigationService)
        {
            this.navigationService = navigationService;

            RefreshCommand = new DelegateCommand(AddTask);
        }

        private void AddTask()
        {
            
        }

        public string Data 
        {
            get => _data;
            set => SetProperty(ref _data, value);
        }

        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("id") && parameters.ContainsKey("edit"))
            {
                var param = parameters["id"].ToString();
            }
                           

            base.OnNavigatedTo(parameters);
        }

        public DelegateCommand RefreshCommand { get; set; }
    }
}
