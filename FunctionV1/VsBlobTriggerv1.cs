using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System.Configuration;
using System;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Linq;
using DataAccess;

namespace FunctionV1
{
    public static class VsBlobTriggerV1
    {
        [FunctionName("VsBlobTriggerV1")]
        public static void Run([BlobTrigger("xmldropsv1/{name}", Connection = "AzureWebJobsStorage")]string myBlob, string name, TraceWriter log)
        {
            log.Info($"Xml dropped, processing parent file name:{name}; Size: {myBlob.Length} Bytes");

            var fullRecipeSet = Helper.XmlObj<RecipeSet>.DeserializeData(myBlob);

            // order by recipe ids
            fullRecipeSet.Recipes.Recipe = fullRecipeSet.Recipes.Recipe.OrderBy(x => x.Recipe_id).ToList();

            var batchSize = Convert.ToInt32(ConfigurationManager.AppSettings["RecipeBatchSize"]);

            var thisBlobStorage = Helper.GetBlobStorage("xmldropsv1", log);
            var processBlobStorage = Helper.GetBlobStorage("processing", log);

            int count = 0;
            int index = 0;
            // split into smaller documents and move to 'processing' blob. This will allow for large file processing to succeed.
            while (count < fullRecipeSet.Recipes.Recipe.Count)
            {
                index++;

                // take recipes in batches and to build a new document
                var addToAdd = fullRecipeSet.Recipes.Recipe.Skip(count).Take(batchSize);

                // Add recipes and any shared xml data to the file.
                var subRecipeSet = new RecipeSet();
                subRecipeSet.Recipes.Recipe.AddRange(addToAdd);
                subRecipeSet.TaxonomyTypes = fullRecipeSet.TaxonomyTypes;

                // create a new xml document
                var newRecipeDoc = Helper.XmlObj<TaxonomyTypes>.SerializeToXmlDoc(subRecipeSet);

                // Upload document to 'processing' blob
                var filename = name.Insert(name.ToLower().IndexOf(".xml"), "_" + index); //append the index count to the original filename. Ex. recipe20180130_1.xml
                processBlobStorage.UploadXmlFile(newRecipeDoc, filename);

                count += batchSize;
            }

            thisBlobStorage.DeleteFile(name);
        }
    }
}
