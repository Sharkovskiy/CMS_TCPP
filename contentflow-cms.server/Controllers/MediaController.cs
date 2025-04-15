using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using contentflow_cms.server.Data;

namespace contentflow_cms.server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MediaController : ControllerBase
{
    private readonly ContentflowContext _context;
    private readonly IWebHostEnvironment _environment;

    public MediaController(ContentflowContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Media>>> GetMedia()
    {
        return await _context.Media.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Media>> GetMedia(int id)
    {
        var media = await _context.Media.FindAsync(id);

        if (media == null)
        {
            return NotFound();
        }

        return media;
    }

    [HttpPost]
    public async Task<ActionResult<Media>> UploadMedia(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded");
        }

        var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var media = new Media
        {
            FileName = file.FileName,
            FileType = file.ContentType,
            FilePath = Path.Combine("uploads", uniqueFileName),
            FileSize = file.Length,
            UploadDate = DateTime.UtcNow
        };

        _context.Media.Add(media);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMedia), new { id = media.Id }, media);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMedia(int id)
    {
        var media = await _context.Media.FindAsync(id);
        if (media == null)
        {
            return NotFound();
        }

        var filePath = Path.Combine(_environment.WebRootPath, media.FilePath);
        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }

        _context.Media.Remove(media);
        await _context.SaveChangesAsync();

        return NoContent();
    }
} 