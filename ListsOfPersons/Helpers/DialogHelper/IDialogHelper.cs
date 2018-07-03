using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace ListsOfPersons.Helpers.DialogHelper
{
    public interface IDialogHelper
    {
        Task<ContentDialogResult> ShowAsync(object entity);
    }
}
