using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using DomenModel.Models;
using ListsOfPersons.Helpers.DialogHelper;
using RepositoryService;

//Нажимай на кнопку next и получай следуешего по списку
namespace ListsOfPersons.ViewModels
{
    public class PersonContentDialogViewModel : ViewModelBase
    {
        #region Constructor
        /// <summary>
        /// Injection of dialog helper
        /// </summary>
        public PersonContentDialogViewModel(IDialogHelper dialog=null, IRepositoryService<Person> repositoryService=null)
        {
            _dialog = dialog;
            _repositoryService = repositoryService;
        }
        #endregion

        #region Field
        Person _selectedPerson;
        IDialogHelper _dialog;
        IRepositoryService<Person> _repositoryService;
        #endregion

        #region Bindable properties
        public string FullName
        {
            get { return $"{SelectedPerson.Name} {SelectedPerson.LastName}"; }
        }

        public Person SelectedPerson
        {
            set { Set(ref _selectedPerson, value); }
            get { return _selectedPerson; }
        }
        #endregion

        #region Events handlers
        /// <summary>
        /// Show next favorite person in Content Dialog
        /// </summary>
        public async void ShowNext()
        {
            var listOfFavorite = await _repositoryService.GetAllFavoriteAsync();
            listOfFavorite.Find(a => SelectedPerson.Id == a.Id);

        }
        #endregion
    }
}

