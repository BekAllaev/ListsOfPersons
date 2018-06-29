using System;
using ListsOfPersons.ProxyObjects;
using System.Collections.Specialized;
using ListsOfPersons.Messages;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ListsOfPersons.Services.RepositoryService;
using ListsOfPersons.Models;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Messaging;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Template10.Services.NavigationService;
using Windows.UI.StartScreen;
using ListsOfPersons.Services.TileServices;
using Template10.Common;
using System.Windows;

namespace ListsOfPersons.ViewModels
{
    public class MasterDetailPageViewModel : ViewModelBase
    {
        #region Fields
        ObservableCollection<Person> _persons;
        IRepositoryService<Person> _personsRepositary;
        DelegateCommand _deletePersonCommand;
        DelegateCommand _addPersonCommand;
        DelegateCommand _editPersonCommand;
        DelegateCommand _makeFavoriteCommand;
        DelegateCommand _pinCommand;
        ITileService _tileService;
        Person _selectedPerson;
        Symbol _pinSymbol;
        Symbol _favoriteSymbol;
        int remainPersons;
        bool IsPinned;
        SecondaryTile tileOnShow;
        #endregion

        #region Constructor
        public MasterDetailPageViewModel(IRepositoryService<Person> personsList, ITileService tileService)
        {
            _personsRepositary = personsList;
            _tileService = tileService;
            _favoriteSymbol = Symbol.Favorite;
            _pinSymbol = Symbol.Pin;
            _deletePersonCommand = new DelegateCommand(DeletePersonExecute, CanDeletePerson);
            _editPersonCommand = new DelegateCommand(EditPersonExecute, CanEditPerson);
            _makeFavoriteCommand = new DelegateCommand(MakeFavoriteExecute, CanMakeFavorite);
            _pinCommand = new DelegateCommand(PinExecute, CanPin);
            _addPersonCommand = new DelegateCommand(AddPersonExecute);
        }
        #endregion

        #region Bindable properties
        public ObservableCollection<Person> Persons
        {
            get { return _persons; }
            set { Set(ref _persons, value); }
        }

        public Person SelectedPerson
        {
            get { return _selectedPerson; }
            set
            {
                Set(ref _selectedPerson, value);
                if (SelectedPerson != null)
                    SelectedPerson.ProxyPerson = new PersonProxy(SelectedPerson);
                DeletePersonCommand.RaiseCanExecuteChanged();
                EditPersonCommand.RaiseCanExecuteChanged();
                MakeFavoriteCommand.RaiseCanExecuteChanged();
                PinCommand.RaiseCanExecuteChanged();
                if (SelectedPerson != null && _tileService.Exists(SelectedPerson.Id))
                    PinSymbol = Symbol.UnPin;
                else
                    PinSymbol = Symbol.Pin;
                if (SelectedPerson != null && SelectedPerson.IsFavorite == true)
                    FavoriteSymbol = Symbol.Favorite;
                else if (SelectedPerson != null && SelectedPerson.IsFavorite == false)
                    FavoriteSymbol = Symbol.UnFavorite;
            }
        }

        public Symbol FavoriteSymbol
        {
            set { Set(ref _favoriteSymbol, value); }
            get { return _favoriteSymbol; }
        }

        public Symbol PinSymbol
        {
            set { Set(ref _pinSymbol, value); }
            get { return _pinSymbol; }
        }
        #endregion

        #region Navigation events
        /// <summary>
        /// Runs every time when navigated to the view
        /// </summary>
        /// <param name="parameter">
        /// Object that pass from navigate from page
        /// </param>
        /// <param name="mode">
        /// Instance of NavigationMode enum
        /// </param>
        /// <param name="state"></param>
        /// <returns></returns>
        public async override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var argument = parameter as string;
            if(!string.IsNullOrEmpty(argument))
            {
                Person personOnTile = await _personsRepositary.GetByIdAsync(argument);
                await NavigationService.NavigateAsync(typeof(Views.AddEditPage), personOnTile);
            }

            var list = await _personsRepositary.GetAllAsync();
            Persons = new ObservableCollection<Person>(list);

            Messenger.Default.Register<PersonsChangedMessage>(this, (message) => HandlePersonsChangedMessage(message));

            await Task.CompletedTask;
        }

