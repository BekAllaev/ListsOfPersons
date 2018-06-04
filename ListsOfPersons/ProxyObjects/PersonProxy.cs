using System;
using ListsOfPersons.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Validation;

namespace ListsOfPersons.ProxyObjects
{
    public class PersonProxy : ValidatableModelBase
    {
        #region Constructor
        public PersonProxy(Person person)
        {
            if (person == null)
                return;
            Name = person.Name;
            Id = person.Id;
            LastName = person.LastName;
            Email = person.Email;
            Notes = person.Notes;
            PathToImage = person.PathToImage;
            IsFavorite = person.IsFavorite;
            DateOfBirth = person.DateOfBirth;
        }
        #endregion

        #region Initial properties
        public string Name
        {
            set { Write<string>(value); }
            get { return Read<string>(); }
        }

        public string Id
        {
            set { Write<string>(value); }
            get { return Read<string>(); }
        }


        public string LastName
        {
            set { Write<string>(value); }
            get { return Read<string>(); }
        }

        public string Email
        {
            set { Write<string>(value); }
            get { return Read<string>(); }
        }

        public string Notes
        {
            set { Write<string>(value); }
            get { return Read<string>(); }
        }

        public DateTime DateOfBirth
        {
            set { Write(value); }
            get { return Read<DateTime>(); }
        }

        private Uri _pathToImage;
        public Uri PathToImage
        {
            set { _pathToImage = value; }
            get { return _pathToImage; }
        }

        private bool _isFavorite;
        public bool IsFavorite
        {
            set { _isFavorite = value; }
            get { return _isFavorite; }
        }

        #endregion

        #region Calculated properties
        public string FullName
        {
            get { return $"{Name} {LastName}"; }
        }

        public Uri EmailUri
        {
            get { return new Uri($"mailto:{Email}"); }
        }

        public int Age
        {
            get { return DateTime.Now.Year - DateOfBirth.Year ; }
        }

        #endregion
    }
}
