using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Navigation;
using DryIoc;
using Tasker.Repositories;
using System.Net.Http;
using Prism.Services;

namespace Tasker.ViewModels
{
	public class RegistrationViewModel : ViewModelBase
	{
        private readonly INavigationService _navigationService;
	    private string _phone;
	    private string _code;
	    private bool _isSend = false;
	    private string _actionText = "Отправить";
        readonly IContainer _container;
        readonly IPageDialogService _dialogService;

        public RegistrationViewModel(IContainer container, INavigationService navigationService, IPageDialogService dialogService)
            : base(navigationService)
	    {
            this._dialogService = dialogService;
            this._container = container;
            _navigationService = navigationService;
            ActionCommand = new DelegateCommand(DoAction);
            Title = "Регистрация";
	    }

	    private async void DoAction()
	    {
            var repostiroty = _container.Resolve<IRegistrationRepository>();

            if (!IsSend)
            {
                var result = await repostiroty.Registration(Phone);

                if (!result.IsSuccessStatusCode)
                {
                    await _dialogService.DisplayAlertAsync("Ошибка", "Указанный номер уже используется", "Ok", "Отмена");
                }
                else
                {
                    IsSend = true;
                    ActionText = "Подтвердить";
                }
            }
            else
            {
                Confirm();
            }
        }

	    private async void Confirm()
	    {
	        var repostiroty = _container.Resolve<IRegistrationRepository>();

            var result = await repostiroty.Confirm(Phone, Code);
	        if (result.IsSuccessStatusCode)
	        {
	            await _navigationService.GoBackAsync();
            }
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
