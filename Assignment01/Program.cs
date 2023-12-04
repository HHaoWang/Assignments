using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

Stopwatch stopwatch = new();
FileStream writer = File.Open("generated_password.csv", FileMode.Create);
writer.Write("学号,密码\n"u8);
string[] lines = File.ReadAllLines("input.txt");

string[][] chunks = lines.Chunk(1000).ToArray();

stopwatch.Start();

int threadCount = 0;
int i = 0;

while (true)
{
    if (threadCount >= 8)
    {
        Thread.Sleep(500);
        continue;
    }

    if (i >= chunks.Length)
    {
        break;
    }

    Thread thread = new(Generate);
    thread.Start(chunks[i]);
    i++;
    threadCount++;
}

void Generate(object? partialLinesObject)
{
    string[] partialLines = (string[])partialLinesObject!;
    foreach (var line in partialLines)
    {
        string[] parts = line.Split(" ");
        string newLine = $"{parts[1]},{GenerateEncryptedPassword(parts[1])}\n";

        lock (writer)
        {
            writer.Write(Encoding.UTF8.GetBytes(newLine));
        }
    }

    writer.Flush();
    Console.WriteLine($"已处理{partialLines.Length}条数据，当前时间：{stopwatch.Elapsed.TotalSeconds}s");

    threadCount--;
}

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