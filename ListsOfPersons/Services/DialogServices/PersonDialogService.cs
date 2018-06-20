using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using ListsOfPersons.Views.ContentDailogs;
using ListsOfPersons.Models;

namespace ListsOfPersons.Services.DialogServices
{
    public class PersonDialogService : IDialogService
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
