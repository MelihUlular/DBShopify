//admin@shoesector.com

using Chilkat;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Net;
using System.Security.Policy;

namespace DBShopify
{
    internal class Program
    {
        public static string ChilkatKey = "Start my 30-day Trial";
        public static string ShopifyAPIKey = "";
        public static string ShopifyAPISecretKey = "";

        static void Main(string[] args)
        {

            unlockChilkat();
            AddItem();



            //var variantJson = GetAllVariants("8096080920834");

           

   
            //using (Image image = Image.FromFile(Path))
            //{
            //    using (MemoryStream m = new MemoryStream())
            //    {
            //        image.Save(m, image.RawFormat);
            //        byte[] imageBytes = m.ToArray();

            //        // Convert byte[] to Base64 String
            //        string base64String = Convert.ToBase64String(imageBytes);
            //        return base64String;
            //    }
            //



        }

        private static void updateVariants(string productId)
        {
            var variantJson = GetAllVariants(productId);

            Chilkat.StringBuilder sbResponseBody = new Chilkat.StringBuilder();

            sbResponseBody.SetString(variantJson);

            Chilkat.JsonObject json_ = new Chilkat.JsonObject();
            json_.LoadSb(sbResponseBody);

            JsonArray variants = json_.ArrayOf("variants");

            int count = variants.Size;

            List<string> ids = new List<string>();

            for (int i = 0; i < count; i++)
            {
                JsonObject variant = variants.ObjectAt(i);
                string variantid = variant.StringOf("id");
                ids.Add(variantid);
                UpdateMetafieldSize(variantid, sizesCache.First());

                sizesCache.RemoveAt(0);

            }

            //Process ids
            updateVariantImages(productId, ids);
        }

