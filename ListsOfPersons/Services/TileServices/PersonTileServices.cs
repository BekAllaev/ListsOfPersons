using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ListsOfPersons.Services.TileServices;
using Windows.UI.StartScreen;

namespace ListsOfPersons.Services.TileServices
{
    public class PersonTileServices : ITileService
    {
        private bool IsPinned;
        private bool IsExsist;
        public async Task<bool> RequestCreate(SecondaryTile tile)
        {
            IsPinned = await tile.RequestCreateAsync();
            return IsPinned;
        }

        public async Task<bool> RequestDelete(SecondaryTile tile)
        {
            IsPinned= await tile.RequestDeleteAsync();
            return IsPinned;
        }

        public bool Exists(SecondaryTile tile)
        {
            string tileID = tile.TileId;
            IsExsist= SecondaryTile.Exists(tileID);
            return IsExsist;
        }
    }
}
