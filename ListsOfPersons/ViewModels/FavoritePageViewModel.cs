using System;
using Template10.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TypesLibrary.Models;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using ServicesLibrary.RepositoryService;
using Windows.UI.Xaml.Navigation;
using ServicesLibrary.DialogServices;
using System.Collections.ObjectModel;

namespace ListsOfPersons.ViewModels
{
    public class FavoritePageViewModel : ViewModelBase
    {
        #region Fields
        ObservableCollection<Person> _persons;
        IRepositoryService<Person> _repositoryService;
        IDialogService _dialog;
        Person _selectedPerson;
        #endregion

        #region Constructor
        public FavoritePageViewModel(IRepositoryService<Person> repositoryService, IDialogService dialog)
        {
            _repositoryService = repositoryService;
            _dialog = dialog;
        }
        #endregion

        #region Bindable properties
        public ObservableCollection<Person> Persons
        {
            set { Set(ref _persons, value); }
            get { return _persons; }
        }

        public Person SelectedPeson
        {
            set { Set(ref _selectedPerson, value); }
            get { return _selectedPerson; }
        }
        #endregion

        #region Navigation events
        public async override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var persons = await _repositoryService.GetAllFavoriteAsync();
            Persons = new ObservableCollection<Person>(persons);
        }
        #endregion

        #region EventHandler
        public async void ShowDialog()
        {
            if (SelectedPeson == null)
                return;
            if (SelectedPeson.IsFavorite == true)
            {
                ContentDialogResult result = await _dialog.ShowAsync(SelectedPeson);
                if (SelectedPeson.IsFavorite == false)
                    Persons.Remove(SelectedPeson);
            }
        }
        #endregion
    }
}
