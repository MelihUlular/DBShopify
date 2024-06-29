
using System.Configuration;
using System.Diagnostics;
using System.Net;
using DevExpress.Xpo.Metadata;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using System;
using System.Linq;
using System.Globalization;
using DBShopify;
using DevExpress.Data.Filtering;
using System.Collections.Generic;

namespace ShopifyAddItems
{
    class ShopifyAdd
    {
        public static string ChilkatKey ="Start my 30-day Trial";
        public static string ShopifyAPIKey ="";
        public static string ShopifyAPISecretKey ="";
        
        public static void Run(List<ItemGroupCode2Type> list)
        {
            unlockChilkat();
            init();
            AddItem(list);
        }


        private static void unlockChilkat()
        {
            Chilkat.Global glob = new Chilkat.Global();
            bool success1 = glob.UnlockBundle(ChilkatKey);
            if (success1 != true)
            {
                Console.WriteLine(glob.LastErrorText);
                return;
            }
        }
        private static Chilkat.Rest shopifyAccess()
        {
            bool success;
            Chilkat.Rest rest = new Chilkat.Rest();

            rest.SetAuthBasic(ShopifyAPIKey, ShopifyAPISecretKey);
            Chilkat.StringBuilder sbJson = new Chilkat.StringBuilder();
            success = rest.Connect("xxxxx.myshopify.com", 443, true, true);
            if (success != true)
            {
                Console.WriteLine(rest.LastErrorText);
                return null;
            }
            return rest;
        }


