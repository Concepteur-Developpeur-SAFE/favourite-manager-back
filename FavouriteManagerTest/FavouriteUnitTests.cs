using FavouriteManager.Persistence.entity;
using System.ComponentModel;

namespace FavouriteManagerTest
{
    [TestClass]
    public class FavouriteUnitTests
    {
        static Category category1 = new Category(1, "Cat1");

        static DateTime lastUpdated = DateTime.Now;

        Favourite favourite1 = new Favourite(1, "Link1", "Label1", true, category1, lastUpdated);

        [TestMethod]
        public void TestCategory()
        {
            Assert.AreEqual(category1.Id, 1);
            Assert.AreEqual(category1.Label,  "Cat1");

            Assert.AreEqual(favourite1.Id, 1);
            Assert.AreEqual(favourite1.Link, "Link1");
            Assert.AreEqual(favourite1.Label, "Label1");
            Assert.AreEqual(favourite1.IsValid, true);
            Assert.AreEqual(favourite1.Category, category1);
            Assert.AreEqual(favourite1.UpdatedAt, lastUpdated);
        }
    }
}