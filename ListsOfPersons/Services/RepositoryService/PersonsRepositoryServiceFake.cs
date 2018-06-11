using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ListsOfPersons.Models;
using GalaSoft.MvvmLight.Messaging;
using ListsOfPersons.Messages;


namespace ListsOfPersons.Services.RepositoryService
{
    class PersonsRepositoryServiceFake : IRepositoryService<Person>
    {
        private List<Person> _persons;

        private PersonsChangedMessage message;

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

        public Task<List<Person>> GetAllFavoriteAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Person> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Person person)
        {
            var _person = _persons.FirstOrDefault(p => p.Id == person.Id);

            _persons.Remove(_person);
            _persons.Add(person);
            _persons.Sort();

            await WritePersonsAsync();
        }
        #endregion

        #region Read/Write
        private async Task<List<Person>> ReadPersonsAsync()
        {
            return await Task.Run(() => _persons = new List<Person>
            {
                new Person{Id=Guid.NewGuid().ToString(), Name="Веста",LastName="Буркот",Email="vesta.burkot@mail.com", IsFavorite=true, DateOfBirth=new DateTime(1998,7,1),
                    Notes ="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur pharetra dictum nibh vel ornare. Donec sem urna, rhoncus sed cursus ac, aliquet at nisl. Pellentesque cursus et lacus vel porta. Morbi iaculis efficitur volutpat. Curabitur sit amet cursus nisl, ac suscipit mauris. Nulla a tellus a odio tincidunt maximus. Maecenas non eros lacus. Donec aliquam libero nec ex ullamcorper, in lobortis nibh dapibus. Mauris vehicula, tellus quis congue tincidunt, neque massa auctor ante, ut laoreet felis nisi id tellus. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nunc sed dapibus quam, in viverra arcu. Vivamus ut metus non magna viverra porttitor a ultrices dui.",
                    PathToImage = new Uri("http://100portraits.ru/wp-content/uploads/2017/08/Vesta_100Portraits-04.jpg")},
                new Person{Id=Guid.NewGuid().ToString(), Name="Андрей",LastName="Альтов",Email="andrey.altov@mail.com", IsFavorite=false,DateOfBirth=new DateTime(1950,7,1),
                    Notes ="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur pharetra dictum nibh vel ornare. Donec sem urna, rhoncus sed cursus ac, aliquet at nisl. Pellentesque cursus et lacus vel porta. Morbi iaculis efficitur volutpat. Curabitur sit amet cursus nisl, ac suscipit mauris. Nulla a tellus a odio tincidunt maximus. Maecenas non eros lacus. Donec aliquam libero nec ex ullamcorper, in lobortis nibh dapibus. Mauris vehicula, tellus quis congue tincidunt, neque massa auctor ante, ut laoreet felis nisi id tellus. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nunc sed dapibus quam, in viverra arcu. Vivamus ut metus non magna viverra porttitor a ultrices dui.",
                    PathToImage =new Uri ("http://100portraits.ru/wp-content/uploads/2017/05/Tatyana_Makarova-01.jpg")},
                new Person{Id=Guid.NewGuid().ToString(), Name="Ксения",LastName="Берелет",Email="ksenia.berelet@mail.com", IsFavorite=true,DateOfBirth=new DateTime(1998,7,1),
                    Notes ="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur pharetra dictum nibh vel ornare. Donec sem urna, rhoncus sed cursus ac, aliquet at nisl. Pellentesque cursus et lacus vel porta. Morbi iaculis efficitur volutpat. Curabitur sit amet cursus nisl, ac suscipit mauris. Nulla a tellus a odio tincidunt maximus. Maecenas non eros lacus. Donec aliquam libero nec ex ullamcorper, in lobortis nibh dapibus. Mauris vehicula, tellus quis congue tincidunt, neque massa auctor ante, ut laoreet felis nisi id tellus. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nunc sed dapibus quam, in viverra arcu. Vivamus ut metus non magna viverra porttitor a ultrices dui.",
                    PathToImage = new Uri ("http://100portraits.ru/wp-content/uploads/2017/08/Kseniya_Berelet_09w.jpg")},
                new Person{Id=Guid.NewGuid().ToString(), Name="Маргарита",LastName="Бабкина",Email="margarite.babkina@mail.com", IsFavorite=false,DateOfBirth=new DateTime(1998,7,1),
                    Notes ="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur pharetra dictum nibh vel ornare. Donec sem urna, rhoncus sed cursus ac, aliquet at nisl. Pellentesque cursus et lacus vel porta. Morbi iaculis efficitur volutpat. Curabitur sit amet cursus nisl, ac suscipit mauris. Nulla a tellus a odio tincidunt maximus. Maecenas non eros lacus. Donec aliquam libero nec ex ullamcorper, in lobortis nibh dapibus. Mauris vehicula, tellus quis congue tincidunt, neque massa auctor ante, ut laoreet felis nisi id tellus. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nunc sed dapibus quam, in viverra arcu. Vivamus ut metus non magna viverra porttitor a ultrices dui.",
                    PathToImage =new Uri("http://100portraits.ru/wp-content/uploads/2017/08/Margarita_100Portraits-16.jpg")},
                new Person{Id=Guid.NewGuid().ToString(), Name="Владимир", LastName="Тюрин", Email="vladimir.turin@mail.com", IsFavorite=true,DateOfBirth=new DateTime(1990,7,1),
                    Notes ="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur pharetra dictum nibh vel ornare. Donec sem urna, rhoncus sed cursus ac, aliquet at nisl. Pellentesque cursus et lacus vel porta. Morbi iaculis efficitur volutpat. Curabitur sit amet cursus nisl, ac suscipit mauris. Nulla a tellus a odio tincidunt maximus. Maecenas non eros lacus. Donec aliquam libero nec ex ullamcorper, in lobortis nibh dapibus. Mauris vehicula, tellus quis congue tincidunt, neque massa auctor ante, ut laoreet felis nisi id tellus. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nunc sed dapibus quam, in viverra arcu. Vivamus ut metus non magna viverra porttitor a ultrices dui.",
                    PathToImage =new Uri("http://100portraits.ru/wp-content/uploads/2017/08/Vladimir_100Portraits-06.jpg") },
                new Person{Id=Guid.NewGuid().ToString(), Name="Дарья",LastName="Митичашвили",Email="daria.michiashvili2@mail.com", IsFavorite=true,DateOfBirth=new DateTime(1998,7,1),
                    Notes ="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur pharetra dictum nibh vel ornare. Donec sem urna, rhoncus sed cursus ac, aliquet at nisl. Pellentesque cursus et lacus vel porta. Morbi iaculis efficitur volutpat. Curabitur sit amet cursus nisl, ac suscipit mauris. Nulla a tellus a odio tincidunt maximus. Maecenas non eros lacus. Donec aliquam libero nec ex ullamcorper, in lobortis nibh dapibus. Mauris vehicula, tellus quis congue tincidunt, neque massa auctor ante, ut laoreet felis nisi id tellus. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nunc sed dapibus quam, in viverra arcu. Vivamus ut metus non magna viverra porttitor a ultrices dui.",
                    PathToImage =new Uri("http://100portraits.ru/wp-content/uploads/2017/08/Darya_Mitichashvili_06w.jpg")}
            });
        }

        private async Task WritePersonsAsync()
        {
            await Task.CompletedTask;
        }

        #endregion
    }
}
