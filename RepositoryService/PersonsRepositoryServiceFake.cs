using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomenModel.Models;
using GalaSoft.MvvmLight.Messaging;
using Message;
using Windows.Graphics.Imaging;

namespace RepositoryService
{
    public class PersonsRepositoryServiceFake : IRepositoryService<Person>
    {
        #region Fields
        private List<Person> _persons;

        private PersonsChangedMessage message;
        #endregion

        #region Properties
        /// <summary>
        /// Used in test class process
        /// Functionality same as list that used in custom class
        /// </summary>
        public List<Person> TestList
        {
            set { _persons = value; }
            get { return _persons; }
        }
        #endregion

        #region Implementation of IRepositoryService
        public async Task AddAsync(Person person)
        {
            if (_persons == null) _persons = await ReadPersonsAsync();

            _persons.Add(person);

            await WritePersonsAsync();

            //message = new PersonsChangedMessage() { OperationType = CRUD.Create, IsAvailable = true };
            //Messenger.Default.Send<PersonsChangedMessage>(message);
        }

        /// <summary>
        /// Remove item from list
        /// </summary>
        /// <param name="id">
        /// ID of removing item
        /// </param>
        public async Task DeleteAsync(string id, int remainPerson)
        {
            if (remainPerson == 1)
                message = new PersonsChangedMessage() { OperationType = CRUD.Delete, IsAvailable = false };
            else
            {
                message = new PersonsChangedMessage() { OperationType = CRUD.Delete, IsAvailable = true };
                _persons.Remove(_persons.Find(a => a.Id == id));
            }

            try
            {
                await WritePersonsAsync();
            }
            catch (Exception)
            {
                return;
            }

            Messenger.Default.Send<PersonsChangedMessage>(message);
        }

        /// <summary>
        /// Return list of persons
        /// </summary>
        /// <returns>
        /// If the list is null, return Task that return a list of persons
        /// Otherwise return last readed list
        /// </returns>
        public async Task<List<Person>> GetAllAsync()
        {
            return _persons = _persons ?? await ReadPersonsAsync();
        }

        public async Task<List<Person>> GetAllFavoriteAsync()
        {
            return await Task.Run(() =>
                 (from person in _persons
                  where person.IsFavorite.Equals(true)
                  select person).ToList<Person>());
        }

        public async Task<Person> GetByIdAsync(string id)
        {
            return await Task.Run(() => _persons.Find(a => a.Id == id));
        }

        public async Task UpdateAsync(Person person)
        {
            var _person = _persons.FirstOrDefault(p => p.Id == person.Id);

            _persons.Remove(_person);
            _persons.Add(person);

            await WritePersonsAsync();

            var message = new PersonsChangedMessage() { OperationType = CRUD.Update, IsAvailable = true };
            Messenger.Default.Send<PersonsChangedMessage>(message);
        }
        #endregion

        //#region Methods of class
        //private 

        //#endregion

