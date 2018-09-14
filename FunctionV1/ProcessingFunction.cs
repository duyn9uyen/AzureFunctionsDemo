using System.IO;
using DataAccess;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System.Linq;
using System;

namespace FunctionV1
{
    public static class ProcessingFunction
    {
        [FunctionName("ProcessingFunction")]
        public static void Run([BlobTrigger("processing/{name}", Connection = "AzureWebJobsStorage")]string myBlob, string name, TraceWriter log)
        {
            log.Info($"Processing file name:{name} \n Size: {myBlob.Length} Bytes");

            var fullRecipeSet = Helper.XmlObj<RecipeSet>.DeserializeData(myBlob);
            // create xml document
            var doc = Helper.XmlObj<TaxonomyTypes>.SerializeToXmlDoc(myBlob);

            var thisBlobStorage = Helper.GetBlobStorage("processing", log);
            var successBlobStorage = Helper.GetBlobStorage("success", log);
            var failureBlobStorage = Helper.GetBlobStorage("failure", log);

            try
            {
                using (var ctx = new RecipeContext())
                {
                    ctx.Recipe.AddRange(fullRecipeSet.Recipes.Recipe);
                    ctx.SaveChanges();

                    successBlobStorage.UploadXmlFile(doc, name);
                }
            }
            catch (Exception ex)
            {
                log.Info("Error saving recipes to database. " + ex.Message + " " + ex.InnerException);
                failureBlobStorage.UploadXmlFile(doc, name);
            }

            thisBlobStorage.DeleteFile(name);
        }
    }
}
