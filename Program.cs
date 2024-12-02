
//Symmetric


using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;


string key = "b14ca5898a4e4133bbce2ea2315a1916";
string iv = "1234567890123457";
string plainText = "Hello World";

var encrypted = SymmetricEncryption.Encrypt(plainText, key, iv);

var decrypted = SymmetricEncryption.Decrypt(encrypted, key, iv);

Console.WriteLine(encrypted);
Console.WriteLine(decrypted);


public class SymmetricEncryption
{
    public static string Encrypt(string plainText, string key, string iv)
    {
        var aes = System.Security.Cryptography.Aes.Create();
        aes.Key = System.Text.Encoding.UTF8.GetBytes(key);
        aes.IV = System.Text.Encoding.UTF8.GetBytes(iv);

        MemoryStream memoryStream = new MemoryStream();
        CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);

        StreamWriter writer = new StreamWriter(cryptoStream);
        writer.Flush();
        cryptoStream.FlushFinalBlock();
        writer.Write(plainText);

        return Convert.ToBase64String(memoryStream.ToArray());
    }

    public static string Decrypt(string encryptedText, string key, string iv)
    {
        var aes = System.Security.Cryptography.Aes.Create();
        aes.Key = System.Text.Encoding.UTF8.GetBytes(key);
        aes.IV = System.Text.Encoding.UTF8.GetBytes(iv);

        byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        using (MemoryStream memoryStream = new MemoryStream(encryptedBytes))
        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
        using (StreamReader reader = new StreamReader(cryptoStream))
        {
            string decryptedText = reader.ReadToEnd();
            return decryptedText;
        }
    }

}