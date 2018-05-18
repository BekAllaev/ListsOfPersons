using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;

namespace ListsOfPersons.ViewModels
{
    public class AddEditPageViewModel : ViewModelBase
    {
        string _title;
        #region Bindable properties
        public string Title
        {
            set { Set(ref _title, value); }
            get { return _title; }
        }
        #endregion

        #region Navigation events
        public async override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (parameter == null)
                Title = "View for adding";
            else
                Title = "View for editing";

            await Task.CompletedTask;
        }
        #endregion
    }
}
