using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ListsOfPersons.ProxyObjects;

namespace ListsOfPersons.Models
{
    /// <summary>
    /// Model of person
    /// </summary>
    public class Person
    {
        public string Name { set; get; }
        public string LastName { get; set; }
        public string Notes { set; get; }
        public string Email { set; get; }
        public Uri PathToImage { set; get; }
        public bool IsFavorite { set; get; }
        public string Id { set; get; }
        public DateTime DateOfBirth { set; get; }
        /// <summary>
        /// Instance of the current person over which you can commit an action(show details,edit his details etc.)
        /// </summary>
        public PersonProxy DetailViewPerson { set; get; }
    }
}
