using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ListsOfPersons.Services.TileServices;
using Windows.UI.StartScreen;
using System.Collections.ObjectModel;

namespace ListsOfPersons.Services.TileServices
{
    public class PersonTileServices : ITileService
    {
        private ObservableCollection<SecondaryTile> tiles = new ObservableCollection<SecondaryTile>();

        public async Task<bool> RequestCreate(SecondaryTile tile)
        {
            var IsPinned = await tile.RequestCreateAsync();
            if (IsPinned)
                tiles.Add(tile);
            return IsPinned;
        }

        /// <summary>
        /// Remove tile from START menu
        /// </summary>
        /// <param name="personId">
        /// Persons Id that show on tile
        /// </param>
        /// <returns></returns>
        public async void RequestDelete(string personId)
        {
            var currentTile = tiles.First(a => a.Arguments == personId);

            await currentTile.RequestDeleteAsync();
            tiles.Remove(currentTile);
        }

        public bool Exists(string personId)
        {
            if (tiles.Count != 0)
            {
                var currentTile = tiles.FirstOrDefault(a => a.Arguments == personId);
                if (currentTile == null)
                    return false;
            }
            else
                return false;
            return true;
        }
    }
}