        private static void updateVariantImages(string productId, List<string> variantIds)
        {
            Chilkat.Rest rest = new Chilkat.Rest();
            bool success;

            // URL: https://your-development-store.myshopify.com/admin/api/2023-07/products/632910392/images.json
            bool bTls = true;
            int port = 443;
            bool bAutoReconnect = true;
            success = rest.Connect("xxxxxx.myshopify.com", port, bTls, bAutoReconnect);
            if (success != true)
            {
                Debug.WriteLine("ConnectFailReason: " + Convert.ToString(rest.ConnectFailReason));
                Debug.WriteLine(rest.LastErrorText);
                return;
            }

            // Note: The above code does not need to be repeatedly called for each REST request.
            // The rest object can be setup once, and then many requests can be sent.  Chilkat will automatically
            // reconnect within a FullRequest* method as needed.  It is only the very first connection that is explicitly
            // made via the Connect method.

            // Use this online tool to generate code from sample JSON: Generate Code to Create JSON

            // The following JSON is sent in the request body.

            // {
            //   "image": {
            //     "variant_ids": [
            //       808950810,
            //       457924702
            //     ],
            //     "attachment": "R0lGODlhbgCMAPf/APbr48VySrxTO7IgKt2qmKQdJeK8lsFjROG5p/nz7Zg3\nMNmnd7Q1MLNVS9GId71hSJMZIuzTu4UtKbeEeakhKMl8U8WYjfr18YQaIbAf\nKKwhKdKzqpQtLebFortOOejKrOjZ1Mt7aMNpVbAqLLV7bsNqR+3WwMqEWenN\nsZYxL/Ddy/Pm2e7ZxLlUQrIjNPXp3bU5MbhENbEtLtqhj5ZQTfHh0bMxL7Ip\nNsNyUYkZIrZJPcqGdYIUHb5aPKkeJnoUHd2yiJkiLKYiKLRFOsyJXKVDO8up\nosFaS+TBnK4kKti5sNaYg/z49aqYl5kqLrljUtORfMOlo/36+H4ZH8yDYq0f\nKKFYTaU9MrY8MrZBNXwXHpgaIdGVYu/byLZNP9SaZLIyOuXCtHkpJst+Wpcm\nLMyCa8BfP9GMb9KQdPDd1PPk1sd5VP79/L5dQZ0bI9+ymqssK9WcfIoXHdzG\nxdWWfteib79lSr1YP86MYurQxKdcUKdMQr5ZSfPs6YEZH8uhl4oWIenMuurQ\nttmejaqoqsqBVaAcJLlJN5kvMLlZRMNsSL5fRak0LbdQQMVvSPjw6cJnRpkf\nKtmjhvfu5cJtT7IuOMVvWLY/M/37+o0YH9ibhtSYdObErc6HarM9NnYSGNGR\navLi09unje3WyeO8rsVrT7tdRtK3uffu6NWeaL9pTJIjJrM4NPbx8cdyX7M7\nPYYVHu7j4KgoNJAYIKtkV5o9MsOcldicis+RYNutfrhFOZ0hJbqinZ8bI8h5\nUObFuOfItJsfJrJfUOfIqc+PXqQtK8RnSbA4Mcd3Tm0SGbpXQ8aqp7RLNs+s\novHfzpVhV9iggMd1TLtbRKUdKXEQFsd4XrZRPLIgMZUeJ+jKvrAlK6AhJ65A\nMpMpKuC3j5obIsRwS7hAN8l/YtvDvnYXHbAoLI47SIUsOMenorF4gO/m4+fH\npo4vLZ8oKMukqp0cJbhVSMV2UuPR0bAfMLIrLrg/OcJwT8h+Vt+wn8eurLlh\nQrIfKHQOHHQOHf///////yH5BAEAAP8ALAAAAABuAIwAAAj/AP8JHDhQXjpz\n/PopXNiPn0OHDRMmbKhQIsOJFS1SxAhxI8SHFzVeDBnx48iNBAeeOkcxokeX\nFRdOnAlSokaaLXNujJkxo8iYHRkKtWkzZSsaOXkAWsoUECynsHgoqEW1qtVa\nU7Mq2Mq1K9cUW8GKTUG2rNkUHNByWMuWLdWva7t1W7UKG4S7eO/ycEhQHgaK\nsL4VGGyocGE3br5929KuxQFFkEtIlgypsuUDmDMfWGRmUZvPoEHfGU36jgDT\nLQSoVt3IQ2sPsL0IUNZGlZ0H0lo00jEkCytWMspdGzBgn/F9EBIWnKIQlqHB\nhA0bQpx48Z7UAkoEcMTdUeTJJSxf/4akOTNnzqHb3GkjrUdp0gKwq77jWdod\nO7dNKWvhRUcWT6zYQI82xB03AAQNCdTKX/xAAB10hfVCnRtbVIhIAy14oJoZ\nAXS4XXfdQaYIeOGJRx555Z1nRnrqqUeaMtIYY8dmn7Vg2yK57TYEgAzIQGBx\nxyXHj0A0OOTggxFKSN1iWwTTAIYanpYdMtFE4+GVIHrn3XeUmVhZeWiIMoOY\nnVQDGiTgKALJjIssIsADt0mjjI6+AXcDgQYi2M8/7ijEwzRIFmBIL9NVV+EW\nVzyZ4Wqj9RBABchQWeWkV3aY5ZYjjgieeKL446mnjxwAiZVpliAjZqblt19/\n/7HCwIAFGv+X3J4s9fMckoYhphiTQTwJ5Wqn9dDDAWuMUUEFviTrS6STVlmp\npVmKqCkOn34aB6TIBAAOJeHZAYl6ptixSCL8edGbq8HFeqBDcygEyIOCGqYk\nkxUW4euiq7knbA/gUDHGv//ec2wFayQbaQWinOCslVhmSUq1/gCDLJXacgtJ\nCYu4J66cjbAKoA3CxapnOgm9g+ughdK7xYX3Rinlvj2YYcYanVBBhTg2Axzw\nG4/4k4bBzDZbKRUQP1LIsRSX6sgBZtwhzQP68ccbj7AWty4/5igEoaC9dK3r\noVtgs4evvzKqb8wyQ0JFJzXXbDMVcQBQLTDGVmCssstKGs09oPT/jQcRoBw9\nMamKgEOeeg/gqBtvdVZSDnHFIQgRD4RxXWhiYEOQKNn4zncHzDIzHc0ZpHdy\nRicIQOypKDf7q3Pd96ABzSab+E1EIYIvS2o0ijA92gPZiCB1qwL+iJxL78Z7\n2NeHQrAK2YrCZva+bcgcujFUQIEG6WigonoCdLT9tr9UbIIAMMCEkkYacvvT\nxSgsBPKGJKBEAw4yjhx+hyn+PAJFfztyVdWOt5B3RehyimneFuwFvQxFyTSf\n25f1zCAqSFACDXTQ3gwSoDoElI5tZyBAINqnuhJ+Kg9vOIOaVnSHT5ECHucK\n0OMiBxJAPCdXmGseBLoBvei5rFEStB5m/yBhjFJUIw50oIMoLvCpFRAADduj\nwxvUYMIqmvARCBiDeiwRBk+lQQTEq5qQ3CWdJSkGAlu4y9h66EBgAbF6QhSV\nMUpQilKcQRNLwIenfpFEJebBioC0ohrQQJ8QhMIfSwhgj2YouYTYUEmGqhBe\nFNBDH5otgmgLnRyLWMdq0GEGCMCHJjSBjzQE8pSChMLTCJBI4pXDBeuiiA1T\nprK7PK+SUPphsIQ1wSEag5OUKIUlyiAmAowClci0YizKILUAFi+WDQEEJOmF\nxlnMYnOVbOP0gkjBTdZRmDiwhCuywcRkmtOEpHjC1DzBABto4xqN5AcgdEXN\nNO4Ql0+CB2xctv9LM2SSgpXhZB0t0QlT+iMUkzinQquFihD452P0gGdGAPGN\nHKYxjbOAwBpxqU9+ApGXQgyoQDWRgASwoAMGMMAHDrnQhc5AkQPSU0NgYVF7\nQmAWKcBnPvc5HwGcbUVxJCInEfACQXQACUhFQkqRwAIOttScv9ABO21wA8k1\np5Z3mYXYdNqAjvLzbHDUpFCNIQoUdGAdHUhrUg2gVAOg4AXmvEAaOPEGaCCA\nAASQxBtIYYIq5kEHAaKHVfsRGB3eNBPYxKdXGVWGUnAzdOSxgyg+MIxhoDWt\nal3rUlXABEBeYBQIiMMm0AAKPBBAE1A4nTjWEIAzvGEFqsvDEHqEjZj/wMKw\n1rwlVxerGkv4AxVoAOkEmXGMOKDgA8i1LFrRioSjKrWtKRVEQlXHBBSKQhLQ\nEG3tCHCLJaSWClD0zgHO8LBqDeIYNsDGTG4ryZtak4G7lZ6G2sBSfyCAaTK7\nAzfgQIEzoOC/yKVsZS+bWeim1BsdqEG10oCANxDgDZwIRHa3O4hbaA91nlKB\nKA7QBhHo0VPwCFBtAdNea86CZVztKk8FUN5PjQIHxKWABihQBkHY+L/HTa5l\nMetcAxvAG94wQAQAkA1SIIAUBvUHdkVLgBkMwrvkPSEkVtSCJ/yCAJ5gZ20l\nwgObziITGk3xTqUHhWoxYQVdAIYINMBmO0TA/8aCwHGOBbwOAvc4pXj2RieY\nIY69ttgfpJBEHOLQ5ArTAQ2SaPAb4lAC33XsoaxYhUx4kFVrZoKSYlYxbOzg\nPX8kAM1d6AILOuEDDQzBBCaIwJvhjOMAU7bOmE0qdMUhhFozQhVxiMWnuiAJ\nQTfZyahFQydWGwA1cbiZAJL0Qiht6UzoVsxetUQaJhEKZzhDBdh+A5s9AQxU\nq3rVN241ne0sa1rXWgjbqLUd3uqPUYhCFNDAxwzm3d3vjgF/vTvAHegUaYbw\nwMSZyAR8oX0I2BwiC2eoQQ2srYJA6IDNb2ABqr39bVYDWMfkRgIVzs1xdEOD\nCjhQ4nXlPe9BaOLQNf+rRjQc0eg2DM8TyvZTs3mY6Xwy4xI2YLMGdIAAhTvD\nFWzuhKhZIHGKq9riF381rDtQho53/Bjpboc1OiEJktMbtaplrbHboCOYT9rS\nOdhopocwgiRowOw6L0MNCKCBKjwA26IW9cRTXfE4i1vAlpUEHJze8XTXehvc\n2AQ05k3vDHaiDGNYeaPNoAzGxbwf/86EHDCd4kbsyBMySII2NH92nevg4TbI\nA7ZVEGqiF93ocLb7nIdhgGMIoROW4Dvft2GHOqQiDoM3+YWJnT8O7yYL3fgI\nDwK+CrFX0lwBctUxtLH55qNd5xkYxMKvDffSn/7b4L47JYQgjnW0XvZOv0L/\nKmz/BS5sIg5QvtkavDPlO/Am+FzOBCBqgU8veEJA9LCBDRjQznIw3/lJEIBs\n5gqhUIALN3rWR3QTh31IFwcUkAiV1QEOCH4ddw8LkAqpUH5cgAtnIGzikHgs\nxzSW1w3+Jgc0Bz32Rw8DoA3lQA8yIAP6xwoj4H//B4BJYAOjoAZqYIDWRn0J\nuIB1Z3fHQAGdgHeJQIEcxwwLQH5csIHEQARE4C9aRx49oAPw5ydyIHaANUPE\nwXwtmH/6Vw5iKIb/F4DaoAGisAIroIM7WG0MR3pDd3qoJwjVQAEUAAdvEGAG\nsHcUgITFgAtLmIFNiAtQeAInMAa+UGwiyAEW8QMc//AkgKUNx7EPkLOCLOiC\nNiADIzCDY0iDm2cHLxCKbNiGPueDcVh02McJ/GWHjfABxyUJdigEfUiB+pAL\ndVAHX1B+uPCERHAChSAw8QAOHMaIE6EF3MAKkjiJxlGJljgC+UcPm7iJnch8\nDJAHoRiKaqiDBRgK01d9LDB0QFiHdmiH1YACSDCE4ziLsscIdRCIGriLhfiL\naxAPOKAKtbARPFAFQKKMywg5XuiC9ACN0TiNOwAAAHCNL5CN2siN3QiHcYhq\nwCAD6WiHomAJEzmO4LcGueCOG4gLf2OIAjOPOHCPEEFT/KiMzKgNLigDABmN\nnKgL02aQB3mNCkmKB+iNCv+IBjI2Y+O4ihcZi063DcywkReYi04Yj/ewBmuA\nAyRYEbAAAVVwkv3oj9rwgizJks4okCMwCI+ACqgwCQaJkGq4hm3IjW8YakPn\nCWxmhzz5kxfJd3iwkUx4lL0ojw/QlAnxlG4glQYCOStplS8YkJuoCwnwCIY5\nCYgZljRJlqTYg9WnbTq3lm3plrGojrVWixuJgRpIDB95AgLTCCRYkjeVAXw5\nlfqXiVa5ks64QSVlmF8JljO5mAtplj4IdJE5YzpHmenYcXCwAHKJi7rIi74Y\nD7oQms1xU71QmpQ4AOVwmvoHmAH5ABcwna3pmompmAnJmDzIcGp5m2upmxMp\ni+f/Zg9AIJeCeJSG+ACHAH8OwWyzoJyUCIOnCYOAKQP4wATTeQElVZio8AiI\nCZtiSZbbuHAIUAXemZu5CZ4YyQ250KAXeJ6c2YsCYIUYwWyZUADK6QoEwAfO\nOZ8yoANSwAT4SZ37eZjXGZtjOZshoAFQ8HAHOo6TCZ5CgAfluYS4OIhPGA8C\n4AXBtxBP+WXvWZrZ4ClhYAkdmokzgAkhKqIjqp+GaaIyGaAL+XDOEAEueqC4\nGaNuKQTWAAQ1OpceCQktcAgcYFuHJQc+wJfhADFpsAPhcJpewAZKKgVL2qTV\n2ZUnKptqMApJ8ADVZqVYKpkKaodwEAflaYvAuYFE4HIe/8CIEWGhchCkJ7kE\nJQQAHGoDZcYGckqnTGqnhWmiALqYS5AEdGCAVmqgBvqiMqagquANX3qe8cCo\njpqX1iQHsAALaWogx5FkEBMO7URCmjqnTJqfJQql2LkClpAEwNCGahABapmq\nqqqgjAAE3uCgTFgC6tEIZVoRzCYHckBpJ+kBJoQA+xcCqrOpdeqpT/qf2JkF\nSQAPOdiGLoqq0QqeVOCqDUp+RMBh+7atDgELX+atPJCPKOkAJmQJ7fRH54oJ\nc7qk+amfn+qfsAkAKqB5SeAFo7CGwBCo3smWlMkMQPaqyAAJi2AaKTBpECB5\nUdFlKJk6qoMK/McHVsSwdFqnxP9aUv3JrgRghhcbCCswqp0XmdAamTtJmXHg\nqjWaCmqCIwJwsg/RrSvLA6R5HDIAAyJAAJ3mKQQAAwxwC4Akp8Iqog9bna+5\nA2V4g+kUgM/HZlUwtB2rparwYzWKB/nzAG3QtBVaq1HxA5+wl8cBA1iABTCg\nCyGgsK7Af1lrReiariTKn6ggAmTIfDfIAJuntt7pth2bjnAABHKbC74ADi13\nByfLrQG7sp/AA8dBD4EruIILAy0ABboAA66ATMHKqcMKsZ/aCNMouWrbu2vb\nthw7kdUgt3VgP41WsinwEPzwb7NgqzzwA3xrCMYBuKu7ujBwvTBAAOYEtrbr\nqQkwg5z/GLmVa7GWy7EJmo7ccGB4gAxp8i3SMLoNEXnOywOf8AmwsA/aUL3V\ni726QELJtLi3W1ICWQ7SGLm+67tCi6UeSwGb8GOFkC1L+74uAbAq+7z1Sw0F\nwACXcAmBy8H6O7sLxb22O52k4IwD2Yk0SL69a763KWOJgAQLACnFBgl267Qy\nV8H0+wnUgAEb3MMbrL/a+1SaWrNMSgpYqZUEPIY1qMICyMJtCQSB4wv2czjw\nC3mla8E6nAzcEA4+jAU/HLiJG8IAbMRW6ZLgq8S8e8BOPGM4cDtSDLqboQD4\neMV8m8VXkAV47MMeDMJP9SmLiw82oAOpicThm8IHXL6BSgEn/4AHhbAsaRLH\nMSG/e3vBjojHWRADeowFg9DHEMO9DmADDjAK1ZCaLknAhZzGaoyl3IALXHAC\nMry0cjwR8juwz0sN1OBs3HDJlpwFl8DLvMrJnqKpUADKIUoKD1DGpVzAZ3vI\nWKoIxNDKr0yysRy/dKzDP3BTChADunzJlxAOygDMJkQANlAGmMCk+CDI0KiV\nBYzGh9zEOmcDRPCEjEwlI3IACtARkmzB1JBRs9AN3KDN2mzJZQDOJRQGNmAH\nDSuiyhCYL2jGKIzKCMxmdwCFRMDIb9xo07y8V1y/14wXVxADIA3QWRDEBF0t\nBi0CAOwKgDkCmmjGpzy+anwPvbjIJ//gyBitvLNswRmVVewQ0iL9yyVt0PVA\nAIsLBfVJytK4zuXQzknADIZoiIVABNEsx8vWvN/6vJRmU6vw0T4tsyWtOvxn\nA+EABQCgpID8gqh5lQ6dxGR4yIrgi78o01MdyVY9sJ+QCd+ARlmVzT490F8N\nMTEQ1gwQDiGwPh260i2dzJ3Yu8eAO/fw2BVwD408w7UAEv9mqyubQBe1Q/98\nCCA9A38NMSLAf4JtAyFw2Gnd0Il9wmKotm0Q10o5j41svFQtc/M7CwmU1/ZU\nC559CLrwC6FdLSFA2sR9pB5anw4dvlUZDyE5j/SINKBb2RRx2ZldHUxyFxwQ\nA70d3NUCBa7/QtyljdrIvdZj6AFKGQ/oTY84YA8PnCb3ON11PQv0dN0QgA1X\noAuH4Fvc7SkIwABcC97hfdiIvdrgSwnOrd72QAkGDsHSnRDD57wS0g4NcAVb\ncN1bkAKHcAh+vd95cL3+DeABPp+pjcybeAnojQMobg8JTgmqQAlSrAjSHb8q\nOwvT0QDocOMTQAJ6UARk4M+HANr77SnY6+Egrn/tdKTjHY2LkOIqruCq8OR2\n8MYk6ScqSyiGQAI3fuNRsOVRMAEKcAjAHeT+cARD/t8g3k5HLuJHLQMMYA/r\nreAsbhv48QCUYD8NDnmSR+MF0At/YARGoOXoEAW8QAscMARhHNwh/1DmHm7m\nxZ3mxw2Y1rDicY4ft/EAlp4tlS3LkndD3ODnfp7lW14EW7AHYu4pg9C6Zc5/\njE7a+4fkad3iTy7nlW4KtC4N9hAAU47nR1IAwtAMno4Of77labQHrVDqYWC9\nis61qx7i83kIsU7plk7rppAI1G4K0UCSDp4JbgAdJNAMvv7pOL4YViAPpe4P\n+pvsy87qrT6ftQHtiUPr1K4M+9EC9nDnlOYDg+EDf+Dt3/7n6EALi0EL+VDu\nD4DsqI69ql7kjo4F7r4IpiAN8T7vjdAIdmDv74DvPsAN/O7tv14EiUECUQAC\npV4G+ovsqf7hAH6a1jDr8E7tLaAbE+8FMv//3n6S79MwBDuw7xzv6e2gGBMQ\nBadQ6gSABQ5AAA4gAodg8kOe8GduCu8O8S7/8jHfH5/HDiWRDH6QA9hwK4PB\nDfbyBLRAAtPxDbaw5X0g5mlwCXzsMwgABUdw8Aif7ocg7fEu9VP/eUPwCmDw\nAzPxA+TgBxgQ+BBgMpUjKNQR6FEwB6WuDJdw6AAQuMnO9KQNI3UP8x0DQHoP\nBmBABnuxEH4f+KAP+LitPNNRDFq+DCN/CSQt3Psb+fyXBZU/8ZevA5mv+Zqf\nAz/AED+gBeQA+r4f+DkAAShTBKAu8kFOAOFQDQV97oqu6o0g8TFP+7Vv+5Ug\nC9+q+1PQ+7//+1n/DwFF4O/osAFiDgB4DNT+UPDWC/lljgV23zF5b/vwXwny\njw3f+hE/kP1TsP36/wxNABBNeEVBp87fQYQJFS5k2NBOjGoEwvxKSOASFowZ\nscDgyHFIo0ZehrwCU9JkyUopK8nKlIkHP379+P2YMoUcBpw5deZ8RohQE6Cn\nGg4lOnRGDKRZsoS7pMPSA6YXNWLsKJLkSZOVwKhMGSTTrJf9ZNKcomXKTrQY\nevr02cSIvKJxi6aJkaVuXaZMs1ziO5UqPawnuXK9AWEW2Jhja9pMuzMd27YW\nLNga10fuZYUPkdZdqpTv575YbJQbkCHw1sEpb9wQMstwWLFkbfppjJPc/wTI\nhHhJ5r0BBGbMRzfb7ez5MwwbpTMsx5pa9eob2CBM5yETpmzGtTE8hrybN29b\nc1oBn6trc9K7nhmUy6BcOUrn0KHLcr0FQvWYMxdnb3w7t/fvwFMiFvKG0uw8\n4kRLYjkGG0RtMPlWc+GGdyCwbwtYrOsHu7K0a+K/AEO04K0CF8InBvPOg2GE\nKpZTrsHSUotwwgnnmW4LHGGBKbb9bMqhsSly082CW0QMkDLLSvQHFQFiOESX\nLGzQpkUY22swA8Lko9EFLqfBEcdvMhRrwx610OLHtJ5Rc01ahHnCzTeFkXNO\nOfWQkwQ6NNFzTz2X0GQJQAMVdJEYsBhBAyrbK/9tgBcbrCTCG7bkkstvvvwm\nzPzI7JEcNLXDCYICQhXVkAIMMdWQd0x1Y9VdiuHGA1hjhfWQQzyg9dZDYmBg\nyioSVfRKFwfYZ8ZIJ3XhGhe83OLSSwEZU78ea+pUO2wK8MFaUUMl9dReDOll\n1VXbuYIZWWOl1dZDLpGhV3YZXLTR9vZhUMJijUX2mmveYRZcQDLlsCZOp21s\nCx+uLTjbbE/11ttv3diFkSHKRReGcthtN1hgrdxH2Awk5fJefK+ZZ9lvVvXW\n2cT+ZSwHgdHCpmCYDb4WYVNL7baXbsN9FdYYbKDA4otddBdYeffZx9iPjw35\nmmlKNtnUfmXSNNqAW9b/6eWYY8YWYW0V7tYQhxWAwwege61y6OXkbdDoSUFe\nWuR3wP3akKhjUtlHlqklG+YqsjaY620VNgQDMcQQouwrX3zR6KKFZfttyKtw\n+utQnRUL2mjLYjnvtLDpu9e9/ZYZ8FK3maLwwn8OmlF3lWNc7df3gfzteaZZ\n+NTKx5y6RxJ69/333mvBwHOLQ/fhiR2SV34HS47hmnAafJ9gh3AaDMcB7LE/\nIoPY441dhOzDz94VN3DPNmoeM5drAyfK7lWH34baYetVCidBIT6C5UMhB4r2\nn3FheSANRVGCwhBmObtlbgqXyYYNyuYFAMQFCtPwQf3spxAraGBRR+Af91wX\n/zsPoCIuCCAV13yAMsWo7zIOaJHFSHEZHZABdWK4X0JoIAENLIeDCXFA2rgX\nuwG8MC6kKGGoZuaDTEhtd/vBTBoyYLYqeAEzFpihGCagEBqIQQJVGMAOEdLD\n2L0uHJdBAMIOhsTELHExwLnS/i6zAQlIQItWxKIccejGL/4wjPvw4kHSQApA\nBhKQUDCiEWE2C93dTSEW2EMjaWABhbgnA3g8SAj4cElK+kMJWoyjBK6YECtw\nUgKZ7N8ejdZHfzjgGgNY5SpnZsisJXFHikwICTLBskzUECFtxJ/FFKKETmrx\nkwixQiclYAX+mfKUCpnBEZzpzHpkS2Yxm0ViMNcjhf+QABs5uKUuD9KoTOaP\nQb80picxaExk8lCZfIxLNuBhrWnurZpjoiVCbAkBbnrTH2pbTjgZVAVyGnOY\nBylmJ9P5xXWOUS6WEB3ZqgmTazLxMk40WntQub3lbIOc7OjkQP1RUI4e9CCl\nfJ3jjCbEogDAE6KrAiKlVs+4gJF7GUDlDLLnUWCyg6Ps8GgxdyrSVK5zH/WI\noARjZjFEQhSmRCEFg9SGSqIoQadT7alOJcAOoJJUmeFA6VBIETqk+ssPKizK\nDorxwx9CdShSvapOqzpVoO7ApMocgAdcIb74HeSroEOqEn8w1mgVRR0KyEEw\nKqoctTZEquzggFsVooepskP/DwqZAAfmakpGvc4HXSXF54CWVLthALASRYhB\nFpmDd4QxsQxRQmNd61HITnWyCVHC9MTnCsY9U7dH4AM8spGQvVrsiRB4Fg/8\ncFxsJmQDHvUHLQyhWsy01rXs2MFj2ZGC6862KKRgHGY6K9zlEPdyP8AJcteo\n3ClsQCHq0AF0QdkN+HbjlxygL31hO13tMrW7lwkB0BiUoR3x4EfmrYlCNjAF\nCRAoIWmwQexQqQcyxHe+9eXAfVOQAg7k16v7jQsAHGi2Bv0gUzyQQ05Ga+Cy\n0MBEDsZgN8gQ4QnXt7oJ0QOGOZACDTeEu0aTCwC80EKhDcAHMDGHWATMsuMC\nFsVl/9GnP0Jg0kw24MUv/qUTOGDlCj8WETfGsVx2vI+UzsATIFZUaTIRk3QY\n+ZYlFq0Ce5QJHBXgdU+MRCSwEYlVBCHPQZhyn7vhhD9fWdAc2DKhKXxhRCc6\n0Yi4LOPcl6hGVUFqc4gJLGaxufKO1s2VkrOj63znOkciCKMedZ+n7ARUp1rQ\niLAyIlyNYURcONaInrWs9ci4JyJOaFYawDzP8Q+ZwAICLckbgd08i290eh9V\nCIadQw3qO5Oa1H1GNRlSjeorO2HLruZ2rLudAm+Dm9Gxcx/GXmSIMbnjH5W2\nzy2RbOzM+cENBRAWs0N9b3zXWdp8pra1r61tbXdb4N/2Nv8i5gzeIJd5Gjui\nwT+AzQ9YVGrYnNO0Agm27GBkvNnNzje+921qf/+b1QEfuMDFPe5lk/lspUG3\nWKbQCofLBBBuwNEs3C3aikcrB2TTeM81HgmOd3zf/PZ3yFPNaqSXfODF0EDK\nE9e6liZmCvJwOLD7AQhU2efSbG6zm7VgiG1ofBc+//nGgZ7vbYw67aVux4v/\nfXSSK53by/HVrzIwDZTBBANUrzpMeAAIWASeB4P/AQ9+cHjEJx7xWgDE5nLQ\neMdHXvKbg/zkMZ23H/1oFRjYPOc9v3nQ58Aw0xn9LACvO7HQAOZVf/jl0ii1\nHcXe9bPX3euftaPL5R71tIf97nsy7/o0WlP2r4/JOU7B+r5nqva7jz1EdZ97\n4qNe+bonfvCfVXvly1762beOOdLBd+Q7PCAAOw==\n",
            //     "filename": "rails_logo.gif"
            //   }
            // }

            Chilkat.JsonObject json = new Chilkat.JsonObject();
            //json.UpdateInt("image.variant_ids[0]", 808950810);

            for (int i = 0; i < variantIds.Count; i++)
            {
                json.UpdateString("image.variant_ids["+i+"]", variantIds[i]);
            }

            //Get image
            //https://xxxxxx.myshopify.com/admin/api/2023-07/products/8097888436482/images.json

            string imgbase = getImageBase64(productId);





            json.UpdateString("image.attachment", imgbase);
            json.UpdateString("image.filename", "image.jpeg");

            rest.AddHeader("Content-Type", "application/json");
            rest.SetAuthBasic(ShopifyAPIKey, ShopifyAPISecretKey);

            Chilkat.StringBuilder sbRequestBody = new Chilkat.StringBuilder();
            json.EmitSb(sbRequestBody);
            Chilkat.StringBuilder sbResponseBody = new Chilkat.StringBuilder();
            success = rest.FullRequestSb("POST", "/admin/api/2023-07/products/"+ productId + "/images.json", sbRequestBody, sbResponseBody);
            if (success != true)
            {
                Console.WriteLine(rest.LastErrorText);
                return;
            }

            int respStatusCode = rest.ResponseStatusCode;
            Debug.WriteLine("response status code = " + Convert.ToString(respStatusCode));
            if (respStatusCode >= 400)
            {
                Debug.WriteLine("Response Status Code = " + Convert.ToString(respStatusCode));
                Debug.WriteLine("Response Header:");
                Debug.WriteLine(rest.ResponseHeader);
                Debug.WriteLine("Response Body:");
                Debug.WriteLine(sbResponseBody.GetAsString());
                return;
            }

            Chilkat.JsonObject jsonResponse = new Chilkat.JsonObject();
            jsonResponse.LoadSb(sbResponseBody);

            jsonResponse.EmitCompact = false;
            Console.WriteLine(jsonResponse.ToString());
        }

