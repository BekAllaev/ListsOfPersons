using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.StartScreen;

namespace ListsOfPersons.Services.TileServices
{
    /// <summary>
    /// Поведение обозначаюшие работу над плиткой UWP
    /// 1). Закрепить плитку на СТАРТ меню
    /// 2). Открепить плитку от СТАРТ меню
    /// 3). Проверить прикреплена ли плитка
    /// </summary>
    public interface ITileService
    {
        void RequestCreate(SecondaryTile tile);
        void RequestDelete(SecondaryTile tile);
        void Exists(SecondaryTile tile);
    }
}
