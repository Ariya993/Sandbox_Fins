using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sandbox_Calc.Data;
using Sandbox_Calc.Model;
 

namespace Sandbox_Calc.Controllers;

[Route("api/products")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly IFileService _fileService;
    private readonly ILogger<ProductController> _logger;

    public ProductController(AppDbContext dbContext, IFileService fileService, ILogger<ProductController> logger)
    {
        _dbContext = dbContext;
        _fileService = fileService;
        _logger = logger;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateProduct([FromForm] ProductDTO productToAdd)
    {
        return NoContent();
        //try
        //{
        //    if (productToAdd.ImageFile?.Length > 1 * 1024 * 1024)
        //        return BadRequest("File size should not exceed 1 MB");

        //    string[] allowedExtensions = [".jpg", ".jpeg", ".png"];
        //    string createdImageName = await _fileService.SaveFileAsync(productToAdd.ImageFile, allowedExtensions);

        //    var product = new Product
        //    {
        //        ImageName = productToAdd.ImageName,
        //        file = createdImageName
        //    };

        //    _dbContext.Products.Add(product);
        //    await _dbContext.SaveChangesAsync();

        //    return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        //}
        //catch (Exception ex)
        //{
        //    _logger.LogError(ex, "Error creating product");
        //    return StatusCode(500, ex.Message);
        //}
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductUpdateDTO productToUpdate)
    {
        try
        {
            return NoContent();
            //if (id != productToUpdate.Id)
            //    return BadRequest("ID mismatch");

            //var existingProduct = await _dbContext.Products.FindAsync(id);
            //if (existingProduct == null)
            //    return NotFound($"Product with id: {id} not found");

            //string oldImage = existingProduct.ProductImage;

            //if (productToUpdate.ImageFile != null)
            //{
            //    if (productToUpdate.ImageFile.Length > 1 * 1024 * 1024)
            //        return BadRequest("File size should not exceed 1 MB");

            //    string[] allowedExtensions = [".jpg", ".jpeg", ".png"];
            //    string createdImageName = await _fileService.SaveFileAsync(productToUpdate.ImageFile, allowedExtensions);
            //    productToUpdate.ProductImage = createdImageName;
            //}

            //existingProduct.ProductName = productToUpdate.ProductName;
            //existingProduct.ProductImage = productToUpdate.ProductImage;

            ////_dbContext.Products.Update(existingProduct);
            ////await _dbContext.SaveChangesAsync();

            //if (productToUpdate.ImageFile != null)
            //    _fileService.DeleteFile(oldImage);

            //return Ok(existingProduct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            //var existingProduct = await _dbContext.Products.FindAsync(id);
            //if (existingProduct == null)
            //    return NotFound($"Product with id: {id} not found");

            //_dbContext.Products.Remove(existingProduct);
            //await _dbContext.SaveChangesAsync();

           // _fileService.DeleteFile(existingProduct.file);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetProduct(int id)
    {
        return NoContent();
        //var product = await _dbContext.Products.FindAsync(id);
        //return product == null9
        //    ? NotFound($"Product with id: {id} not found")
        //    : Ok(product);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetProducts()
    {
        return NoContent();
        //  var products = await _dbContext.Products.ToListAsync();
        //return Ok(new { data = products });
    }
}
