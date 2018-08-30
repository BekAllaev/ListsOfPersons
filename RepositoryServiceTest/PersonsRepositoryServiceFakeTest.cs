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
        PersonsRepositoryServiceFake repository = new PersonsRepositoryServiceFake(); // Used for evoke method that under testing.

        const int Count = 1000; //Count of items in each collection of this class

        /// <summary>
        /// Test business logic - that not remove last item of collection.
        /// </summary>
        [TestMethod]
        public void NotRemoveIfOneRemainPerson()
        {
            List<Person> list = new List<Person>()
            {
                new Person(){ Id=Guid.NewGuid().ToString() }
            };
            repository.TestList = new List<Person>(list);

            string personId = list[0].Id;

            repository.DeleteAsync(personId, 1).Wait();

            Assert.AreNotEqual(0, repository.TestList.Count);
        }

        [DataTestMethod]
        [TestCategory("Param method")]
        [DataRow(Count)]
        public void GetAllAsyncTest(int count)
        {
            List<Person> list = GetList(ItemProperty.No,count);

            repository.TestList = new List<Person>(list);

            List<Person> resultList = repository.GetAllAsync().Result;

            //TODO: Talk about what we compare
            Assert.AreEqual(count, resultList.Count);
        }

        [DataTestMethod]
        public void GetAllFavoriteTest()
        {
            List<Person> list = GetList(ItemProperty.IsFavorite,Count);

            repository.TestList = new List<Person>(list);
            List<Person> resultList = repository.GetAllFavoriteAsync().Result;

            Assert.AreEqual(resultList.Count, list.Count - 1);
        }

        [TestMethod]
        public void GetByIdTest()
        {
            List<Person> list = GetList(ItemProperty.ID,Count);
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
        /// Property that will use for comparing,searching etc 
        /// </param>
        /// <param name="itemsCount">
        /// Count of items in list
        /// </param>
        private List<Person> GetList(ItemProperty characteristic, int itemsCount)
        {
            List<Person> list = new List<Person>();

            switch (characteristic)
            {
                case ItemProperty.No:
                    for (int i = 0; i < itemsCount; i++)
                        list.Add(new Person());
                    return list;
                case ItemProperty.ID:
                    for (int i = 0; i < itemsCount; i++)
                        list.Add(new Person() { Id = Guid.NewGuid().ToString() });
                    return list;
                case ItemProperty.IsFavorite:
                    list.Add(new Person() { IsFavorite = false });
                    for (int i = 0; i < itemsCount; i++)
                        list.Add(new Person() { IsFavorite = true });
                    return list;
                default:
                    throw new Exception("Check parameter");
            }
        }
    }
}
