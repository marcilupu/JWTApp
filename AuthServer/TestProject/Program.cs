// See https://aka.ms/new-console-template for more information
using JWTManager;
using System.Security.Cryptography.X509Certificates;

Console.WriteLine("Hello, World!");

IJwtManager manager= new JwtManager(new X509Certificate2("cert.pfx", "1234"));
string token = manager.GenerateJwt("Marcela");

Console.WriteLine(token);
