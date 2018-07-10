using RepositoryService;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using DomenModel.Models;

namespace RepositoryServiceTest
{
    //The only propertie of the item to be used
    enum ItemProperty
    {
        // All consts except "No" taken from model
        ID, 
        IsFavorite, 
        No // No one prop won`t use
    } 

    [TestClass]
    public class PersonsRepositoryServiceFakeTest
    {
        PersonsRepositoryServiceFake repository = new PersonsRepositoryServiceFake();

        const int Count = 1000;

        [TestMethod]
        public void NotRemoveIfOneRemainPerson()
        {
            List<Person> list = new List<Person>()
            {
                new Person(){ Id=Guid.NewGuid().ToString() }
            };
            repository.TestList = new List<Person>(list);

            var personId = list[0].Id;

            repository.DeleteAsync(personId, 1).Wait();

            Assert.AreNotEqual(0, repository.TestList.Count);
        }

        [TestMethod]
        public void GetAllAsyncTest()
        {
            List<Person> list = GetList(ItemProperty.No);

            repository.TestList = new List<Person>(list);

            List<Person> resultList = repository.GetAllAsync().Result;

            //TODO: Talk about what we compare
            Assert.AreEqual(Count, resultList.Count);
        }

        [TestMethod]
        public void GetAllFavoriteTest()
        {
            List<Person> list = new List<Person>()
            {
                new Person(){IsFavorite=false}
            };
            list.AddRange(GetList(ItemProperty.IsFavorite));

            repository.TestList = new List<Person>(list);
            List<Person> resultList = repository.GetAllFavoriteAsync().Result;

            Assert.AreEqual(resultList.Count, list.Count - 1);
        }

        [TestMethod]
        public void GetByIdTest()
        {
            List<Person> list = GetList(ItemProperty.ID);
            Random random = new Random();

            repository.TestList = new List<Person>(list);

            int randomIndex = random.Next(0, Count);
            string id = list[randomIndex].Id;
            Person searchingPerson = repository.GetByIdAsync(id).Result;

            Assert.AreEqual(searchingPerson, list[randomIndex]);
        }

        [TestMethod]
        public void UpdateAsyncTest()
        {
            List<Person> list = new List<Person>()
            {
                new Person(){Id=Guid.NewGuid().ToString(),Name="Jack"}
            };
            repository.TestList = new List<Person>(list);

            Person editingPerson = list[0];

            editingPerson.Name = "Bill";

            repository.UpdateAsync(editingPerson).Wait();

            int lastIndexItem = repository.TestList.Count - 1;

            Person expectedPerson = repository.TestList[lastIndexItem];
            Person actualPerson = editingPerson;

            Assert.AreEqual(expectedPerson, actualPerson);
        }

        /// <summary>
        /// Generate new List 
        /// </summary>
        /// <param name = "characteristic">
        /// Characteristic that would be use for comparing,searching etc 
        /// </param>
        private List<Person> GetList(ItemProperty characteristic)
        {
            List<Person> list = new List<Person>();
            switch (characteristic)
            {
                case ItemProperty.No:
                    for (int i = 0; i < Count; i++)
                        list.Add(new Person());
                    break;
                case ItemProperty.ID:
                    for (int i = 0; i < Count; i++)
                        list.Add(new Person() { Id = Guid.NewGuid().ToString() });
                    break;
                case ItemProperty.IsFavorite:
                    for (int i = 0; i < Count; i++)
                        list.Add(new Person() { IsFavorite = true });
                    break;
            }
            return list;
        }
    }
}
