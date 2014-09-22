using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daivata.Storage
{
    public class StorageUtils
    {
        public static void UploadThumbnail(string filename, System.IO.Stream filecontent){
            StorageCredentials scred = new StorageCredentials("daivata", "yEq6pYjfEoFwH8LMKXQVioSZhgkdqBXTISL0cLindFYE9YVSGLmByke1RA87P15V6XAmo740Jyoq99HXiV6eDQ==");

            CloudStorageAccount storageAccount = new CloudStorageAccount(scred, false);

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference("gallery");
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);
            blockBlob.UploadFromStream(filecontent);
        }

    }
}
