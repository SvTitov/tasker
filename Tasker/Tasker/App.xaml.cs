using Tasker.ViewModels;
using Tasker.Views;
using DryIoc;
using Prism.DryIoc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Tasker.Repositories;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Tasker
{
    public partial class App : PrismApplication
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            //await NavigationService.NavigateAsync("NavigationPage/EditTask?id=2&edit=true");
            await NavigationService.NavigateAsync("NavigationPage/Login");
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<NavigationPage>();
            Container.RegisterTypeForNavigation<MainPage>();
            Container.RegisterTypeForNavigation<Login>();
            Container.RegisterTypeForNavigation<Registration>();
            Container.RegisterTypeForNavigation<EditTask>();
            Container.Register<IRestRepository, RestRepository>();
            Container.Register<ILoginRepository, LoginRepository>();
            Container.Register<IRegistrationRepository, RegistrationRepository>();
        }
    }
}
