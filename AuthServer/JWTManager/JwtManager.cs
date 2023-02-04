using JWTManager.Models;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace JWTManager
{
    public class JwtManager : IJwtManager
    {
        private readonly X509Certificate2 _certificate;
        private readonly HashAlgorithmName _algorithm;
        private readonly RSASignaturePadding _padding;
        private readonly JwtHeader _jwtHeader;

        public JwtManager(X509Certificate2 certificate)
        {
            _certificate = certificate;
            _algorithm = HashAlgorithmName.SHA256;
            _padding = RSASignaturePadding.Pkcs1;
            _jwtHeader = new JwtHeader() { Algorithm = "RS256", Type = "JWT" };
        }

        // Generate a new jwt token.
        public string GenerateJwt(string username)
        {
            // Cryptographic algorithm used for signature is RSA signature with SHA-256(RS256).
            string jwtHeaderJson = JsonConvert.SerializeObject(_jwtHeader);
            string base64JwtHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes(jwtHeaderJson));

            JwtPayload jwtPayload = new JwtPayload() { Username = username };
            string jwtPayloadJson = JsonConvert.SerializeObject(jwtPayload);
            string base64JwtPayload = Convert.ToBase64String(Encoding.UTF8.GetBytes(jwtPayloadJson));

            byte[] dataToSign = Encoding.ASCII.GetBytes(base64JwtHeader + "." + base64JwtPayload);

            byte[] signature = _certificate.GetRSAPrivateKey()!.SignData(dataToSign, _algorithm, _padding);
            string base64jwtSignature = Convert.ToBase64String(signature);

            string token = base64JwtHeader + "." + base64JwtPayload + "." + base64jwtSignature;

            return token;
        }

        // Denissa
        public bool ValidateJwt(string token)
        {
            // Ca sa verifici semnatura
            //bool isValid = cert.GetRSAPublicKey()!.VerifyData(dataToSign, signature, algorithmUsed, paddingUsed);
            return true;
        }

        // Serialize the jwt token.
        public JwtPayload GetPayload(string token) { return null; }
    }
}
