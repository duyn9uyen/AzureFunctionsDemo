using System.IO;
using DataAccess;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System.Linq;
using System;
using DataAccess.XmlClasses;
using DataAccess.POCOs;
using DataAccess.AutoMapperConfig;
//using AutoMapper;

namespace FunctionV1
{
    public static class ProcessingFunction
    {
        static ProcessingFunction()
        {
            MapInitializer.Activate();
        }

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
                    // Save taxonomies
                    foreach (var taxElement in fullRecipeSet.TaxonomyTypes.Taxonomy)
                    {
                        var taxdto = MapInitializer.Mapper.Map<Taxonomy>(taxElement);
                        ctx.Taxonomy.Add(taxdto);
                    }

                    // Todo: Save unique facets so that we don't have duplicate taxonomyid and name combinations.

                    // Map xml object to POCO then save off recipe
                    foreach (var recipeElement in fullRecipeSet.Recipes.Recipe)
                    {
                        var recipeDto = MapInitializer.Mapper.Map<Recipe>(recipeElement);
                        ctx.Recipe.Add(recipeDto);
                    }

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
