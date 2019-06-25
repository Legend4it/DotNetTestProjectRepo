using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using System.Web.Caching;

namespace UsingWebCachingInWebFormApp
{
    class Program
    {
        static void Main(string[] args)
        {
            const string key = "CustomerData";
            var dataObject = Serializer.SerializeObjectToJson(MokeItems.GetCustomers());

            CacheItemRemovedCallback onRemoveCallback = new CacheItemRemovedCallback(OnRemoveCallback);
            HttpRuntime.Cache.Insert(key, dataObject,null,DateTime.Now.AddSeconds(20),TimeSpan.Zero,CacheItemPriority.High,onRemoveCallback);

            if (HttpRuntime.Cache[key] != null)
            {
                Console.WriteLine(HttpRuntime.Cache[key] as string);
            }

            List<CachedCustomer> fromCacheObject = Serializer.DeserializedJsonToObject(HttpRuntime.Cache[key] as string);
            Thread.Sleep(10000);

            Console.WriteLine(HttpRuntime.Cache[key] as string);

            if (HttpRuntime.Cache[key] == null)
            {
                Console.WriteLine("New Cache ready to new item");
            }

            Console.ReadKey();
        }

        private static void OnRemoveCallback(string key, object value, CacheItemRemovedReason reason)
        {
            //HttpRuntime.Cache.Remove(key);
            Console.WriteLine(reason);
        }
    }
}

public class CachedCustomer
{
    public string custoemrNo { get; set; }
    public string custoemrCompnayCode { get; set; }
    public List<CachedItem> Items { get; set; }
}
public class CachedItem
{
    public string ItemNr { get; set; }
    public decimal TransferPrice { get; set; }
    public decimal SalesPrice { get; set; }
    public string TransferCurrency { get; set; }
    public string SalesCurrency { get; set; }
    public decimal Quantity { get; set; }
    public string Message { get; set; }
}

public static class MokeItems
{
    public static List<CachedCustomer> GetCustomers()
    {
        var itemsObject = new List<CachedItem>
        {
            new CachedItem()
            {
                ItemNr = "01",
                Message = "Hello",
                Quantity = 1,
                SalesCurrency = "EURO",
                SalesPrice = 2
            },
            new CachedItem()
            {
                ItemNr = "02",
                Message = "Hello",
                Quantity = 1,
                SalesCurrency = "EURO",
                SalesPrice = 2
            },
            new CachedItem()
            {
                ItemNr = "03",
                Message = "Hello",
                Quantity = 1,
                SalesCurrency = "EURO",
                SalesPrice = 2
            }
        };
        var customerObject=new CachedCustomer(){custoemrNo = "ABC-001", custoemrCompnayCode = "002", Items = itemsObject};
        return new List < CachedCustomer > { customerObject};
    }
}

public static class Serializer
{
    public static string SerializeObjectToJson(object objectToSerialize)
    {
        return JsonConvert.SerializeObject(objectToSerialize, Formatting.Indented);
    }

    public static List<CachedCustomer> DeserializedJsonToObject(string jsonStringToDeserialized)
    {
        return JsonConvert.DeserializeObject<List<CachedCustomer>>(jsonStringToDeserialized);
    }
}