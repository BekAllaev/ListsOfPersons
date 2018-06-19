using System;
using Template10.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ListsOfPersons.Models;
using System.Threading.Tasks;
using ListsOfPersons.Services.RepositoryService;
using Windows.UI.Xaml.Navigation;

namespace ListsOfPersons.ViewModels
{
    public class FavoritePageViewModel:ViewModelBase
    {
        #region Fields
        private List<Person> _persons;
        private IRepositoryService<Person> _repositoryService;
        #endregion

        #region Constructor
        public FavoritePageViewModel(IRepositoryService<Person> repositoryService)
        {
            _repositoryService = repositoryService;
        }
        #endregion

        #region Bindable properties
        public List<Person> Persons
        {
            set { Set(ref _persons, value); }
            get { return _persons; }
        }
        #endregion

        #region Navigation events
        public async override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            Persons = await _repositoryService.GetAllFavoriteAsync();
        }
        #endregion
    }
}