        private static string getImageBase64(string productId)
        {
            Chilkat.Rest rest = new Chilkat.Rest();
            rest.SetAuthBasic(ShopifyAPIKey, ShopifyAPISecretKey);
            bool success;

            // URL: https://your-development-store.myshopify.com/admin/api/2023-07/products/632910392/images.json
            bool bTls = true;
            int port = 443;
            bool bAutoReconnect = true;
            success = rest.Connect("xxxxx.myshopify.com", port, bTls, bAutoReconnect);
            if (success != true)
            {
                Debug.WriteLine("ConnectFailReason: " + Convert.ToString(rest.ConnectFailReason));
                Debug.WriteLine(rest.LastErrorText);
                return" ";
            }

            // Note: The above code does not need to be repeatedly called for each REST request.
            // The rest object can be setup once, and then many requests can be sent.  Chilkat will automatically
            // reconnect within a FullRequest* method as needed.  It is only the very first connection that is explicitly
            // made via the Connect method.


            Chilkat.StringBuilder sbResponseBody = new Chilkat.StringBuilder();
            success = rest.FullRequestNoBodySb("GET", "/admin/api/2023-07/products/"+productId+"/images.json", sbResponseBody);
            if (success != true)
            {
                Debug.WriteLine(rest.LastErrorText);
                return "";
            }

            int respStatusCode = rest.ResponseStatusCode;
            Debug.WriteLine("response status code = " + Convert.ToString(respStatusCode));
            if (respStatusCode >= 400)
            {
                Debug.WriteLine("Response Status Code = " + Convert.ToString(respStatusCode));
                Debug.WriteLine("Response Header:");
                Debug.WriteLine(rest.ResponseHeader);
                Debug.WriteLine("Response Body:");
                Debug.WriteLine(sbResponseBody.GetAsString());
                return "";
            }

            Chilkat.JsonObject jsonResponse = new Chilkat.JsonObject();
            jsonResponse.LoadSb(sbResponseBody);

            jsonResponse.EmitCompact = false;
            Debug.WriteLine(jsonResponse.Emit());

            JsonArray images = jsonResponse.ArrayOf("images");

            JsonObject imageObj = images.ObjectAt(0);

            string url = imageObj.StringOf("src");

            return ImageLinkToBase64(url);
        }

