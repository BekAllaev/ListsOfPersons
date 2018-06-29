using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;
using ListsOfPersons.Services.RepositoryService;
using ListsOfPersons.Models;

namespace ListsOfPersons.ViewModels
{
    public class PersonTileViewViewModel : ViewModelBase
    {
        #region Fields
        IRepositoryService<Person> _personsRepositary;
        Person _showedPerson;
        #endregion

        #region Constructor
        public PersonTileViewViewModel(IRepositoryService<Person> personsRepositary)
        {
            _personsRepositary = personsRepositary;
        }
        #endregion

        #region Navigation events
        public async override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var Id = parameter as string;
            ShowedPerson = await _personsRepositary.GetByIdAsync(Id);
        }
        #endregion

        #region Bindable properties
        public Person ShowedPerson
        {
            set { Set(ref _showedPerson, value); }
            get { return _showedPerson; }
        }
        #endregion
    }
}
