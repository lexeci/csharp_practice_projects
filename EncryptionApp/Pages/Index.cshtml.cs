using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;

public class IndexModel : PageModel
{
    private readonly EncryptionService _encryptionService = new EncryptionService();

    [BindProperty]
    public IFormFile UploadedFile { get; set; }

    public void OnPostEncrypt()
    {
        if (UploadedFile != null)
        {
            var inputPath = Path.Combine("wwwroot", UploadedFile.FileName);
            var encryptedPath = Path.Combine("wwwroot", "encrypted_" + UploadedFile.FileName);

            using (var stream = new FileStream(inputPath, FileMode.Create))
            {
                UploadedFile.CopyTo(stream);
            }

            _encryptionService.EncryptFile(inputPath, encryptedPath);
        }
    }

    public void OnPostDecrypt()
    {
        if (UploadedFile != null)
        {
            var encryptedPath = Path.Combine("wwwroot", UploadedFile.FileName);
            var decryptedPath = Path.Combine("wwwroot", "decrypted_" + UploadedFile.FileName);

            using (var stream = new FileStream(encryptedPath, FileMode.Create))
            {
                UploadedFile.CopyTo(stream);
            }
            _encryptionService.DecryptFile(encryptedPath, decryptedPath);
        }
    }
}