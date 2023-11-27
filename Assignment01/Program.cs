using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

Stopwatch stopwatch = new Stopwatch();
var writer = File.Open("generated_password.csv", FileMode.CreateNew);
writer.Write(Encoding.UTF8.GetBytes("学号,密码\n"));
string[] lines = File.ReadAllLines("input.txt");
stopwatch.Start();

int n = 0;
foreach (var line in lines)
{
    string[] parts = line.Split(" ");
    string newLine = $"{parts[1]},{GenerateEncryptedPassword(parts[1])}\n";
    writer.Write(Encoding.UTF8.GetBytes(newLine));

    if (n % 500 == 0)
    {
        writer.Flush();
        Console.WriteLine($"已处理{n}条数据，耗时：{stopwatch.Elapsed.TotalSeconds}s");
    }
    n++;
}
writer.Flush();
writer.Close();


/// <summary>
/// 生成加密后的密码
/// </summary>
/// <param name="originalPassword">原始密码</param>
/// <returns>加密后的密码</returns>
static string GenerateEncryptedPassword(string originalPassword)
{
    //生成采用PBKDF2算法加密后的64字节密码+64字节盐值的Base64字符串
    byte[] salt = RandomNumberGenerator.GetBytes(64);
    byte[] hashedPassword = KeyDerivation.Pbkdf2(
        password: originalPassword,
        salt: salt,
        prf: KeyDerivationPrf.HMACSHA256,
        iterationCount: 10000,
        numBytesRequested: 64);
    string password = Convert.ToBase64String(hashedPassword.Concat(salt).ToArray());
    return password;
}