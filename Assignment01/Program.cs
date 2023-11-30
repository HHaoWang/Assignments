//using Microsoft.AspNetCore.Cryptography.KeyDerivation;
//using System.Diagnostics;
//using System.Security.Cryptography;
//using System.Text;

//Stopwatch stopwatch = new Stopwatch();
//var writer = File.Open("generated_passwor.csv", FileMode.CreateNew);
//writer.Write(Encoding.UTF8.GetBytes("学号,密码\n"));
//string[] lines = File.ReadAllLines("input.txt");

//stopwatch.Start();
//int n = 0;
//foreach (var line in lines)
//{
//    string[] parts = line.Split(" ");
//    string newLine = $"{parts[1]},{GenerateEncryptedPassword(parts[1])}\n";
//    writer.Write(Encoding.UTF8.GetBytes(newLine));

//    if (n % 500 == 0)
//    {
//        writer.Flush();
//        Console.WriteLine($"已处理{n}条数据，耗时：{stopwatch.Elapsed.TotalSeconds}s");
//    }
//    n++;
//}
//writer.Flush();
//writer.Close();
///// <summary>
///// 生成加密后的密码
///// </summary>
///// <param name="originalPassword">原始密码</param>
///// <returns>加密后的密码</returns>
//static string GenerateEncryptedPassword(string originalPassword)
//{
//    //生成采用PBKDF2算法加密后的64字节密码+64字节盐值的Base64字符串
//    byte[] salt = RandomNumberGenerator.GetBytes(64);
//    byte[] hashedPassword = KeyDerivation.Pbkdf2(
//        password: originalPassword,
//        salt: salt,
//        prf: KeyDerivationPrf.HMACSHA256,
//        iterationCount: 10000,
//        numBytesRequested: 64);
//    string password = Convert.ToBase64String(hashedPassword.Concat(salt).ToArray());
//    return password;




using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Collections.Concurrent;
using System.Diagnostics;
 using System.Security.Cryptography;
using System.Text;

Stopwatch stopwatch = new Stopwatch();//创建了一个Stopwatch对象，用于计算代码执行的时间
var writer = File.Open("generat.csv", FileMode.CreateNew);//创建一个文件,用于存储生成的密码
writer.Write(Encoding.UTF8.GetBytes("学号,密码\n"));//将字节写入文件
string[] lines = File.ReadAllLines("input.txt");//从文件中读取所有行，并存储在一个字符串数组lines中

stopwatch.Start();//计算代码执行的时间
List<string> newLines = new List<string>();//存储结果
int n = 0;//记录处理的数据行数
int Size = 600; // 每次处理数据量的大小

// 创建多个线程
int threadcount = Environment.ProcessorCount; // 获取当前系统的处理器核心数作为线程数
Thread[] threads = new Thread[threadcount];//创建了一个名为threads的线程数组
//使用一个循环来创建线程（线程执行fun函数）和通过Start方法启动每个线程
for (int i = 0; i < threadcount; i++)
{
    threads[i] = new Thread(start: fun);
    threads[i].Start();
}
 
// 等待所有线程完成
foreach (var thread in threads)
{
    thread.Join();//等待线程执行完成
}
// 写入文件
foreach (var newLine in newLines)
{
    byte[] lineBytes = Encoding.UTF8.GetBytes(newLine + "\n");
    writer.Write(lineBytes);
}
Console.WriteLine($"所有任务执行完成，总耗时：{stopwatch.Elapsed.TotalSeconds}s");
writer.Flush(); 
writer.Close();
 
void fun()
{
    int startIndex;//记录当前要处理数据位置
    while (true)
    {
        startIndex = Interlocked.Add(ref n, Size) - Size; // 原子操作，获取当前要处理的数据行数

        if (startIndex >= lines.Length)
            break; // 所有数据已处理完毕，退出循环

        // 处理每个批次数据
        for (int j = startIndex; j < startIndex + Size && j < lines.Length; j++)
        {
            string[] parts = lines[j].Split(" ");//使用空格分割字符串
            string newLine = $"{parts[1]},{GenerateEncryptedPassword(parts[1])}";//调用函数生成加密后的密码
            newLines.Add(newLine);//学号和密码写入文件

        }
    }
}

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

