using System;
using DomenModel.ProxyObjects;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomenModel.Models;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;
using RepositoryService;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Controls;

namespace ListsOfPersons.ViewModels
{
    public class AddEditPageViewModel : ViewModelBase
    {
        #region Fields
        string _title;
        Person currentPerson;
        PersonProxy proxyPerson;
        IRepositoryService<Person> PersonsRepositary;
        enum States { Edit, Add };
        States CurrentState;
        #endregion

        #region Constructor
        public AddEditPageViewModel(IRepositoryService<Person> personsRepositary)
        {
            PersonsRepositary = personsRepositary;
        }
        #endregion

        #region ViewModel properties
        private bool IsCanceling { set; get; } //Flag that shows was view canceled by clicking CANCEL or SAVE button or not
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

        private int _day = 1;
        public int Day
        {
            set
            {
                _day = value;
                //if (_year == 0 | _month == 0)
                //    TempPerson.DateOfBirth = new DateTime(1, 1, _day);
                //else
                TempPerson.DateOfBirth = new DateTime(_year, _month, _day);
            }
            get { return TempPerson.DateOfBirth.Day; }
        }

        private int _month = 1;
        public int Month
        {
            set
            {
                _month = value;
                //if (_year == 0 | _day == 0)
                //    TempPerson.DateOfBirth = new DateTime(1, _month, 1);
                //else
                TempPerson.DateOfBirth = new DateTime(_year, _month, _day);
            }
            get { return TempPerson.DateOfBirth.Month; }
        }

        private int _year = 1;
        public int Year
        {
            set
            {
                _year = value;
                //if (_day == 0 | _month == 0)
                //    TempPerson.DateOfBirth = new DateTime(_year, 1, 1);
                //else
                TempPerson.DateOfBirth = new DateTime(_year, _month, _day);
            }
            get { return TempPerson.DateOfBirth.Year; }
        }

        #region Calculated properties
        public Int32[] DaysAmount
        {
            get { return Counter(31); }
        }

        public Int32[] MonthAmount
        {
            get { return Counter(12); }
        }

        public Int32[] YearsAmount
        {
            get { return Counter(1940, 2001); }
        }
        #endregion

        #region Overloads of Counter func
        private Int32[] Counter(int a)
        {
            Int32[] ProxyList = new Int32[a];
            for (int i = 0; i != a; i++)
                ProxyList[i] = i + 1;
            return ProxyList;
        }

        private Int32[] Counter(int firstYear, int lastYear)
        {
            int capacity = lastYear - firstYear + 1; //When you minus first from last year there is no place for lastyear
            Int32[] ProxyList = new Int32[capacity]; //it cause we add 1.
            ProxyList[0] = firstYear;
            for (int i = 1; i != capacity; i++)
                ProxyList[i] = firstYear + i;
            return ProxyList;
        }
        #endregion
        #endregion

        #region Navigation events
        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (parameter == null)
            {
                if (CurrentState == States.Edit)
                    TempPerson = null;

                if (TempPerson != null)
                    return Task.CompletedTask;
            }
            else
            {
                if (CurrentState == States.Add)
                    TempPerson = null;

                if (currentPerson != null && currentPerson.Id == (parameter as Person).Id)
                    return Task.CompletedTask;
            }

            currentPerson = parameter == null ? new Person() { Id = Guid.NewGuid().ToString() } : parameter as Person;

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
                Title = "Adding new person";
            }
            else
            {
                CurrentState = States.Edit;
                Title = $"Editing {TempPerson.FullName}";
            }

            return Task.CompletedTask;
        }

        public override Task OnNavigatedFromAsync(IDictionary<string, object> pageState, bool suspending)
        {
            if (IsCanceling)
            {
                IsCanceling = false;
                TempPerson = null;
            }

            return Task.CompletedTask;
        }
        #endregion

        #region Navigation tasks
        public async void GotoBackUnSaved()
        {
            IsCanceling = true;
            await NavigationService.NavigateAsync(typeof(Views.MasterDetailPage));
        }

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

            IsCanceling = true;

            await NavigationService.NavigateAsync(typeof(Views.MasterDetailPage));
        }
        #endregion
    }
}
