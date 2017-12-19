using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Navigation;

namespace Tasker.ViewModels
{
	public class RegistrationViewModel : ViewModelBase
	{
        private readonly INavigationService _navigationService;
	    private string _phone;
	    private string _code;
	    private bool _isSend = false;
	    private string _actionText = "Отправить";


	    public RegistrationViewModel(INavigationService navigationService)
            : base(navigationService)
	    {
	        _navigationService = navigationService;
            ActionCommand = new DelegateCommand(DoAction);
	    }

	    private void DoAction()
	    {
	        IsSend = true;
	        ActionText = "Подтвердить";
	    }

	    public string Phone
	    {
	        get => _phone;
	        set => SetProperty(ref _phone, value);
	    }

	    public string Code
	    {
	        get => _code;
	        set => SetProperty(ref _code, value); 
        }

	    public bool IsSend
        {
	        get => _isSend;
	        set => SetProperty(ref _isSend, value); 
	    }

	    public string ActionText
	    {
	        get => _actionText;
	        set => SetProperty(ref _actionText, value);
	    }

        public DelegateCommand ActionCommand { get; set; }
    }
}
