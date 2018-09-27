using System.IO;
using DataAccess;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System.Linq;
using System;
using DataAccess.XmlClasses;
using DataAccess.POCOs;
using DataAccess.AutoMapperConfig;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace DemoApp1
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

            // get all the blob storages
            var thisBlobStorage = Helper.GetBlobStorage("processing", log);
            var successBlobStorage = Helper.GetBlobStorage("success", log);
            var failureBlobStorage = Helper.GetBlobStorage("failure", log);

            try
            {
                using (var ctx = new RecipeContext())
                {
                    #region Assign facet to recipe and recipe to context
                    foreach (var recipeElement in fullRecipeSet.Recipes.Recipe)
                    {
                        Recipe recipeDto = MapInitializer.Mapper.Map<Recipe>(recipeElement);

                        var facs = recipeDto.Facets;

                        // Null out the facets that gets mapped from the xml. 
                        recipeDto.Facets = new List<Facet>();

                        // Reassign facet from the db. Otherwise, trying to save the recipe with the 
                        // facets that was mapped from the xml will cause duplicate facts trying to insert.
                        foreach (var f in facs)
                        {
                            var dbFacet = ctx.Facet.Where(x => x.Taxonomy_id == f.Taxonomy_id && x.Name == f.Name).First();
                            recipeDto.Facets.Add(dbFacet);
                        }

                        ctx.Recipe.Add(recipeDto);
                    }
                    #endregion

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
