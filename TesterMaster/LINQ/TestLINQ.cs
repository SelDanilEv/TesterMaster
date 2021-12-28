using System;
using System.Collections.Generic;
using System.Linq;
using TestMasterInfrastructure.Interfaces;

namespace TesterMaster.LINQ
{
    public class TestLINQ : ITest
    {
        private class Item
        {
            public Item(float originalPrice, float discountedPrice)
            {
                OriginalPrice = originalPrice;
                DiscountedPrice = discountedPrice;
            }

            public float OriginalPrice { get; set; }
            public float DiscountedPrice { get; set; }
        }

        private readonly List<Item> Items = new List<Item>()
        {
            new Item (10,7),
            new Item (12,10),
            new Item (11,11)
        };

        public void StartTest()
        {
            TestSelectAndSumFloat();
        }

        private void TestSelectAndSumFloat()
        {
            Console.WriteLine($"Start case 1: test select with sum of float values");

            try
            {
                var totalDiscount = Items.Select(item => item.OriginalPrice - item.DiscountedPrice).Sum();
                Console.WriteLine($"Total discount: {totalDiscount}");
            }
            catch { }

            Console.WriteLine($"End case 1.");
        }
    }
}
