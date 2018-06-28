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
        public async void RequestCreate(SecondaryTile tile)
        {
            //string tileID = tile.TileId;
            //SecondaryTile tileToShow = new SecondaryTile(tileID);
            //await tileToShow.RequestCreateAsync();
            await tile.RequestCreateAsync();
        }

        public async void RequestDelete(SecondaryTile tile)
        {
            await tile.RequestDeleteAsync();
        }

        public void Exists(SecondaryTile tile)
        {
            string tileID = tile.TileId;
            SecondaryTile.Exists(tileID);
        }
    }
}
