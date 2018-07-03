using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using DomenModel.Models;
using ListsOfPersons.Views.ContentDailogs;

namespace ListsOfPersons.Helpers.DialogHelper
{
    public class PersonDialogHelper : IDialogHelper
    {
        public ContentDialogResult DialogResult { get; private set; }

        public async Task<ContentDialogResult> ShowAsync(object entity)
        {
            var dialog = new PersonContentDialog();
            dialog.ViewModel.SelectedPerson = entity as Person;

            return DialogResult = await dialog.ShowAsync();
        }
    }
}
