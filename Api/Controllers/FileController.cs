using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController : ControllerBase
{
    private readonly ILogger<FileController> _logger;

    public FileController(ILogger<FileController> logger)
    {
        _logger = logger;
    }

    [HttpPost("upload")]
    public async Task<ActionResult> UploadFile([FromForm] UploadFilesModel body)
    {
        foreach (var file in body.Files)
        {
            var path = Path.Join(Directory.GetCurrentDirectory(), file.FileName);
            _logger.LogInformation("Saving file {FileName} to path {Path}...", file.FileName, path);
            await using var driveFile = System.IO.File.Create(path);
            await file.CopyToAsync(driveFile);
        }
        
        return NoContent();
    }
}

public sealed record UploadFilesModel
{
    public IFormFileCollection Files { get; set; } = null!;
}