using System;
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

namespace ListsOfPersons.ViewModels
{
    public class MasterDetailPageViewModel : ViewModelBase
    {
        #region Fields
        ObservableCollection<Person> _persons;
        IRepositoryService<Person> _personsRepositary;
        #endregion

        #region Constructor
        public MasterDetailPageViewModel(IRepositoryService<Person> personsList)
        {
            _personsRepositary = personsList;
        }
        #endregion

        #region Bindable properties
        public ObservableCollection<Person> Persons
        {
            get { return _persons; }
            set { Set(ref _persons, value); }
        }
        #endregion

        #region Navigation events
        /// <summary>
        /// Runs every time when navigated to the view
        /// </summary>
        /// <param name="parameter">
        /// 
        /// </param>
        /// <param name="mode">
        /// 
        /// </param>
        /// <param name="state"></param>
        /// <returns></returns>
        public async override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var list = await _personsRepositary.GetAllAsync();
            Persons = new ObservableCollection<Person>(list);

            await Task.CompletedTask;
        }
        #endregion
    }

}