        public static string ImageLinkToBase64(string url)
        {
            using (var handler = new HttpClientHandler { })
            using (var client = new HttpClient(handler))
            {
                var bytes = client.GetByteArrayAsync(url);
                bytes.Wait();
                return Convert.ToBase64String(bytes.Result);
            }
        }

        private static string GetAllVariants(string id)
        {
            Chilkat.Rest rest = new Chilkat.Rest();
            bool success;
            rest.SetAuthBasic(ShopifyAPIKey, ShopifyAPISecretKey);

          
            // URL: https://xxxxxx.myshopify.com/admin/api/2023-07/products/8096080920834/variants.json
            success = rest.Connect("xxxxxx.myshopify.com", 443, true, true);
            if (success != true)
            {
                Debug.WriteLine("ConnectFailReason: " + Convert.ToString(rest.ConnectFailReason));
                Debug.WriteLine(rest.LastErrorText);
                return null;
            }

            Chilkat.StringBuilder sbResponseBody = new Chilkat.StringBuilder();
            success = rest.FullRequestNoBodySb("GET", "/admin/api/2023-07/products/"+ id + "/variants.json", sbResponseBody);
            if (success != true)
            {
                Debug.WriteLine(rest.LastErrorText);
                return null;
            }

            int respStatusCode = rest.ResponseStatusCode;
            Debug.WriteLine("response status code = " + Convert.ToString(respStatusCode));
            if (respStatusCode >= 400)
            {
                Debug.WriteLine("Response Status Code = " + Convert.ToString(respStatusCode));
                Debug.WriteLine("Response Header:");
                Debug.WriteLine(rest.ResponseHeader);
                Debug.WriteLine("Response Body:");
                Debug.WriteLine(sbResponseBody.GetAsString());
                return null;
            }

            Chilkat.JsonObject jsonResponse = new Chilkat.JsonObject();
            jsonResponse.LoadSb(sbResponseBody);

            jsonResponse.EmitCompact = false;
            var result = jsonResponse.Emit();
            return result;
        }

