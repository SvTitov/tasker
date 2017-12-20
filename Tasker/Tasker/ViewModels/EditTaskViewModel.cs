using System;
using DryIoc;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Tasker.Repositories;

namespace Tasker.ViewModels
{
    public class EditTaskViewModel : ViewModelBase
    {
        private readonly IContainer _container;
        readonly INavigationService navigationService;
        private readonly IPageDialogService _dialogService;
        private string _data;
        private DateTime _date = DateTime.Now;
        private string _actionText;
        private string _currentGuid;

        public EditTaskViewModel(IContainer container, INavigationService navigationService, IPageDialogService dialogService)
            :base(navigationService)
        {
            _container = container;
            this.navigationService = navigationService;
            _dialogService = dialogService;

            RefreshCommand = new DelegateCommand(Action);
        }

        public string ActionText
        {
            get => _actionText;
            set => SetProperty(ref _actionText, value);
        }

        public bool IsEdit { get; set; }

        private async void Action()
        {
            var repository = _container.Resolve<IRestRepository>();
            if (IsEdit)
            {
                var result = await repository.UpdateTask(_currentGuid, Data, Date);
                if (result)
                    await _dialogService.DisplayAlertAsync("Сообщение", "Задача успешно Обновлена", "Ок", "Отмена");
                else
                    await _dialogService.DisplayAlertAsync("Ошибка", "Не удолось обновить задачу", "Ок", "Отмена");
            }
            else
            {
                bool result = await repository.AddTask(Data, Date, Guid.NewGuid().ToString());
                if (result)
                {
                    await _dialogService.DisplayAlertAsync("Сообщение", "Задача успешно добавлена", "Ок", "Отмена");
                    await navigationService.GoBackAsync();
                }
                else
                    await _dialogService.DisplayAlertAsync("Ошибка", "Не удолось создать задачу", "Ок", "Отмена");
            }
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

        public async override void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("id") && parameters.ContainsKey("edit"))
            {
                _currentGuid = parameters["id"].ToString();

                Title = "Обновить задачу";
                ActionText = "Обновить";

                var taskInput = await _container.Resolve<IRestRepository>().GetTask(_currentGuid);

                Data = taskInput.Data;
                Date = taskInput.Date;

                IsEdit = true;
            }
            else
            {
                Title = "Добавить задачу";

                ActionText = "Добавить";
                IsEdit = false;
            }
                           

            base.OnNavigatedTo(parameters);
        }

        public DelegateCommand RefreshCommand { get; set; }
    }
}
