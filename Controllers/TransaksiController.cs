using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sandbox_Calc.Data;
using Sandbox_Calc.Model;
namespace Sandbox_Calc.Controllers;

[Route("api/transaksi")]
[ApiController]
public class TransaksiController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly IFileService _fileService;
    private readonly ILogger<TransaksiController> _logger;

    public TransaksiController(AppDbContext dbContext, IFileService fileService, ILogger<TransaksiController> logger)
    {
        _dbContext = dbContext;
        _fileService = fileService;
        _logger = logger;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromForm] TransaksiDTO _transaksi)
    {

        try
        {
            if (_transaksi.ImageFile?.Length > 1 * 1024 * 1024)
                return BadRequest("File size should not exceed 1 MB");

            string[] allowedExtensions = [".jpg", ".jpeg", ".png"];
            string createdImageName = await _fileService.SaveFileAsync(_transaksi.ImageFile, allowedExtensions);
            var data = new Transaksi
            {
                id_category = _transaksi.id_category,
                tanggal = _transaksi.tanggal,
                nominal = _transaksi.nominal,
                description = _transaksi.description,
                status = _transaksi.status,
                user_created = _transaksi.user_created,
                date_created = _transaksi.date_created,
                ImageFile = createdImageName
            };

            _dbContext.Transaksi.Add(data);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetData), new { id = data.Id }, data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating data");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, [FromForm] UpdateTransaksiDTO _transaksi)
    {
        try
        {
            if (id != _transaksi.Id)
                return BadRequest("ID mismatch");

            var existingProduct = await _dbContext.Transaksi.FindAsync(id);
            if (existingProduct == null)
                return NotFound($"Product with id: {id} not found");

            string oldImage = existingProduct.ImageFile;
            string createdImageName = "";
            if (_transaksi.ImageFile != null)
            {
                if (_transaksi.ImageFile.Length > 1 * 1024 * 1024)
                    return BadRequest("File size should not exceed 1 MB");

                string[] allowedExtensions = [".jpg", ".jpeg", ".png"];
                 createdImageName = await _fileService.SaveFileAsync(_transaksi.ImageFile, allowedExtensions);
                existingProduct.ImageFile = createdImageName;
            }
            else
            {
                existingProduct.ImageFile = oldImage;
            }
            existingProduct.tanggal = _transaksi.tanggal;
            existingProduct.nominal = _transaksi.nominal;
            existingProduct.description = _transaksi.description;   
            existingProduct.status = _transaksi.status;
            existingProduct.user_modified = _transaksi.user_modified;
            existingProduct.date_modified = _transaksi.date_modified;
            existingProduct.id_category = _transaksi.id_category;


            _dbContext.Transaksi.Update(existingProduct);
            await _dbContext.SaveChangesAsync();

            if (_transaksi.ImageFile != null)
                _fileService.DeleteFile(oldImage);

            return Ok(existingProduct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating data");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var existingProduct = await _dbContext.Transaksi.FindAsync(id);
            if (existingProduct == null)
                return NotFound($"Product with id: {id} not found");
            _fileService.DeleteFile(existingProduct.ImageFile);
            _dbContext.Transaksi.Remove(existingProduct);
            await _dbContext.SaveChangesAsync();

            

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting data");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetData(int id)
    {
        var product = await _dbContext.Transaksi.FindAsync(id);
        return product == null
            ? NotFound($"Data with id: {id} not found")
            : Ok(product);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllData([FromHeader] string username)
    {
        var products = await _dbContext.Transaksi
             .Where(t => t.user_created == username)
             .ToListAsync();

        return Ok(new { data = products });
    }
}
