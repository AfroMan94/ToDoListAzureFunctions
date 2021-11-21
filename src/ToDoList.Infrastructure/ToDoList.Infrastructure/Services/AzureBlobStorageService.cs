using Microsoft.AspNetCore.Http;
using Storage.Net.Blobs;
using System;
using System.IO;
using System.Threading.Tasks;
using ToDoList.Core.Interfaces;

namespace ToDoList.Infrastructure.Services
{
    /// <summary>
    ///     Azure Blob Storage
    /// </summary>
    public class AzureBlobStorageService : IStorageService
    {
        private readonly IBlobStorage _blobStorage;

        public AzureBlobStorageService(IBlobStorage blobStorage)
        {
            _blobStorage = blobStorage ?? throw new ArgumentNullException(nameof(blobStorage));
        }

        public async Task<string> UploadFile(IFormFile file, string fullPath)
        {
            await using Stream str = file.OpenReadStream();
            await _blobStorage.WriteAsync(fullPath, str);

            return fullPath;
        }

        public async Task<Stream> GetFileStream(string filePath)
        {
            MemoryStream ms = new MemoryStream();
            await _blobStorage.ReadToStreamAsync(filePath, ms);
            return ms;
        }

    }
}
