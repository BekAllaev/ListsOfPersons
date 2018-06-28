using Windows.UI.Xaml;
using System.Threading.Tasks;
using ListsOfPersons.Services.SettingsServices;
using Windows.ApplicationModel.Activation;
using Template10.Controls;
using Template10.Common;
using GalaSoft.MvvmLight.Ioc;
using ListsOfPersons.Services.RepositoryService;
using ListsOfPersons.Models;
using System;
using System.Linq;
using ListsOfPersons.Views;
using ListsOfPersons.ViewModels;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Controls;
using Template10.Services.NavigationService;
using ListsOfPersons.Services.DialogServices;
using ListsOfPersons.Services.TileServices;

namespace ListsOfPersons
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki

    [Bindable]
    sealed partial class App : BootStrapper
    {
        public App()
        {
            InitializeComponent();
            SplashFactory = (e) => new Views.Splash(e);

            #region app settings

            // some settings must be set in app.constructor
            var settings = SettingsService.Instance;
            RequestedTheme = settings.AppTheme;
            CacheMaxDuration = settings.CacheMaxDuration;
            ShowShellBackButton = settings.UseShellBackButton;

            #endregion
        }

        public override UIElement CreateRootElement(IActivatedEventArgs e)
        {
            var service = NavigationServiceFactory(BackButton.Attach, ExistingContent.Exclude);
            return new ModalDialog
            {
                DisableBackButtonWhenModal = true,
                Content = new Views.Shell(service),
                ModalContent = new Views.Busy(),
            };
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            // Registering dependency in container
            SimpleIoc.Default.Register<MasterDetailPageViewModel>();
            SimpleIoc.Default.Register<IRepositoryService<Person>, PersonsRepositoryServiceFake>();
            SimpleIoc.Default.Register<AddEditPageViewModel>();
            SimpleIoc.Default.Register<FavoritePageViewModel>();
            SimpleIoc.Default.Register<IDialogService, PersonDialogService>();
            SimpleIoc.Default.Register<PersonContentDialogViewModel>();
            SimpleIoc.Default.Register<ITileService, PersonTileServices>();

            // TODO: add your long-running task here
            await NavigationService.NavigateAsync(typeof(Views.MasterDetailPage));
        }

        public override INavigable ResolveForPage(Page page, NavigationService navigationService)
        {
            if (page is MasterDetailPage)
                return SimpleIoc.Default.GetInstance<MasterDetailPageViewModel>();
            else if (page is FavoritePage)
                return SimpleIoc.Default.GetInstance<FavoritePageViewModel>();
            else if (page is AddEditPage)
                return SimpleIoc.Default.GetInstance<AddEditPageViewModel>();
            else
                return base.ResolveForPage(page, navigationService);
        }
    }
}