        private static void AddItem(List<ItemGroupCode2Type> itemList)
        {
            
            string product_tags = string.Empty;

            bool success;
            Chilkat.StringBuilder sbJson = new Chilkat.StringBuilder();
            Chilkat.Rest rest = shopifyAccess();
            if (rest == null)
            {
                Console.WriteLine("Connection error ");
                return;
            }

            foreach (var invItm in itemList)
            {
                int counter = 0;


                Chilkat.JsonObject jsonReq = new Chilkat.JsonObject();




                var item = InventoryManager.GetItemInventoryByItemGroupCode2All(invItm.ItemGroupCode2);


                if (item.FirstOrDefault() == null)
                    continue;

                Console.WriteLine("Submitting to website {0} ", item.FirstOrDefault().ItemGroupCode2);

                var itemMarketing = InventoryManager.GetItemMarketing(item.First().ItemGroupCode2);
                if (itemMarketing == null)
                {
                    Console.WriteLine("No marketing data", item.First().ItemGroupCode2);
                    continue;
                }
                var itemImages = InventoryManager.GetItemInventoryImageListByItemGroupCode2(item.First().ItemGroupCode2).ToList();
                if (itemImages.Count == 0)
                {
                    Console.WriteLine("No image data", item.First().ItemGroupCode2);
                    continue;
                }
                string itemCat = string.Empty;
                var ebayItemCat = EBayDataManager.GetEBayCategoryList().Where(x => x.Id == Convert.ToInt16(itemMarketing.CategoryNumber1)).FirstOrDefault();
                if (ebayItemCat == null)
                {
                    itemCat = null;
                }
                else
                    itemCat = ebayItemCat.CategoryName;

                string _titleGender = string.Empty;

                var textinfo = new CultureInfo("en-US", false).TextInfo;

                var code2Arr = item.First().ItemGroupCode2.Split('|');
                switch (code2Arr[2])
                {
                    case ("M"):
                        _titleGender = "Men";
                        break;
                    case ("W"):
                        _titleGender = "Women";
                        break;
                    case ("B"):
                        _titleGender = "Boy";
                        break;
                    case ("G"):
                        _titleGender = "Girl";
                        break;
                    case ("U"):
                        _titleGender = "Unisex";
                        break;
                    case ("F"):
                        _titleGender = "Women";
                        break;
                    case ("Y"):
                        _titleGender = "Youth";
                        break;
                    case ("J"):
                        _titleGender = "Junior";
                        break;
                    case ("I"):
                        _titleGender = "Infant";
                        break;
                    case ("T"):
                        _titleGender = "Toddler";
                        break;
                    case ("K"):
                        _titleGender = "Kids";
                        break;

                    default:
                        break;
                }
                try
                {
                    string _width = string.Empty;

                    switch (item.First().Width)
                    {
                        case "XN":
                            _width = "Extra Narrow Width";
                            break;
                        case "N":
                            _width = "Narrow Width";
                            break;
                        case "M":
                            _width = string.Empty;
                            break;
                        case "W":
                            _width = "Wide Width";
                            break;
                        case "XW":
                            _width = "Extra Wide Width";
                            break;
                        case "MM":
                            _width = string.Empty;
                            break;
                        case "S":
                            _width = "Narrow Width";
                            break;

                        default:
                            break;
                    }
                    product_tags = string.Format("{0}, {1}, {2}, {3} ,{4}, {5}, {6}", textinfo.ToTitleCase(invItm.BrandName), _titleGender, textinfo.ToTitleCase(invItm.ItemName), textinfo.ToTitleCase(item.First().Color),
                                                        textinfo.ToTitleCase(item.First().MasterColorName), ebayItemCat.CategoryName, itemCat + _width);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("null error exception");
                    continue;
                }



                string sizeList = string.Empty;
                string itemTitle = string.Empty;


                string pType = string.Empty;

                if (itemMarketing.EbayStyle1 == null)
                    pType = itemCat;
                else
                    pType = itemCat;



                
                itemTitle = string.Format("{0} {1} {2}", item.First().BrandName, _titleGender, item.First().ItemName);

                jsonReq.UpdateString("product.title", textinfo.ToTitleCase(itemTitle));
                jsonReq.UpdateString("product.body_html", itemMarketing.EBayDescription);
                jsonReq.UpdateString("product.vendor", textinfo.ToTitleCase(item.First().BrandName));
                jsonReq.UpdateString("product.product_type", pType);
                jsonReq.UpdateString("product.collections", pType);
                jsonReq.UpdateString("product.options[0].name", "Color");
                jsonReq.UpdateString("product.options[1].name", "Size");
                //jsonReq.UpdateString("product.options[1].values[0]", item.First().Color.ToTitleCase());
                foreach (var itm in item)
                {
                    switch (itm.Width)
                    {
                        case "XN":
                        case "N":
                        case "M":
                        case "W":
                        case "XW":
                        case "MM":
                        case "S":
                            jsonReq.UpdateString("product.options[2].name", "Width");
                            jsonReq.UpdateString(string.Format("product.variants[{0}].option1", counter), textinfo.ToTitleCase(item.First().Color));
                            jsonReq.UpdateString(string.Format("product.variants[{0}].option2", counter), itm.Size);
                            jsonReq.UpdateString(string.Format("product.variants[{0}].option3", counter), itm.Width);
                            jsonReq.UpdateString(string.Format("product.variants[{0}].compare_at_price", counter), itm.MaxSalesPrice.ToString());
                            jsonReq.UpdateString(string.Format("product.variants[{0}].price", counter), itm.MinSalesPrice.ToString());
                            jsonReq.UpdateString(string.Format("product.variants[{0}].sku", counter), itm.ItemCodeFull);
                            jsonReq.UpdateString(string.Format("product.variants[{0}].barcode", counter), itm.UPCCode);
                            jsonReq.UpdateString(string.Format("product.variants[{0}].inventory_quantity", counter), itm.QuantityOnHand.ToString());
                            jsonReq.UpdateString(string.Format("product.variants[{0}].fulfillment_service", counter), "manual");
                            jsonReq.UpdateString(string.Format("product.variants[{0}].inventory_management", counter), "shopify");
                            jsonReq.UpdateString(string.Format("product.variants[{0}].inventory_policy", counter), "deny");
                            jsonReq.UpdateString(string.Format("product.variants[{0}].currency_code", counter), "USD");
                            jsonReq.UpdateString(string.Format("product.variants[{0}].requires_shipping", counter), "true");
                            sizeList = sizeList + "," + itm.Size;
                            counter++;
                            break;

                        default:
                            jsonReq.UpdateString(string.Format("product.variants[{0}].option2", counter), itm.Size);
                            jsonReq.UpdateString(string.Format("product.variants[{0}].option1", counter), textinfo.ToTitleCase(item.First().Color));
                            jsonReq.UpdateString(string.Format("product.variants[{0}].compare_at_price", counter), itm.MaxSalesPrice.ToString());
                            jsonReq.UpdateString(string.Format("product.variants[{0}].price", counter), itm.MaxSalesPrice.ToString());
                            jsonReq.UpdateString(string.Format("product.variants[{0}].sku", counter), itm.ItemCodeFull);
                            jsonReq.UpdateString(string.Format("product.variants[{0}].barcode", counter), itm.UPCCode);
                            jsonReq.UpdateString(string.Format("product.variants[{0}].inventory_quantity", counter), itm.QuantityOnHand.ToString());
                            jsonReq.UpdateString(string.Format("product.variants[{0}].fulfillment_service", counter), "manual");
                            jsonReq.UpdateString(string.Format("product.variants[{0}].inventory_management", counter), "shopify");
                            jsonReq.UpdateString(string.Format("product.variants[{0}].inventory_policy", counter), "deny");
                            jsonReq.UpdateString(string.Format("product.variants[{0}].currency_code", counter), "USD");
                            jsonReq.UpdateString(string.Format("product.variants[{0}].requires_shipping", counter), "true");
                            sizeList = sizeList + "," + itm.Size;
                            counter++;
                            break;
                    }

                }
                product_tags = product_tags + " " + sizeList;
                jsonReq.UpdateString("product.tags", product_tags);
                counter = 0;

                foreach (var pic in itemImages)
                {
                    jsonReq.UpdateString(string.Format("product.images[{0}].src", counter), "http://image.shoesector.com/i/" + pic.ImageName);
                    counter++;

                }

                Chilkat.StringBuilder sbReq = new Chilkat.StringBuilder();
                jsonReq.EmitSb(sbReq);

                rest.AddHeader("Content-Type", "application/json");

                sbJson = new Chilkat.StringBuilder();
                success = rest.FullRequestSb("POST", "/admin/products.json ", sbReq, sbJson);
                if (success != true)
                {

                    Console.WriteLine(rest.LastErrorText);
                    //Helper.SetTimer("0.00:10:00.0000", "Standing by");
                    rest = shopifyAccess();
                    continue;
                }

                if (rest.ResponseStatusCode != 201)
                {
                    Console.WriteLine("Received error response code: " + Convert.ToString(rest.ResponseStatusCode));
                    Console.WriteLine("Response body:");
                    Console.WriteLine(sbJson.GetAsString());

                   // ShopifyManager.UpdateWebSiteMasterGroup(item.FirstOrDefault().MasterGroupCode, 0, true, "ERROR", null);
                    continue;

                }


                if (success != true)
                {
                    Debug.WriteLine(rest.LastErrorText);
                    return;
                }

                string productId_;
                Chilkat.JsonObject json_ = new Chilkat.JsonObject();
                json_.LoadSb(sbJson);

                productId_ = json_.StringOf("product.id");


                string masterGroupID = Guid.NewGuid().ToString().Replace("-", string.Empty);
                //add product id to table
             //   var returnID = ShopifyManager.UpdateWebSiteMasterGroup(masterGroupID, 1, true, "DONE", productId_);

                //update inventory mastergroupcode

                //foreach (var itm in item)
                //{
                //    ShopifyManager.UpdateMasterGroupCode(itm, masterGroupID);
                //}
                Console.WriteLine("Sucsess... MasterGroupID : {0}, ProductID: {1}", masterGroupID, productId_);

            }




            // The following code creates the JSON request body.
            // The JSON created by this code is shown below.

            // The JSON request body created by the above code:

            // {
            //   "product": {
            //     "title": "Burton Custom Freestyle 151",
            //     "body_html": "<strong>Good snowboard!<\/strong>",
            //     "vendor": "Burton",
            //     "product_type": "Snowboard",
            //     "variants": [
            //       {
            //         "option1": "First",
            //         "price": "10.00",
            //         "sku": "123"
            //       },
            //       {
            //         "option1": "Second",
            //         "price": "20.00",
            //         "sku": "123"
            //       }
            //     ]
            //   }
            // }

            Chilkat.JsonObject json = new Chilkat.JsonObject();
            // json.LoadSb(sbJson);
/*
            // The following code parses the JSON response.
            // A sample JSON response is shown below the sample code.
            int productId;
            string productTitle;
            string productBody_html;
            string productVendor;
            string productProduct_type;
            string productCreated_at;
            string productHandle;
            string productUpdated_at;
            string productPublished_at;
            bool productTemplate_suffix;
            string productPublished_scope;
            string productTags;
            bool productImage;
            int i;
            int count_i;
            int id;
            int product_id;
            string title;
            string price;
            string sku;
            int position;
            int grams;
            string inventory_policy;
            bool compare_at_price;
            string fulfillment_service;
            bool inventory_management;
            string option1;
            bool option2;
            bool option3;
            string created_at;
            string updated_at;
            bool taxable;
            bool barcode;
            bool image_id;
            int inventory_quantity;
            int weight;
            string weight_unit;
            int old_inventory_quantity;
            bool requires_shipping;
            string name;
            int j;
            int count_j;
            string strVal;

            productId = json.IntOf("product.id");
            productTitle = json.StringOf("product.title");
            productBody_html = json.StringOf("product.body_html");
            productVendor = json.StringOf("product.vendor");
            productProduct_type = json.StringOf("product.product_type");
            productCreated_at = json.StringOf("product.created_at");
            productHandle = json.StringOf("product.handle");
            productUpdated_at = json.StringOf("product.updated_at");
            productPublished_at = json.StringOf("product.published_at");
            productTemplate_suffix = json.IsNullOf("product.template_suffix");
            productPublished_scope = json.StringOf("product.published_scope");
            productTags = json.StringOf("product.tags");
            productImage = json.IsNullOf("product.image");
            i = 0;
            count_i = json.SizeOfArray("product.variants");
            while (i < count_i)
            {
                json.I = i;
                id = json.IntOf("product.variants[i].id");
                product_id = json.IntOf("product.variants[i].product_id");
                title = json.StringOf("product.variants[i].title");
                price = json.StringOf("product.variants[i].price");
                sku = json.StringOf("product.variants[i].sku");
                position = json.IntOf("product.variants[i].position");
                grams = json.IntOf("product.variants[i].grams");
                inventory_policy = json.StringOf("product.variants[i].inventory_policy");
                compare_at_price = json.IsNullOf("product.variants[i].compare_at_price");
                fulfillment_service = json.StringOf("product.variants[i].fulfillment_service");
                inventory_management = json.IsNullOf("product.variants[i].inventory_management");
                option1 = json.StringOf("product.variants[i].option1");
                option2 = json.IsNullOf("product.variants[i].option2");
                option3 = json.IsNullOf("product.variants[i].option3");
                created_at = json.StringOf("product.variants[i].created_at");
                updated_at = json.StringOf("product.variants[i].updated_at");
                taxable = json.BoolOf("product.variants[i].taxable");
                barcode = json.IsNullOf("product.variants[i].barcode");
                image_id = json.IsNullOf("product.variants[i].image_id");
                inventory_quantity = json.IntOf("product.variants[i].inventory_quantity");
                weight = json.IntOf("product.variants[i].weight");
                weight_unit = json.StringOf("product.variants[i].weight_unit");
                old_inventory_quantity = json.IntOf("product.variants[i].old_inventory_quantity");
                requires_shipping = json.BoolOf("product.variants[i].requires_shipping");
                i = i + 1;
            }

            i = 0;
            count_i = json.SizeOfArray("product.options");
            while (i < count_i)
            {
                json.I = i;
                id = json.IntOf("product.options[i].id");
                product_id = json.IntOf("product.options[i].product_id");
                name = json.StringOf("product.options[i].name");
                position = json.IntOf("product.options[i].position");
                j = 0;
                count_j = json.SizeOfArray("product.options[i].values");
                while (j < count_j)
                {
                    json.J = j;
                    strVal = json.StringOf("product.options[i].values[j]");
                    j = j + 1;
                }

                i = i + 1;
            }

            i = 0;
            count_i = json.SizeOfArray("product.images");
            while (i < count_i)
            {
                json.I = i;
                i = i + 1;
            }

            // A sample JSON response body that is parsed by the above code:

            // {
            //   "product": {
            //     "id": 1071559755,
            //     "title": "Burton Custom Freestyle 151",
            //     "body_html": "<strong>Good snowboard!<\/strong>",
            //     "vendor": "Burton",
            //     "product_type": "Snowboard",
            //     "created_at": "2017-09-22T14:48:54-04:00",
            //     "handle": "burton-custom-freestyle-151",
            //     "updated_at": "2017-09-22T14:48:55-04:00",
            //     "published_at": "2017-09-22T14:48:54-04:00",
            //     "template_suffix": null,
            //     "published_scope": "global",
            //     "tags": "",
            //     "variants": [
            //       {
            //         "id": 1070325225,
            //         "product_id": 1071559755,
            //         "title": "First",
            //         "price": "10.00",
            //         "sku": "123",
            //         "position": 1,
            //         "grams": 0,
            //         "inventory_policy": "deny",
            //         "compare_at_price": null,
            //         "fulfillment_service": "manual",
            //         "inventory_management": null,
            //         "option1": "First",
            //         "option2": null,
            //         "option3": null,
            //         "created_at": "2017-09-22T14:48:54-04:00",
            //         "updated_at": "2017-09-22T14:48:54-04:00",
            //         "taxable": true,
            //         "barcode": null,
            //         "image_id": null,
            //         "inventory_quantity": 1,
            //         "weight": 0.0,
            //         "weight_unit": "lb",
            //         "old_inventory_quantity": 1,
            //         "requires_shipping": true
            //       },
            //       {
            //         "id": 1070325226,
            //         "product_id": 1071559755,
            //         "title": "Second",
            //         "price": "20.00",
            //         "sku": "123",
            //         "position": 2,
            //         "grams": 0,
            //         "inventory_policy": "deny",
            //         "compare_at_price": null,
            //         "fulfillment_service": "manual",
            //         "inventory_management": null,
            //         "option1": "Second",
            //         "option2": null,
            //         "option3": null,
            //         "created_at": "2017-09-22T14:48:54-04:00",
            //         "updated_at": "2017-09-22T14:48:54-04:00",
            //         "taxable": true,
            //         "barcode": null,
            //         "image_id": null,
            //         "inventory_quantity": 1,
            //         "weight": 0.0,
            //         "weight_unit": "lb",
            //         "old_inventory_quantity": 1,
            //         "requires_shipping": true
            //       }
            //     ],
            //     "options": [
            //       {
            //         "id": 1022828915,
            //         "product_id": 1071559755,
            //         "name": "Title",
            //         "position": 1,
            //         "values": [
            //           "First",
            //           "Second"
            //         ]
            //       }
            //     ],
            //     "images": [
            //     ],
            //     "image": null
            //   }
            // }
*/
            Debug.WriteLine("Example Completed.");
        }


        private static void init()
        {
            string connectionString ="xxxxx.myshopify.com";
            var dict = new ReflectionDictionary();
            var store = XpoDefault.GetConnectionProvider(connectionString, AutoCreateOption.SchemaAlreadyExists);

            dict.GetDataStoreSchema(
                typeof(ItemInventory),
                typeof(ItemMarketing),
                typeof(ItemImageLibrary),
                typeof(EBayCategoryList),
                typeof(EbayCategoryStyleList),
                typeof(WebSiteItem),
                typeof(WebSiteMasterGroupItem)
                );


            XpoDefault.DataLayer = new ThreadSafeDataLayer(dict, store);
            XpoDefault.Session = null;
        }


    }
}
