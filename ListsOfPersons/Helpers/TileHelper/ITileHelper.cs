using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.StartScreen;

namespace ListsOfPersons.Helpers.TileHelper
{
    /// <summary>
    /// Поведение обозначаюшие работу над плиткой UWP
    /// 1). Закрепить плитку на СТАРТ меню
    /// 2). Открепить плитку от СТАРТ меню
    /// 3). Проверить прикреплена ли плитка
    /// </summary>
    public interface ITileHelper
    {
        Task<bool> RequestCreate(SecondaryTile tile);

        void RequestDelete(string id);

        bool Exists(string id);
    }
}
