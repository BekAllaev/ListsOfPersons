using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.StartScreen;

namespace ListsOfPersons.Services.TileServices
{
    public class TileDesigner
    {
        public TileDesigner(string FirstName, string LastName, string Id, Uri pathToLogo, string argument)
        {
            DisplayName = $"{FirstName} {LastName}";
            TileId = Id;
            Logo = pathToLogo;
            Argument = argument;
            CurrentTile = new SecondaryTile(TileId, DisplayName, Argument, Logo, TileSize.Square150x150);
        }

        private string DisplayName { get; }
        private string TileId { get; }
        private string Argument { get; } 
        private Uri Logo { get; }
        public SecondaryTile CurrentTile { get; }
    }
}
