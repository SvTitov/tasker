using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Navigation;
using DryIoc;
using Tasker.Repositories;
using Prism.Services;

namespace Tasker.ViewModels
{
	public class LoginViewModel : ViewModelBase
	{
	    private string _phone;
	    private string _password;
        private readonly INavigationService _navigationService;
        private readonly IContainer _container;
        readonly IPageDialogService _dialogService;

        public LoginViewModel(IContainer container ,INavigationService navigationService, IPageDialogService dialogService)
            :base(navigationService)
        {
            this._dialogService = dialogService;
            this._container = container;
            NextCommand = new DelegateCommand(OnNext);
            RegistrationCommand = new DelegateCommand(OnRegistration);
            this._navigationService = navigationService;
        }

	    private async void OnRegistration()
	    {
	        await _navigationService.NavigateAsync("Registration");
	    }

	    private async void OnNext()
	    {
            var login = _container.Resolve<ILoginRepository>();

            if (string.IsNullOrWhiteSpace(Phone))
                return;
            
            var result = await login.Login(new Models.Output.LoginOutput { Phone = Phone });

            if (result != null)
            {
                Settings.CurrentToken = result.Token;
                await _navigationService.NavigateAsync("app:///NavigationPage/MainPage");
            }
            else
            {
                await _dialogService.DisplayAlertAsync("Ошибка", "Не удалось зайти :(", "Ok", "Отмена");
            }
        }

	    public string Phone
	    {
            get => _phone;
	        set => SetProperty(ref _phone, value);
	    }

	    public string Password
	    {
	        get => _password;
	        set => SetProperty(ref _password, value);
	    }

        public DelegateCommand NextCommand { get; set; }

        public DelegateCommand RegistrationCommand { get; set; }

    }
}
