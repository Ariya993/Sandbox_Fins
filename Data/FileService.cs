using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Sandbox_Calc.Data
{

    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile imageFile, string[] allowedFileExtensions);
        void DeleteFile(string fileNameWithExtension);
    }

    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment environment;

        public FileService(IWebHostEnvironment _environment)
        {
            environment = _environment;
        }
        public async Task<string> SaveFileAsync(IFormFile imageFile, string[] allowedFileExtensions)
        {
            if (imageFile == null)
            {
                throw new ArgumentNullException(nameof(imageFile));
            }
            var webRootPath = environment.WebRootPath ?? @"C:\inetpub\wwwroot";
            var path = Path.Combine(webRootPath, "Uploads"); 

            // Buat folder jika belum ada
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // Validasi ekstensi
            var ext = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            if (!allowedFileExtensions.Contains(ext))
            {
                throw new ArgumentException($"Only {string.Join(", ", allowedFileExtensions)} are allowed.");
            }

            // Buat nama unik
            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(path, fileName);

            // Simpan file
            using var stream = new FileStream(filePath, FileMode.Create);
            await imageFile.CopyToAsync(stream);

            // Kembalikan path relatif (untuk digunakan sebagai URL publik)
            return $"/Uploads/{fileName}";
        }


        public void DeleteFile(string fileNameWithExtension)
        {
            if (string.IsNullOrEmpty(fileNameWithExtension))
            {
                throw new ArgumentNullException(nameof(fileNameWithExtension));
            }
            //var contentPath = environment.ContentRootPath;
            
            var webRootPath = environment.WebRootPath ?? @"C:\inetpub\wwwroot";
            //var path = Path.Combine( $"C:\\inetpub\\wwwrootUploads", fileNameWithExtension);
            fileNameWithExtension = fileNameWithExtension.TrimStart('/', '\\');
            var path = Path.Combine("C:\\inetpub\\wwwroot", fileNameWithExtension);
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Invalid file path");
            }
            File.Delete(path);
        }

    }
}