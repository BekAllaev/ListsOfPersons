using System;
using ListsOfPersons.ProxyObjects;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ListsOfPersons.Models;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;
using ListsOfPersons.Services.RepositoryService;

namespace ListsOfPersons.ViewModels
{
    public class AddEditPageViewModel : ViewModelBase
    {
        #region Fields
        string _title;
        Person currentPerson;
        PersonProxy proxyPerson;
        IRepositoryService<Person> PersonsRepositary;
        enum States { Edit,Add};
        States CurrentState;
        #endregion

        #region Constructor
        public AddEditPageViewModel(IRepositoryService<Person> personsRepositary)
        {
            PersonsRepositary = personsRepositary;
        }
        #endregion

        #region Bindable properties
        public string Title  
        {
            set { Set(ref _title, value); }
            get { return _title; }
        }

        public PersonProxy TempPerson
        {
            set { Set(ref proxyPerson, value); }
            get { return proxyPerson; }
        }
        #endregion

        #region Navigation events
        public async override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            currentPerson = parameter == null ? new Person() { Id = Guid.NewGuid().ToString() } : parameter as Person;

            var temp = new PersonProxy(currentPerson)
            {
                Name = currentPerson.Name,
                LastName = currentPerson.LastName,
                Email = currentPerson.Email
            };
            TempPerson = temp;
            
            if (parameter == null)
            {
                CurrentState = States.Add;
                Title = "Adding new item";
            }
            else
            {
                CurrentState = States.Edit;
                Title = $"Editing {TempPerson.FullName}";
            }

            await Task.CompletedTask;
        }
        #endregion

        #region Navigation tasks
        public async void GotoBackUnSaved() =>
            await NavigationService.NavigateAsync(typeof(Views.MasterDetailPage));

        public async void GotoBackSaved()
        {
            currentPerson.Name = TempPerson.Name;
            currentPerson.LastName = TempPerson.LastName;
            currentPerson.Notes = TempPerson.Notes;
            currentPerson.Email = TempPerson.Email;
            currentPerson.PathToImage = TempPerson.PathToImage;

            if (CurrentState == States.Add)
                await PersonsRepositary.AddAsync(currentPerson);

            await NavigationService.NavigateAsync(typeof(Views.MasterDetailPage));
        }
        #endregion
    }
}
