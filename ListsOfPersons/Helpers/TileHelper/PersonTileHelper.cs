using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.StartScreen;
using System.Collections.ObjectModel;

namespace ListsOfPersons.Helpers.TileHelper
{
    public class PersonTileHelper : ITileHelper
    {
        private ObservableCollection<SecondaryTile> tiles = new ObservableCollection<SecondaryTile>();

        #region Implementation of ITileService
        /// <summary>
        /// Add tile to START menu
        /// </summary>
        /// <param name="tile">
        /// Tile that add on menu
        /// </param>
        /// <returns></returns>
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
        /// Id of person that shown on tile
        /// </param>
        /// <returns></returns>
        public async void RequestDelete(string personId)
        {
            var currentTile = tiles.First(a => a.Arguments == personId);

            if (SecondaryTile.Exists(currentTile.TileId))
            {
                await currentTile.RequestDeleteAsync();
                tiles.Remove(currentTile);
            }
        }

        /// <summary>
        /// Check exists tile or not
        /// </summary>
        /// <param name="personId">
        /// Id of person that shown on tile
        /// </param>
        /// <returns></returns>
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
        #endregion
    }
}
