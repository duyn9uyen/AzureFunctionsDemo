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
using DataAccess.XmlClasses;
using DataAccess.AutoMapperConfig;
using DataAccess.POCOs;
using System.Collections.Generic;

namespace FunctionV1
{
    public static class VsBlobTriggerV1
    {
        static VsBlobTriggerV1()
        {
            MapInitializer.Activate();
        }

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

            
            using (var ctx = new RecipeContext())
            {
                #region Save Taxonomies
                foreach (var taxElement in fullRecipeSet.TaxonomyTypes.Taxonomy)
                {
                    if (ctx.Taxonomy != null)
                    {
                        var taxes = ctx.Taxonomy.ToList();
                        if (taxes.Count <= 0 || !ctx.Taxonomy.Any(x => x.Name == taxElement.Name))
                        {
                            var taxdto = MapInitializer.Mapper.Map<Taxonomy>(taxElement);
                            ctx.Taxonomy.Add(taxdto);
                        }
                    }
                }
                ctx.SaveChanges();
                #endregion

                #region Get all unique facets
                // Get all unique facets from all the recipes. This will be used later when we assign facets to recipes.
                var uniqueFacets = new List<Facet>();
                foreach (var recipeElement in fullRecipeSet.Recipes.Recipe)
                {
                    foreach (var f in recipeElement.Facet)
                    {
                        if (!uniqueFacets.Any(x =>
                                            x.Taxonomy_id.ToString() == f.Taxonomy_id
                                            && x.Name == f.Name))
                        {
                            var fac = MapInitializer.Mapper.Map<Facet>(f);
                            uniqueFacets.Add(fac);
                        }
                    }

                    ctx.Facet.AddRange(uniqueFacets);
                }
                ctx.SaveChanges();
                #endregion
            }


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
