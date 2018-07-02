using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using TypesLibrary.Models;

namespace ViewModelsLibrary
{
    public class PersonContentDialogViewModel : ViewModelBase
    {
        #region Field
        Person _selectedPerson;
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
    }
}
