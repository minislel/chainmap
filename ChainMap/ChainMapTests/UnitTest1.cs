using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ChainMapLib;

namespace ChainMapLib.Tests
{
    [TestClass]
    public class ChainMapTests
    {
        [TestMethod]
        public void Indexer_GetValueFromMainDictionary()
        {
            var dict = new Dictionary<int, string>() { {1, "One" } };    
            var chainMap = new ChainMap<int, string>(dict);
            chainMap.Add(1, "Two");

            // Act
            var result = chainMap[1];

            // Assert
            Assert.AreEqual("Two", result);
        }

        [TestMethod]
        public void Indexer_GetValueFromSecondaryDictionary()
        {
            // Arrange
            var mainDictionary = new Dictionary<int, string> { { 1, "One" } };
            var secondaryDictionary = new Dictionary<int, string> { { 2, "Two" } };
            var chainMap = new ChainMap<int, string>(mainDictionary);
            chainMap.AddDictionary(secondaryDictionary, 1);

            // Act
            var result = chainMap[2];

            // Assert
            Assert.AreEqual("Two", result);
        }



        [TestMethod]
        public void Indexer_SetValueInSecondaryDictionary()
        {
            // Arrange
            var mainDictionary = new Dictionary<int, string> { { 1, "One" } };
            var secondaryDictionary = new Dictionary<int, string> { { 2, "Two" } };
            var chainMap = new ChainMap<int, string>(mainDictionary);
            chainMap.AddDictionary(secondaryDictionary, 1);

            // Act
            chainMap[2] = "New Value";

            // Assert
            Assert.AreEqual("New Value", chainMap[2]);
        }

        [TestMethod]
        public void Keys_ReturnsAllKeys()
        {
            // Arrange
            var mainDictionary = new Dictionary<int, string> { { 1, "One" } };
            var secondaryDictionary = new Dictionary<int, string> { { 2, "Two" }, { 3, "Three" } };
            var chainMap = new ChainMap<int, string>(mainDictionary);
            chainMap.AddDictionary(secondaryDictionary, 1);

            // Act
            var keys = chainMap.Keys;

            // Assert
            CollectionAssert.AreEqual(new List<int> { 1, 2, 3 }, new List<int>(keys));
        }

        [TestMethod]
        public void Values_ReturnsAllValues()
        {
            // Arrange
            var mainDictionary = new Dictionary<int, string> { { 1, "One" } };
            var secondaryDictionary = new Dictionary<int, string> { { 2, "Two" }, { 3, "Three" } };
            var chainMap = new ChainMap<int, string>(mainDictionary);
            chainMap.AddDictionary(secondaryDictionary, 1);

            // Act
            var values = chainMap.Values;

            // Assert
            CollectionAssert.AreEqual(new List<string> { "One", "Two", "Three" }, new List<string>(values));
        }

        [TestMethod]
        public void Count_ReturnsCorrectCount()
        {
            // Arrange
            var mainDictionary = new Dictionary<int, string> { { 1, "One" } };
            var secondaryDictionary = new Dictionary<int, string> { { 2, "Two" }, { 3, "Three" } };
            var chainMap = new ChainMap<int, string>(mainDictionary);
            chainMap.AddDictionary(secondaryDictionary, 1);

            // Act
            var count = chainMap.Count;

            // Assert
            Assert.AreEqual(3, count);
        }

        [TestMethod]
        public void Add_ThrowsException()
        {
            // Arrange
            var chainMap = new ChainMap<int, string>();

            // Act
            chainMap.Add(1, "One");

            // Assert
            Assert.ThrowsException<KeyNotFoundException>(()=>chainMap[1]);
        }
        [TestMethod]
        public void Add_AddsValueToMainDictionary()
        {
            var dict1 = new Dictionary<int, string>();
            dict1.Add(1, "one");
            var chainMap = new ChainMap<int, string>(dict1);
            chainMap.Add(1, "two");
            Assert.AreEqual(chainMap[1], "two");

        }

        [TestMethod]
        public void AddDictionary_AddsDictionaryWithPriority()
        {
            // Arrange
            var mainDictionary = new Dictionary<int, string> { { 1, "One" } };
            var secondaryDictionary = new Dictionary<int, string> { { 2, "Two" } };
            var chainMap = new ChainMap<int, string>(mainDictionary);

            // Act
            chainMap.AddDictionary(secondaryDictionary, 1);

            // Assert
            Assert.AreEqual("Two", chainMap[2]);
        }

