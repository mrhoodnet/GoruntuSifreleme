using System;
using System.IO;
using System.Security.Cryptography;

public class ImageEncryptionApp
{
    private const string Key = "16CHARACTERKEY"; 
    private const string Iv = "16BYTEINITVECTOR";

    public static void Main()
    {
        string imagePath = "path/to/image.jpg";
        string encryptedImagePath = "path/to/encrypted_image.bin";

        Console.WriteLine("1. Görüntüyü Şifrele");
        Console.WriteLine("2. Şifreli Görüntüyü Geri Dönüştür");
        Console.Write("Seçiminizi yapın (1 veya 2): ");
        int choice = int.Parse(Console.ReadLine());

        switch (choice)
        {
            case 1:
                EncryptImage(imagePath, encryptedImagePath);
                Console.WriteLine("Görüntü başarıyla şifrelendi.");
                break;
            case 2:
                DecryptImage(encryptedImagePath, imagePath);
                Console.WriteLine("Şifreli görüntü başarıyla geri dönüştürüldü.");
                break;
            default:
                Console.WriteLine("Geçersiz seçim.");
                break;
        }
    }

    private static void EncryptImage(string imagePath, string encryptedImagePath)
    {
        byte[] keyBytes = Convert.FromBase64String(Key);
        byte[] ivBytes = Convert.FromBase64String(Iv);

        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.IV = ivBytes;

            using (ICryptoTransform encryptor = aes.CreateEncryptor())
            {
                using (FileStream inputFileStream = new FileStream(imagePath, FileMode.Open))
                {
                    using (FileStream outputFileStream = new FileStream(encryptedImagePath, FileMode.Create))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(outputFileStream, encryptor, CryptoStreamMode.Write))
                        {
                            inputFileStream.CopyTo(cryptoStream);
                        }
                    }
                }
            }
        }
    }

    private static void DecryptImage(string encryptedImagePath, string imagePath)
    {
        byte[] keyBytes = Convert.FromBase64String(Key);
        byte[] ivBytes = Convert.FromBase64String(Iv);

        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.IV = ivBytes;

            using (ICryptoTransform decryptor = aes.CreateDecryptor())
            {
                using (FileStream inputFileStream = new FileStream(encryptedImagePath, FileMode.Open))
                {
                    using (FileStream outputFileStream = new FileStream(imagePath, FileMode.Create))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(inputFileStream, decryptor, CryptoStreamMode.Read))
                        {
                            cryptoStream.CopyTo(outputFileStream);
                        }
                    }
                }
            }
        }
    }
}