        #region Read/Write
        private async Task<List<Person>> ReadPersonsAsync()
        {
            return await Task.Run(() => _persons = new List<Person>
            {
                new Person{Id=Guid.NewGuid().ToString(), Name="Илон",LastName="Маск",Email="Ilon.mask@mail.com", IsFavorite=true, DateOfBirth=new DateTime(1988,7,1),
                    Notes ="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur pharetra dictum nibh vel ornare. Donec sem urna, rhoncus sed cursus ac, aliquet at nisl. Pellentesque cursus et lacus vel porta. Morbi iaculis efficitur volutpat. Curabitur sit amet cursus nisl, ac suscipit mauris. Nulla a tellus a odio tincidunt maximus. Maecenas non eros lacus. Donec aliquam libero nec ex ullamcorper, in lobortis nibh dapibus. Mauris vehicula, tellus quis congue tincidunt, neque massa auctor ante, ut laoreet felis nisi id tellus. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nunc sed dapibus quam, in viverra arcu. Vivamus ut metus non magna viverra porttitor a ultrices dui.",
                    PathToImage = new Uri("https://img3.eadaily.com/r650x400/o/2bf/314a320b25e137426740bea048d9e.jpg")},
                new Person{Id=Guid.NewGuid().ToString(), Name="Лев",LastName="Яшин",Email="Lev.yashin@mail.com", IsFavorite=false,DateOfBirth=new DateTime(1950,7,1),
                    Notes ="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur pharetra dictum nibh vel ornare. Donec sem urna, rhoncus sed cursus ac, aliquet at nisl. Pellentesque cursus et lacus vel porta. Morbi iaculis efficitur volutpat. Curabitur sit amet cursus nisl, ac suscipit mauris. Nulla a tellus a odio tincidunt maximus. Maecenas non eros lacus. Donec aliquam libero nec ex ullamcorper, in lobortis nibh dapibus. Mauris vehicula, tellus quis congue tincidunt, neque massa auctor ante, ut laoreet felis nisi id tellus. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nunc sed dapibus quam, in viverra arcu. Vivamus ut metus non magna viverra porttitor a ultrices dui.",
                    PathToImage =new Uri ("https://secrethistory.su/uploads/posts/2014-02/1391712110_lev-yashin.jpg")},
                new Person{Id=Guid.NewGuid().ToString(), Name="Кристиан",LastName="Стюарт",Email="Kristen.stewart@mail.com", IsFavorite=true,DateOfBirth=new DateTime(1998,7,1),
                    Notes ="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur pharetra dictum nibh vel ornare. Donec sem urna, rhoncus sed cursus ac, aliquet at nisl. Pellentesque cursus et lacus vel porta. Morbi iaculis efficitur volutpat. Curabitur sit amet cursus nisl, ac suscipit mauris. Nulla a tellus a odio tincidunt maximus. Maecenas non eros lacus. Donec aliquam libero nec ex ullamcorper, in lobortis nibh dapibus. Mauris vehicula, tellus quis congue tincidunt, neque massa auctor ante, ut laoreet felis nisi id tellus. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nunc sed dapibus quam, in viverra arcu. Vivamus ut metus non magna viverra porttitor a ultrices dui.",
                    PathToImage = new Uri ("https://my-hit.org/storage/1875060_500x800x250.jpg")},
                new Person{Id=Guid.NewGuid().ToString(), Name="Райан",LastName="Гослинг",Email="Ryan.gosling@mail.com", IsFavorite=false,DateOfBirth=new DateTime(1998,7,1),
                    Notes ="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur pharetra dictum nibh vel ornare. Donec sem urna, rhoncus sed cursus ac, aliquet at nisl. Pellentesque cursus et lacus vel porta. Morbi iaculis efficitur volutpat. Curabitur sit amet cursus nisl, ac suscipit mauris. Nulla a tellus a odio tincidunt maximus. Maecenas non eros lacus. Donec aliquam libero nec ex ullamcorper, in lobortis nibh dapibus. Mauris vehicula, tellus quis congue tincidunt, neque massa auctor ante, ut laoreet felis nisi id tellus. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nunc sed dapibus quam, in viverra arcu. Vivamus ut metus non magna viverra porttitor a ultrices dui.",
                    PathToImage =new Uri("http://games-of-thrones.ru/sites/default/files/pictures/all/gosling/31.jpg")},
                new Person{Id=Guid.NewGuid().ToString(), Name="Том", LastName="Хидделстон", Email="Tom.hiddleston@mail.com", IsFavorite=true,DateOfBirth=new DateTime(1990,7,1),
                    Notes ="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur pharetra dictum nibh vel ornare. Donec sem urna, rhoncus sed cursus ac, aliquet at nisl. Pellentesque cursus et lacus vel porta. Morbi iaculis efficitur volutpat. Curabitur sit amet cursus nisl, ac suscipit mauris. Nulla a tellus a odio tincidunt maximus. Maecenas non eros lacus. Donec aliquam libero nec ex ullamcorper, in lobortis nibh dapibus. Mauris vehicula, tellus quis congue tincidunt, neque massa auctor ante, ut laoreet felis nisi id tellus. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nunc sed dapibus quam, in viverra arcu. Vivamus ut metus non magna viverra porttitor a ultrices dui.",
                    PathToImage =new Uri("http://www.livestory.com.ua/images//Tom_Hiddlston_25.jpg") },
                new Person{Id=Guid.NewGuid().ToString(), Name="Килиан",LastName="Мёрфи",Email="Cilian.murphy@mail.com", IsFavorite=true,DateOfBirth=new DateTime(1998,7,1),
                    Notes ="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur pharetra dictum nibh vel ornare. Donec sem urna, rhoncus sed cursus ac, aliquet at nisl. Pellentesque cursus et lacus vel porta. Morbi iaculis efficitur volutpat. Curabitur sit amet cursus nisl, ac suscipit mauris. Nulla a tellus a odio tincidunt maximus. Maecenas non eros lacus. Donec aliquam libero nec ex ullamcorper, in lobortis nibh dapibus. Mauris vehicula, tellus quis congue tincidunt, neque massa auctor ante, ut laoreet felis nisi id tellus. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nunc sed dapibus quam, in viverra arcu. Vivamus ut metus non magna viverra porttitor a ultrices dui.",
                    PathToImage =new Uri("https://upload.wikimedia.org/wikipedia/commons/thumb/0/04/Cillian_Murphy_2014_cropped.jpg/1200px-Cillian_Murphy_2014_cropped.jpg")}
            });
        }

        private async Task WritePersonsAsync()
        {
            await Task.CompletedTask;
        }

        #endregion

    }
}
