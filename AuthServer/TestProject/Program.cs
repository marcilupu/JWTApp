// See https://aka.ms/new-console-template for more information
using JWTManager;
using System.Security.Cryptography.X509Certificates;

Console.WriteLine("Hello, World!");
var certPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\..\cert.pfx"));

IJwtManager manager = new JwtManager(new X509Certificate2(certPath, "1234"));
string token = manager.GenerateJwt("Marcela", DateTime.Now.AddMinutes(20));

Console.WriteLine(token);
