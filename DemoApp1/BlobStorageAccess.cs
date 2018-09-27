using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using System.Xml;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using Microsoft.Azure.WebJobs.Host;

namespace DemoApp1
{
    public class BlobStorageAccess
    {
        CloudStorageAccount storageAccount;
        CloudBlobClient client;
        CloudBlobContainer container;
        string containerName;
        CloudBlockBlob blob;
        TraceWriter log;

        public BlobStorageAccess(string _connectionString, string _containerName, TraceWriter _log)
        {
            if (CloudStorageAccount.TryParse(_connectionString, out storageAccount))
            {
                client = storageAccount.CreateCloudBlobClient();
                container = client.GetContainerReference(_containerName);
                containerName = _containerName;
                log = _log;
            }
            else
            {
                log.Error("Unable to connect to storage account: " + _containerName);
            }
        }

        public void UploadXmlFile(XmlDocument doc, string filename)
        {
            blob = container.GetBlockBlobReference(filename);
            blob.Properties.ContentType = "application/xml";

            //upload data
            using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(doc.OuterXml)))
            {
                try
                {
                    log.Info("Attempting to save file " + filename + " to: " + containerName);
                    blob.UploadFromStream(stream);
                }
                catch (Exception ex)
                {
                    log.Error("Error uploading file. " + ex.Message + " " + ex.InnerException);
                };
            }
        }

        public void DeleteFile(string filename)
        {
            blob = container.GetBlockBlobReference(filename);
            blob.DeleteIfExists();
        }
    }
}
