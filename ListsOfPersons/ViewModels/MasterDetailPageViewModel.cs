using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ListsOfPersons.Services.RepositoryService;
using ListsOfPersons.Models;
using System.Collections.ObjectModel;
using Template10.Mvvm;

namespace ListsOfPersons.ViewModels
{
    class MasterDetailPageViewModel : ViewModelBase
    {
        #region Fields
        ObservableCollection<Person> _personsList;
        IRepositoryService<Person> _personRepository;
        #endregion

        #region Constructor
        public MasterDetailPageViewModel(IRepositoryService<Person> personsList)
        {
            _personRepository = personsList;
        }
        #endregion

        #region Bindable properties
        public ObservableCollection<Person> Persons
        {
            get { return _personsList; }
            set { Set(ref _personsList, value); }
        }
        #endregion
    }
}
