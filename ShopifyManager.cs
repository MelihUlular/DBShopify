using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBShopify
{
    internal class ShopifyManager
    {
        internal static IEnumerable<EBayCategoryList> GetEBayCategoryList()
        {
            var context = new ShoeSectorDevelopmentEntities();
            var productlist = context.EBayCategoryList
                                              .ToList();
            return productlist;
        }

        internal static List<ItemInventory> GetItemInventoryByItemGroupCode2All(string groupcode)
        {
            ////where entity.Name.Contains("xyz")
            //string code=null;
            //string[] codes = groupcode.Split('|');
            //if (codes.Length >= 3)
            //{
            //    code = codes[0] + "|" + codes[1] + "|" + codes[2];
            //}

            var context = new ShoeSectorDevelopmentEntities();
            var productItemlist = context.ItemInventory
                                              .Where(s => s.ItemGroupCode2.Contains(groupcode)).OrderBy(x=>x.SizeNumeric)
                                              .ToList();
            return productItemlist;
        }

        internal static List<ItemImageLibrary> GetItemInventoryImageListByItemGroupCode2(string groupcode)
        {
            var context = new ShoeSectorDevelopmentEntities();
            var productImagelist = context.ItemImageLibrary
                                              .Where(s => s.ItemGroupCode2 == groupcode)
                                              .ToList();
            return productImagelist;
        }

        internal static List<ItemGroupCode2Type> GetItemListtoUpload()
        {
            using (ShoeSectorDevelopmentEntities db = new ShoeSectorDevelopmentEntities())
            {
                var itmList = (from i in db.ItemInventory
                               join w in db.WebSiteMasterGroupItem on i.MasterGroupCode equals w.MasterGroupCode into w_join
                               from w in w_join.DefaultIfEmpty()
                               where
                                   i.MasterColorName != null &&
                                   i.QuantityOnHand != 0 &&
                                   i.AmazonPriceMarkupPercentage != 0 &&
                                   w.MasterGroupCode == null
                               group i by new { i.ItemGroupCode2 } into g
                               select new ItemGroupCode2Type() { ItemGroupCode2 = g.Key.ItemGroupCode2 }
    ).ToList();
                if (itmList == null)
                    return null;

                return itmList;
            };
         

        }

        internal static ItemMarketing GetItemMarketing(string groupcode)
        {
            var context = new ShoeSectorDevelopmentEntities();
            var Marketinglist = context.ItemMarketing
                                              .Where(s => s.ItemGroupCode2 == groupcode)
                                              .ToList().FirstOrDefault();
            return Marketinglist;

        }

        internal static bool UpdateMasterGroupCode(ItemInventory itm, string masterGroupID, string websiteID)
        {
            using (var context = new ShoeSectorDevelopmentEntities())
            {
                // Retrieve the entity to update based on the provided masterGroupID
                var websiteMasterGroup = context.WebSiteItem.SingleOrDefault(group => group.MasterGroupCode == masterGroupID);

                if (websiteMasterGroup != null)
                {
                    // Modify the properties of the entity
                    websiteMasterGroup.WebSiteId = itm.NumericId;
                    websiteMasterGroup.CreatedOn = DateTime.Now;
                    websiteMasterGroup.UpcNumber = itm.UPCCode;
                    websiteMasterGroup.IsCompleted = true;

                    // Indicate that the entity has been modified
                    context.Entry(websiteMasterGroup).State = EntityState.Modified;
                }
                else
                {
                    // Entity doesn't exist, so create and add a new one
                    websiteMasterGroup = new WebSiteItem
                    {
                        MasterGroupCode = masterGroupID,
                        WebSiteId = Convert.ToInt64(websiteID),
                        CreatedOn = DateTime.Now,
                        UpcNumber = itm.UPCCode,
                        IsCompleted = true,
                    Id = itm.NumericId
                    };
                    context.WebSiteItem.Add(websiteMasterGroup);
                }

                // Save changes to the database
                try
                {
                    // Your existing code for updating or creating the entity

                    context.SaveChanges();
                    return true; // Indicate successful update or creation
                }
                catch (DbEntityValidationException ex)
                {
                    // Handle validation errors
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Console.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                        }
                    }
                    return false; // Indicate validation error
                }


               // return true; // Indicate successful update or creation
            }

        }

        internal static bool UpdateWebSiteMasterGroup(string masterGroupID, int itemID, bool isPublished, string publishNote, string productId_)
        {
            using (var context = new ShoeSectorDevelopmentEntities())
            {
                // Retrieve the entity to update based on the provided masterGroupID
                var websiteMasterGroup = context.WebSiteMasterGroupItem.SingleOrDefault(group => group.MasterGroupCode == masterGroupID);

                if (websiteMasterGroup != null)
                {
                    // Modify the properties of the entity
                    websiteMasterGroup.ItemId = itemID;
                    websiteMasterGroup.IsPublished = isPublished;
                    websiteMasterGroup.ProductID = productId_;
                    websiteMasterGroup.CreatedOn = DateTime.Now;
                    websiteMasterGroup.PublishNote = publishNote;

                    // Indicate that the entity has been modified
                    context.Entry(websiteMasterGroup).State = EntityState.Modified;
                }
                else
                {
                    
                    websiteMasterGroup = new WebSiteMasterGroupItem
                    {
                        MasterGroupCode = masterGroupID,
                        ItemId = itemID,
                        IsPublished = isPublished,
                        ProductID = productId_,
                        CreatedOn = DateTime.Now,
                        PublishNote = publishNote
                };
                    context.WebSiteMasterGroupItem.Add(websiteMasterGroup);
                }

                
                try
                {
                    

                    context.SaveChanges();
                    return true; // Indicate successful update or creation
                }
                catch (DbEntityValidationException ex)
                {
                    // Handle validation errors
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Console.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                        }
                    }
                    return false; // Indicate validation error
                }


             //   return true; // Indicate successful update or creation
            }

        }

    }

    internal class ItemGroupCode2Type
    {
        public string ItemGroupCode2 { get; set; }

        public string BrandName { get; set; }
        public string ItemName { get; set; }
    }
}

public static class MyExtensions
{
    static TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
    public static string ToTitleCase(this string str)
    {
        if (str == null) return str;
        string res = textInfo.ToTitleCase(textInfo.ToLower(str));
        return res;
    }
}