        [TestMethod]
        public void Remove_RemovesValueFromMainDictionary()
        {
            var dict = new Dictionary<int, string>() { {1,"One" } };
            var chainMap = new ChainMap<int, string>(dict);
            chainMap.Add(1, "Two");

            // Act
            var removed = chainMap.Remove(1);

            // Assert
            Assert.IsTrue(removed);
        }

        [TestMethod]
        public void Clear_ClearsAllDictionaries()
        {
            // Arrange
            var mainDictionary = new Dictionary<int, string> { { 1, "One" } };
            var secondaryDictionary = new Dictionary<int, string> { { 2, "Two" } };
            var chainMap = new ChainMap<int, string>(mainDictionary);
            chainMap.AddDictionary(secondaryDictionary, 1);

            // Act
            chainMap.ClearDictionaries();

            // Assert
            Assert.AreEqual(0, chainMap.CountDictionaries());
        }

        [TestMethod]
        public void ContainsKey_ReturnsTrueForExistingKey()
        {
            // Arrange
            var chainMap = new ChainMap<int, string>();
            chainMap.Add(1, "One");

            // Act
            var containsKey = chainMap.ContainsKey(1);

            // Assert
            Assert.IsFalse(containsKey);
        }

        [TestMethod]
        public void ContainsKey_ReturnsFalseForNonExistingKey()
        {
            // Arrange
            var chainMap = new ChainMap<int, string>();

            // Act
            var containsKey = chainMap.ContainsKey(1);

            // Assert
            Assert.IsFalse(containsKey);
        }

        [TestMethod]
        public void ContainsValue_ReturnsTrueForExistingValue()
        {
            var dict = new Dictionary<int, string>() { { 1, "One" } };
            var chainMap = new ChainMap<int, string>(dict);
            
            // Act
            var containsValue = chainMap.ContainsValue("One");

            // Assert
            Assert.IsTrue(containsValue);
        }

        [TestMethod]
        public void ContainsValue_ReturnsFalseForNonExistingValue()
        {
            // Arrange
            var chainMap = new ChainMap<int, string>();

            // Act
            var containsValue = chainMap.ContainsValue("One");

            // Assert
            Assert.IsFalse(containsValue);
        }

        [TestMethod]
        public void GetDictionaries_ReturnsAllDictionaries()
        {
            // Arrange
            var mainDictionary = new Dictionary<int, string> { { 1, "One" } };
            var secondaryDictionary = new Dictionary<int, string> { { 2, "Two" } };
            var chainMap = new ChainMap<int, string>(mainDictionary);
            chainMap.AddDictionary(secondaryDictionary, 1);

            // Act
            var dictionaries = chainMap.GetDictionaries();

            // Assert
            Assert.AreEqual(2, dictionaries.Count);
        }

        [TestMethod]
        public void GetDictionary_ReturnsSpecificDictionary()
        {
            // Arrange
            var mainDictionary = new Dictionary<int, string> { { 1, "One" } };
            var secondaryDictionary = new Dictionary<int, string> { { 2, "Two" } };
            var chainMap = new ChainMap<int, string>(mainDictionary);
            chainMap.AddDictionary(secondaryDictionary, 1);

            // Act
            var dictionary = chainMap.GetDictionary(1);

            // Assert
            CollectionAssert.AreEqual(secondaryDictionary, dictionary);
        }

        [TestMethod]
        public void GetMainDictionary_ReturnsMainDictionary()
        {
            // Arrange
            var dict = new Dictionary<int, string> { { 1, "One" } };
            var expectedDictionary = new Dictionary<int, string> { { 1, "Two" } };
            var chainMap = new ChainMap<int, string>(dict);
            chainMap[1] = "Two";
            // Act
            var dictionary = chainMap.GetMainDictionary();

            // Assert
            CollectionAssert.AreEqual(expectedDictionary, dictionary);
        }

        [TestMethod]
        public void Merge_MergesAllDictionaries()
        {
            // Arrange
            var mainDictionary = new Dictionary<int, string> { { 1, "One" }, { 2, "Two" } };
            var secondaryDictionary = new Dictionary<int, string> { { 2, "New Two" }, { 3, "Three" } };
            var chainMap = new ChainMap<int, string>(mainDictionary);
            chainMap.AddDictionary(secondaryDictionary, 1);

            // Act
            var mergedDictionary = chainMap.Merge();

            // Assert
            Assert.AreEqual(3, mergedDictionary.Count);
            Assert.AreEqual("Two", mergedDictionary[2]);
        }

        // Dodaj pozostałe testy dla pozostałych metod
    }
}
