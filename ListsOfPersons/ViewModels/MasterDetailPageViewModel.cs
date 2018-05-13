using System;
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
        Person _selectedPerson;
        #endregion

        #region Constructor
        public MasterDetailPageViewModel(IRepositoryService<Person> personsList)
        {
            _personsRepositary = personsList;
            _deletePersonCommand = new DelegateCommand(DeletePersonExecute, CanDeletePerson);
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
                DeletePersonCommand.RaiseCanExecuteChanged();
            }
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
        #region DeletCommand
        public DelegateCommand DeletePersonCommand
        {
            get { return _deletePersonCommand ?? new DelegateCommand(DeletePersonExecute, CanDeletePerson); }
        }

        private async void DeletePersonExecute()
        {
            ContentDialog OkCancelDialog = new ContentDialog
            {
                Title = $"Deleting {SelectedPerson.Name} {SelectedPerson.LastName}",
                Content = $"If agree press OK, otherwise Cancel",
                PrimaryButtonText = "OK",
                SecondaryButtonText = "Cancel"
            };
            ContentDialogResult result = await OkCancelDialog.ShowAsync();

            try
            {
                if (result == ContentDialogResult.Primary)
                    await _personsRepositary.DeleteAsync(SelectedPerson.Id);
                else
                    return;
            }
            catch (Exception e)
            {
                ContentDialog exceptionDialog = new ContentDialog
                {
                    Title = $"Invailid operation",
                    Content=e.Message,
                    PrimaryButtonText="OK"
                };
                await exceptionDialog.ShowAsync();
            }
        }

        private bool CanDeletePerson() => SelectedPerson == null ? false : true;
        #endregion
        #endregion

        #region Message handler
        private async void HandlePersonsChangedMessage(PersonsChangedMessage message)
        {
            switch (message.OperationType)
            {
                case CRUD.Delete:
                    Persons.Remove(SelectedPerson);
                    break;
                default:
                    await new MessageDialog(message.ExceptionMessage).ShowAsync();
                    break;
            }
        }
        #endregion
    }

}
