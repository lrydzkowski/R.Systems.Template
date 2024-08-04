using Microsoft.AspNetCore.Mvc;
using R.Systems.Template.Infrastructure.Azure.Models;
using R.Systems.Template.Infrastructure.Azure.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace R.Systems.Template.Api.Web.Controllers;

public class AzureStorageTestsController : ApiControllerBase
{
    private readonly IStorageAccountContainerClient _storageAccountContainerClient;

    public AzureStorageTestsController(IStorageAccountContainerClient storageAccountContainerClient)
    {
        _storageAccountContainerClient = storageAccountContainerClient;
    }

    [SwaggerOperation(Summary = "Create a new file in a Storage Account.")]
    [HttpPost("storage-account/file")]
    public async Task<IActionResult> GetAzureStorageAccountInfo(IFormFile formFile, CancellationToken cancellationToken)
    {
        IReadOnlyList<BlobDetails> blobs =
            await _storageAccountContainerClient.UploadFileAsync(formFile, cancellationToken);

        return Ok(blobs);
    }
}
