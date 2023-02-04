using JWTManager.Models;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace JWTManager
{
    public class JwtManager : IJwtManager
    {
        private JwtHeader _jwtHeader;
        private JwtPayload _jwtPayload;
        private readonly X509Certificate2 _certificate;

        public JwtManager(X509Certificate2 certificate)
        {
            _certificate = certificate;
        }

        // Generate a new jwt token.
        public string GenerateJwt(string username)
        {
            // Cryptographic algorithm used for signature is RSA signature with SHA-256(RS256).
            _jwtHeader = new JwtHeader() { Algorithm = "RS256", Type = "JWT" };
            string jwtHeaderJson = JsonConvert.SerializeObject(_jwtHeader);
            string base64JwtHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes(jwtHeaderJson));

            _jwtPayload = new JwtPayload() { Username = username };
            string jwtPayloadJson = JsonConvert.SerializeObject(_jwtPayload);
            string base64JwtPayload = Convert.ToBase64String(Encoding.UTF8.GetBytes(jwtPayloadJson));

            byte[] dataToSign = Encoding.ASCII.GetBytes(base64JwtHeader + "." + base64JwtPayload);

            HashAlgorithmName algorithm = HashAlgorithmName.SHA256;
            RSASignaturePadding padding = RSASignaturePadding.Pkcs1;

            byte[] signature = _certificate.GetRSAPrivateKey()!.SignData(dataToSign, algorithm, padding);
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
        public JwtManager DeserializeToken(string token) { return null; }
    }
}
