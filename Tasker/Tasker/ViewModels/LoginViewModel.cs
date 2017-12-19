using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Navigation;

namespace Tasker.ViewModels
{
	public class LoginViewModel : ViewModelBase
	{
	    private string _phone;
	    private string _password;
        private readonly INavigationService _navigationService;

        public LoginViewModel(INavigationService navigationService)
            :base(navigationService)
        {
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
	        await _navigationService.NavigateAsync("app:///NavigationPage/MainPage");
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
