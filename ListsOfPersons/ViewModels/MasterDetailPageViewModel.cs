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
using System.ComponentModel;
using System.Runtime.CompilerServices;

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
        Person _selectedPerson;
        //PersonProxy _detailPerson;
        int remainPersons;
        #endregion

        #region Constructor
        public MasterDetailPageViewModel(IRepositoryService<Person> personsList)
        {
            _personsRepositary = personsList;
            _deletePersonCommand = new DelegateCommand(DeletePersonExecute, CanDeletePerson);
            _editPersonCommand = new DelegateCommand(EditPersonExecute, CanEditPerson);
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
                _detailViewPerson = new PersonProxy(_selectedPerson);
                DeletePersonCommand.RaiseCanExecuteChanged();
                EditPersonCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Персона детали которые выводятся на экран
        /// </summary>
        /// Привязать DetailsView к DetailViewPerson 
        private PersonProxy _detailViewPerson;
        public PersonProxy DetailViewPerson
        {
            set { Set(ref _detailViewPerson, value); }
            get { return _detailViewPerson; }
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
            var list = await _personsRepositary.GetAllAsync();
            Persons = new ObservableCollection<Person>(list);

            Messenger.Default.Register<PersonsChangedMessage>(this, (message) => HandlePersonsChangedMessage(message));

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
