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

        // Generate new JWT token.
        public string GenerateJwt(string username, DateTime expiresAt)
        {
            // Cryptographic algorithm used for signature is RSA signature with SHA-256(RS256).
            string jwtHeaderJson = JsonConvert.SerializeObject(_jwtHeader);
            string base64JwtHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes(jwtHeaderJson));

            JwtPayload jwtPayload = new JwtPayload() { Username = username, ExpiresAt = expiresAt };
            string jwtPayloadJson = JsonConvert.SerializeObject(jwtPayload);
            string base64JwtPayload = Convert.ToBase64String(Encoding.UTF8.GetBytes(jwtPayloadJson));

            byte[] dataToSign = Encoding.ASCII.GetBytes(base64JwtHeader + "." + base64JwtPayload);

            byte[] signature = _certificate.GetRSAPrivateKey()!.SignData(dataToSign, _algorithm, _padding);
            string base64jwtSignature = Convert.ToBase64String(signature);

            string token = base64JwtHeader + "." + base64JwtPayload + "." + base64jwtSignature;

            return token;
        }

        // Verify existing JWT token
        public bool ValidateJwt(string token)
        {
            string[] tokenData = token.Split('.');
            string base64JwtHeader = tokenData[0];
            string base64JwtPayload = tokenData[1];
            string base64JwtSignature = tokenData[2];

            byte[] dataToVerify = Encoding.ASCII.GetBytes(base64JwtHeader + "." + base64JwtPayload);
            byte[] signedData = Convert.FromBase64String(base64JwtSignature);

            bool isValid = _certificate.GetRSAPublicKey()!.VerifyData(dataToVerify, signedData, _algorithm, _padding);

            // if the token isValid the payload cannot be null
            if (isValid)
            {
                string jwtPayloadJson = Encoding.UTF8.GetString(Convert.FromBase64String(base64JwtPayload));
                JwtPayload jwtPayload = (JwtPayload)JsonConvert.DeserializeObject(jwtPayloadJson, typeof(JwtPayload))!;
                return (jwtPayload.ExpiresAt > DateTime.Now) ? true : false;
            }
            else
            {
                return false;
            }
        }

        public JwtPayload GetPayload(string token)
        {
            string base64JwtPayload = token.Split('.')[1];
            string jwtPayloadJson = Encoding.UTF8.GetString(Convert.FromBase64String(base64JwtPayload));
            JwtPayload jwtPayload = (JwtPayload)JsonConvert.DeserializeObject(jwtPayloadJson, typeof(JwtPayload))!;
            return jwtPayload;
        }
    }
}