        public async override Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            FavoriteSymbol = Symbol.Favorite;
            PinSymbol = Symbol.Pin;
            await Task.CompletedTask;
        }
        #endregion

        #region Commands

        #region DeleteCommand
        public DelegateCommand DeletePersonCommand
        {
            get { return _deletePersonCommand ?? new DelegateCommand(DeletePersonExecute, CanDeletePerson); }
        }

        private async void DeletePersonExecute()
        {
            remainPersons = Persons.Count;
            ContentDialogResult result = ContentDialogResult.Primary;

            ContentDialog OkCancelDialog = new ContentDialog
            {
                Title = $"Deleting {SelectedPerson.Name} {SelectedPerson.LastName}",
                Content = $"If agree press OK, otherwise Cancel",
                PrimaryButtonText = "OK",
                SecondaryButtonText = "Cancel"
            };

            if (remainPersons != 1)
                result = await OkCancelDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
                await _personsRepositary.DeleteAsync(SelectedPerson.Id, remainPersons);
            else
                return;
        }

        private bool CanDeletePerson() => SelectedPerson == null ? false : true;
        #endregion

        #region PinCommand
        public DelegateCommand PinCommand
        {
            get { return _pinCommand ?? new DelegateCommand(PinExecute, CanPin); }
        }

        private async void PinExecute()
        {
            IsPinned = false;
            tileOnShow = new SecondaryTile(Guid.NewGuid().ToString(), $"{SelectedPerson.Name} {SelectedPerson.LastName}", SelectedPerson.Id, new Uri("ms-appx:///Assets/LogoForTile.png"), TileSize.Square150x150);

            if (PinSymbol == Symbol.UnPin)
            {
                _tileService.RequestDelete(SelectedPerson.Id);
                PinSymbol = Symbol.Pin;
            }
            else
                IsPinned = await _tileService.RequestCreate(tileOnShow);

            if (IsPinned)
                PinSymbol = Symbol.UnPin;
        }

        private bool CanPin() => SelectedPerson == null ? false : true;

        #endregion

        #region EditCommand
        public DelegateCommand EditPersonCommand
        {
            get { return _editPersonCommand ?? new DelegateCommand(EditPersonExecute, CanEditPerson); }
        }

        private void EditPersonExecute()
        {
            NavigationService.Navigate(typeof(Views.AddEditPage), SelectedPerson);
        }

        private bool CanEditPerson() => SelectedPerson == null ? false : true;
        #endregion

        #region AddCommand
        public DelegateCommand AddPersonCommand
        {
            get { return _addPersonCommand ?? new DelegateCommand(AddPersonExecute); }
        }

        private void AddPersonExecute()
        {
            NavigationService.Navigate(typeof(Views.AddEditPage), null);
        }
        #endregion

        #region MakeFavoriteCommand
        public DelegateCommand MakeFavoriteCommand
        {
            get { return _makeFavoriteCommand ?? new DelegateCommand(MakeFavoriteExecute, CanMakeFavorite); }
        }

        private bool CanMakeFavorite() => SelectedPerson == null ? false : true;

        private void MakeFavoriteExecute()
        {
            if (SelectedPerson != null)
                FavoriteSymbol = (SelectedPerson.IsFavorite == true) ? Symbol.UnFavorite : Symbol.Favorite;
            _personsRepositary.UpdateAsync(SelectedPerson);
        }
        #endregion
        #endregion

        #region Message handler
        private async void HandlePersonsChangedMessage(PersonsChangedMessage message)
        {
            if (!message.IsAvailable)
            {
                ContentDialog invailidOperationDialog = new ContentDialog
                {
                    Title = $"Invailid operation",
                    Content = $"Not availbale operation",
                    PrimaryButtonText = "OK"
                };
                await invailidOperationDialog.ShowAsync();
                return;
            }

            switch (message.OperationType)
            {
                case CRUD.Delete:
                    Persons.Remove(SelectedPerson);
                    break;
                case CRUD.Update:
                    SelectedPerson.IsFavorite = (FavoriteSymbol == Symbol.Favorite) ? true : false;
                    break;
                default:
                    ContentDialog notFoundDialog = new ContentDialog
                    {
                        Title = $"NOT FOUND",
                        Content = $"Unknown exception",
                        PrimaryButtonText = "OK"
                    };
                    await notFoundDialog.ShowAsync();
                    break;
            }
        }
        #endregion
    }
}
