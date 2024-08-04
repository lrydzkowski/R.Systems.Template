using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using R.Systems.Template.Infrastructure.Azure.Models;

namespace R.Systems.Template.Infrastructure.Azure.Services;

public interface IStorageAccountContainerClient
{
    Task<IReadOnlyList<BlobDetails>> UploadFileAsync(IFormFile file, CancellationToken cancellationToken);
}

internal class StorageAccountContainerClient
    : IStorageAccountContainerClient
{
    private readonly BlobServiceClient _blobServiceClient;

    public StorageAccountContainerClient(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<IReadOnlyList<BlobDetails>> UploadFileAsync(IFormFile file, CancellationToken cancellationToken)
    {
        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient("test");
        string fileName = $"test-file-{Guid.NewGuid().ToString()}";
        BlobClient blobClient = containerClient.GetBlobClient(fileName);
        await using Stream stream = file.OpenReadStream();
        await blobClient.UploadAsync(
            stream,
            new BlobHttpHeaders { ContentType = file.ContentType },
            cancellationToken: cancellationToken
        );

        List<BlobDetails> blobs = [];
        await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
        {
            blobs.Add(new BlobDetails { Name = blobItem.Name });
        }

        return blobs;
    }
}
