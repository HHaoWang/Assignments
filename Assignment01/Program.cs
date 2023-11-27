/*using Microsoft.AspNetCore.Cryptography.KeyDerivation;
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
}*/



using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

class Program
{
    static void Main()
    {
        Stopwatch Time = new Stopwatch();

        // 创建一个用于写入的文件流
        var writer = File.Open("generated_password.csv", FileMode.CreateNew);

        // 写入文件标题（UTF-8格式）
        writer.Write(Encoding.UTF8.GetBytes("学号,密码\n"));

        // 读取所有行数据到字符串数组
        string[] lines = File.ReadAllLines("input.txt");

        Time.Start();

        int Batchprocessing = 300;

        // 并行处理数据
        Parallel.For(0, lines.Length / Batchprocessing, (i) =>
        {
            // 处理当前批次的数据
            ProcessBatch(lines.Skip(i * Batchprocessing).Take(Batchprocessing).ToArray(), writer);
        });

        writer.Flush();

        writer.Close();

        Console.WriteLine($"已处理{lines.Length}条数据，耗时：{Time.Elapsed.TotalSeconds}s");
    }

    // 处理批次数据
    static void ProcessBatch(string[] batch, FileStream writer)
    {
        // 遍历处理当前批次的每一行数据
        foreach (var line in batch)
        {
            // 拆分行数据
            string[] parts = line.Split(" ");

            // 生成加密密码
            string newLine = $"{parts[1]},{GenerateEncryptedPassword(parts[1])}\n";

            // 写入数据到文件
            lock (writer)
            {
                writer.Write(Encoding.UTF8.GetBytes(newLine));
            }
        }
    }

    static string GenerateEncryptedPassword(string originalPassword)
    {
        // 生成随机盐值
        byte[] salt = RandomNumberGenerator.GetBytes(64);

        // 生成采用PBKDF2算法加密后的64字节密码+64字节盐值的Base64字符串
        byte[] hashedPassword = KeyDerivation.Pbkdf2(
            password: originalPassword,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 64);

        // 拼接转换为Base64字符串
        string password = Convert.ToBase64String(hashedPassword.Concat(salt).ToArray());
        return password;
    }
}