        private static void GetShopifyItem(string id)
        {
            Chilkat.Rest rest = new Chilkat.Rest();
            bool success;

            rest.SetAuthBasic(ShopifyAPIKey, ShopifyAPISecretKey);

            success = rest.Connect("xxxxxx.myshopify.com", 443, true, true);
            if (success != true)
            {
                Debug.WriteLine(rest.LastErrorText);
                return;
            }

            Chilkat.StringBuilder sbJson = new Chilkat.StringBuilder();
            success = rest.FullRequestNoBodySb("GET", "/admin/products.json?ids=8096080920834", sbJson);
            if (success != true)
            {
                Debug.WriteLine(rest.LastErrorText);
                return;
            }

            if (rest.ResponseStatusCode != 200)
            {
                Debug.WriteLine("Received error response code: " + Convert.ToString(rest.ResponseStatusCode));
                Debug.WriteLine("Response body:");
                Debug.WriteLine(sbJson.GetAsString());
                return;
            }

            Chilkat.JsonObject json = new Chilkat.JsonObject();
            json.LoadSb(sbJson);

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

        private static List<string> sizesCache = new List<string>();
        private static void AddItem()
        {
            int count = 0;
            var itemList = ShopifyManager.GetItemListtoUpload();
            string product_tags = string.Empty;

            bool success;
            Chilkat.StringBuilder sbJson = new Chilkat.StringBuilder();
            Chilkat.Rest rest = shopifyAccess();
            foreach (var invItm in itemList)
            {
  
                int counter = 0;

                var item = ShopifyManager.GetItemInventoryByItemGroupCode2All(invItm.ItemGroupCode2);

                if (item.FirstOrDefault() == null)
                    continue;
                count++;
                string time = DateTime.Now.ToString("hh:mm:ss");
                Console.WriteLine("{1} - Index:{2} - Submitting to website {0} ", item.FirstOrDefault().ItemGroupCode2, time, count);


                Chilkat.JsonObject jsonReq = new Chilkat.JsonObject();

                var itemMarketing = ShopifyManager.GetItemMarketing(item.First().ItemGroupCode2);
                if (itemMarketing == null)
                {
                    Console.WriteLine("No marketing data", item.First().ItemGroupCode2);
                    continue;
                }
                var itemImages = ShopifyManager.GetItemInventoryImageListByItemGroupCode2(item.First().ItemGroupCode2);
                if (itemImages.Count == 0)
                {
                    Console.WriteLine("No image data", item.First().ItemGroupCode2);
                    continue;
                }
                string itemCat = string.Empty;
                var ebayItemCat = ShopifyManager.GetEBayCategoryList().Where(x => x.Id == Convert.ToInt16(itemMarketing.CategoryNumber1)).FirstOrDefault();
                if (ebayItemCat == null)
                {
                    itemCat = null;
                }
                else
                    itemCat = ebayItemCat.CategoryName;

                string _titleGender = string.Empty;

                var code2Arr = item.First().ItemGroupCode2.Split('|');
                invItm.BrandName = code2Arr[1];
                invItm.ItemName= code2Arr[0];
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
                    product_tags = string.Format("{0}, {1}, {2}, {3} ,{4}, {5}, {6}", invItm.BrandName.ToTitleCase(), _titleGender, invItm.ItemName.ToTitleCase(), item.First().MasterColorName.ToTitleCase(), //Color
                                                        item.First().MasterColorName.ToTitleCase(), ebayItemCat.CategoryName, itemCat + _width);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("null error exception");
                    Console.WriteLine(ex.ToString());
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

                jsonReq.UpdateString("product.title", itemTitle.ToTitleCase());
                jsonReq.UpdateString("product.body_html", itemMarketing.EBayDescription);
                jsonReq.UpdateString("product.vendor", item.First().BrandName.ToTitleCase());
                jsonReq.UpdateString("product.product_type", pType);
                jsonReq.UpdateString("product.collections", pType);
                jsonReq.UpdateString("product.options[0].name", "Color");
                jsonReq.UpdateString("product.options[1].name", "Size");
                //jsonReq.UpdateString("product.options[2].name", "Width");
                //jsonReq.UpdateString("product.options[1].values[0]", item.First().Color.ToTitleCase());

                sizesCache.Clear();
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
                            //jsonReq.UpdateString("product.options[2].name", "Width");
                            jsonReq.UpdateString(string.Format("product.variants[{0}].option1", counter), itm.Color.ToTitleCase()); //Color
                            jsonReq.UpdateString(string.Format("product.variants[{0}].option2", counter), itm.Size);
                            jsonReq.UpdateString(string.Format("product.variants[{0}].option3", counter), itm.Width);
                            jsonReq.UpdateString(string.Format("product.variants[{0}].compare_at_price", counter), ((decimal)itm.MaxSalesPrice).ToString("0.00"));
                            jsonReq.UpdateString(string.Format("product.variants[{0}].price", counter), ((decimal)itm.MinSalesPrice).ToString("0.00"));
                            jsonReq.UpdateString(string.Format("product.variants[{0}].sku", counter), itm.ItemCodeFull);
                            jsonReq.UpdateString(string.Format("product.variants[{0}].barcode", counter), itm.UPCCode);
                            jsonReq.UpdateString(string.Format("product.variants[{0}].inventory_quantity", counter), itm.QuantityOnHand.ToString());
                            jsonReq.UpdateString(string.Format("product.variants[{0}].fulfillment_service", counter), "manual");
                            jsonReq.UpdateString(string.Format("product.variants[{0}].inventory_management", counter), "shopify");
                            jsonReq.UpdateString(string.Format("product.variants[{0}].inventory_policy", counter), "deny");
                            jsonReq.UpdateString(string.Format("product.variants[{0}].currency_code", counter), "USD");
                            jsonReq.UpdateString(string.Format("product.variants[{0}].requires_shipping", counter), "true");
                            jsonReq.UpdateString(string.Format("product.variants[{0}].weight", counter), itm.ShippingGrossWeight.ToString());
                            jsonReq.UpdateString(string.Format("product.variants[{0}].weight_unit", counter), "oz");
                            sizesCache.Add(itm.Size.ToString());
                            sizeList = sizeList + "," + itm.Size;

                            counter++;
                            break;

                        default:
                            jsonReq.UpdateString(string.Format("product.variants[{0}].option2", counter), itm.Size);
                            jsonReq.UpdateString(string.Format("product.variants[{0}].option1", counter), itm.Color.ToTitleCase()); //Color
                            jsonReq.UpdateString(string.Format("product.variants[{0}].compare_at_price", counter), ((decimal)itm.MaxSalesPrice).ToString("0.00"));
                            jsonReq.UpdateString(string.Format("product.variants[{0}].price", counter), ((decimal)itm.MinSalesPrice).ToString("0.00"));
                            jsonReq.UpdateString(string.Format("product.variants[{0}].sku", counter), itm.ItemCodeFull);
                            jsonReq.UpdateString(string.Format("product.variants[{0}].barcode", counter), itm.UPCCode);
                            jsonReq.UpdateString(string.Format("product.variants[{0}].inventory_quantity", counter), itm.QuantityOnHand.ToString());
                            jsonReq.UpdateString(string.Format("product.variants[{0}].fulfillment_service", counter), "manual");
                            jsonReq.UpdateString(string.Format("product.variants[{0}].inventory_management", counter), "shopify");
                            jsonReq.UpdateString(string.Format("product.variants[{0}].inventory_policy", counter), "deny");
                            jsonReq.UpdateString(string.Format("product.variants[{0}].currency_code", counter), "USD");
                            jsonReq.UpdateString(string.Format("product.variants[{0}].requires_shipping", counter), "true");
                            jsonReq.UpdateString(string.Format("product.variants[{0}].weight", counter), itm.ShippingGrossWeight.ToString());
                            jsonReq.UpdateString(string.Format("product.variants[{0}].weight_unit", counter), "oz");
                            sizesCache.Add(itm.Size.ToString());
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
                    Thread.Sleep(10000);
                    rest = shopifyAccess();
                    continue;
                }

                if (rest.ResponseStatusCode != 201)
                {
                    Console.WriteLine("Received error response code: " + Convert.ToString(rest.ResponseStatusCode));
                    Console.WriteLine("Response body:");
                    Console.WriteLine(sbJson.GetAsString());

                    ShopifyManager.UpdateWebSiteMasterGroup(item.FirstOrDefault().MasterGroupCode, 0, true, "ERROR", null);
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


                string masterGroupID = item.FirstOrDefault().MasterGroupCode;
                //add product id to table
                var returnID = ShopifyManager.UpdateWebSiteMasterGroup(masterGroupID, 1, true, "DONE", productId_);

                //update inventory mastergroupcode

                foreach (var itm in item)
                {
                    ShopifyManager.UpdateMasterGroupCode(itm, masterGroupID, productId_);
                }
                Console.WriteLine("Sucsess... MasterGroupID : {0}, ProductID: {1}", masterGroupID, productId_);

               

               
                Console.WriteLine("Editing Metafields for: {0}", productId_);
                updateVariants(productId_);
                UpdateMetafieldColor(productId_, item.First().MasterColorName);
                Console.WriteLine("done");

                //var variantJson = GetAllVariants("8096080920834");


            }
            Console.WriteLine($"{count} items");
          
            Console.WriteLine("Finished.");
            Console.ReadLine();
        }

       
        private static void UpdateMetafieldSize(string variantID, string size)
        {
            Chilkat.Rest rest = new Chilkat.Rest();
            bool success;
            rest.SetAuthBasic(ShopifyAPIKey, ShopifyAPISecretKey);
            // URL: https://your-development-store.myshopify.com/admin/api/2023-07/variants/808950810.json
            bool bTls = true;
            int port = 443;
            bool bAutoReconnect = true;
            success = rest.Connect("xxxxx.myshopify.com", port, bTls, bAutoReconnect);
            if (success != true)
            {
                Debug.WriteLine("ConnectFailReason: " + Convert.ToString(rest.ConnectFailReason));
                Debug.WriteLine(rest.LastErrorText);
                return;
            }

            // Note: The above code does not need to be repeatedly called for each REST request.
            // The rest object can be setup once, and then many requests can be sent.  Chilkat will automatically
            // reconnect within a FullRequest* method as needed.  It is only the very first connection that is explicitly
            // made via the Connect method.

            // Use this online tool to generate code from sample JSON: Generate Code to Create JSON

            // The following JSON is sent in the request body.

            // {
            //   "variant": {
            //     "id": 808950810,
            //     "metafields": [
            //       {
            //         "key": "new",
            //         "value": "newvalue",
            //         "type": "single_line_text_field",
            //         "namespace": "global"
            //       }
            //     ]
            //   }
            // }

            Chilkat.JsonObject json = new Chilkat.JsonObject();
            json.UpdateString("variant.id", variantID);
            json.UpdateString("variant.metafields[0].key", "mainssize");
            json.UpdateString("variant.metafields[0].value", size);
            json.UpdateString("variant.metafields[0].name", "MainSize");
            json.UpdateString("variant.metafields[0].type", "single_line_text_field");
            json.UpdateString("variant.metafields[0].namespace", "global");

            rest.AddHeader("Content-Type", "application/json");


            Chilkat.StringBuilder sbRequestBody = new Chilkat.StringBuilder();
            json.EmitSb(sbRequestBody);
            Chilkat.StringBuilder sbResponseBody = new Chilkat.StringBuilder();
            success = rest.FullRequestSb("PUT", "/admin/api/2023-07/variants/"+ variantID + ".json", sbRequestBody, sbResponseBody);
            if (success != true)
            {
                Debug.WriteLine(rest.LastErrorText);
                return;
            }

            int respStatusCode = rest.ResponseStatusCode;
            Debug.WriteLine("response status code = " + Convert.ToString(respStatusCode));
            if (respStatusCode >= 400)
            {
                Debug.WriteLine("Response Status Code = " + Convert.ToString(respStatusCode));
                Debug.WriteLine("Response Header:");
                Debug.WriteLine(rest.ResponseHeader);
                Debug.WriteLine("Response Body:");
                Debug.WriteLine(sbResponseBody.GetAsString());
                return;
            }

            Chilkat.JsonObject jsonResponse = new Chilkat.JsonObject();
            jsonResponse.LoadSb(sbResponseBody);

            jsonResponse.EmitCompact = false;
            Debug.WriteLine(jsonResponse.Emit());

        }


        private static void UpdateMetafieldColor(string productID, string color)
        {
            Chilkat.Rest rest = new Chilkat.Rest();
            bool success;
            rest.SetAuthBasic(ShopifyAPIKey, ShopifyAPISecretKey);
      
            bool bTls = true;
            int port = 443;
            bool bAutoReconnect = true;
            success = rest.Connect("xxxxx.myshopify.com", port, bTls, bAutoReconnect);
            if (success != true)
            {
                Debug.WriteLine("ConnectFailReason: " + Convert.ToString(rest.ConnectFailReason));
                Debug.WriteLine(rest.LastErrorText);
                return;
            }

          

            Chilkat.JsonObject json = new Chilkat.JsonObject();
            json.UpdateString("product.id", productID);
            json.UpdateString("product.metafields[0].key", "maincolor");
            json.UpdateString("product.metafields[0].value", color);
            json.UpdateString("product.metafields[0].name", "MainColor");
            json.UpdateString("product.metafields[0].type", "single_line_text_field");
            json.UpdateString("product.metafields[0].namespace", "global");

            rest.AddHeader("Content-Type", "application/json");


            Chilkat.StringBuilder sbRequestBody = new Chilkat.StringBuilder();
            json.EmitSb(sbRequestBody);
            Chilkat.StringBuilder sbResponseBody = new Chilkat.StringBuilder();
            success = rest.FullRequestSb("PUT", "/admin/api/2023-07/products/" + productID + ".json", sbRequestBody, sbResponseBody);
            if (success != true)
            {
                Debug.WriteLine(rest.LastErrorText);
                return;
            }

            int respStatusCode = rest.ResponseStatusCode;
            Debug.WriteLine("response status code = " + Convert.ToString(respStatusCode));
            if (respStatusCode >= 400)
            {
                Debug.WriteLine("Response Status Code = " + Convert.ToString(respStatusCode));
                Debug.WriteLine("Response Header:");
                Debug.WriteLine(rest.ResponseHeader);
                Debug.WriteLine("Response Body:");
                Debug.WriteLine(sbResponseBody.GetAsString());
                return;
            }

            Chilkat.JsonObject jsonResponse = new Chilkat.JsonObject();
            jsonResponse.LoadSb(sbResponseBody);

            jsonResponse.EmitCompact = false;
            Debug.WriteLine(jsonResponse.Emit());

        }


    }
}
