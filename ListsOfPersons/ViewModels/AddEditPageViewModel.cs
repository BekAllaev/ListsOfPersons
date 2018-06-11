using System;
using ListsOfPersons.ProxyObjects;
using System.Collections.Generic;
using ListsOfPersons.Services;
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
        DateTime _dateTime;
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

        public DateTime DateOfbirth
        {
            set { Set(ref _dateTime, value); }
            get { return _dateTime; }
        }
        #endregion

        #region Navigation events
        public async override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            currentPerson = parameter == null ? new Person() { DateOfBirth=DateOfbirth, Id = Guid.NewGuid().ToString() } : parameter as Person;

            var temp = new PersonProxy(currentPerson)
            {
                Name = currentPerson.Name,
                LastName = currentPerson.LastName,
                Email = currentPerson.Email,
                DateOfBirth = currentPerson.DateOfBirth,
                Validator = i =>
                {
                    var u = i as PersonProxy;
                    if (string.IsNullOrEmpty(u.Name))
                        u.Properties[nameof(u.Name)].Errors.Add("FirstName is required");
                    else if (u.Name.Length < 3)
                        u.Properties[nameof(u.Name)].Errors.Add("FirstName must be more then 3 symbols");
                    if (string.IsNullOrEmpty(u.LastName))
                        u.Properties[nameof(u.LastName)].Errors.Add("FirstName is required");
                    if (string.IsNullOrEmpty(u.Email))
                        u.Properties[nameof(u.Email)].Errors.Add("Email is required");
                    else if (!new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(u.Email))
                        u.Properties[nameof(u.Email)].Errors.Add("Must consist . and @");
                }
            };
            TempPerson = temp;
            TempPerson.Validate();
            
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
            currentPerson.DateOfBirth = TempPerson.DateOfBirth; 
            currentPerson.PathToImage = TempPerson.PathToImage;

            if (CurrentState == States.Add)
                await PersonsRepositary.AddAsync(currentPerson);
            else if (CurrentState == States.Edit)
                await PersonsRepositary.UpdateAsync(currentPerson);

            await NavigationService.NavigateAsync(typeof(Views.MasterDetailPage));
        }
        #endregion
    }
}
