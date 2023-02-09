namespace JWTManager.Utils
{
    public static class Base64Url
    {
        public static string ToBase64Url(byte[] arg)
        {
            string s = Convert.ToBase64String(arg); 

            s = s.Split('=')[0]; 
            s = s.Replace('+', '-'); 
            s = s.Replace('/', '_'); 

            return s;
        }

        public static byte[] FromBase64Url(string arg)
        {
            string s = arg;
            s = s.Replace('-', '+'); 
            s = s.Replace('_', '/'); 

            switch (s.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 2: s += "=="; break; // Two pad chars
                case 3: s += "="; break; // One pad char
                default: throw new Exception("Illegal base64url string!");
            }

            return Convert.FromBase64String(s);
        }
    }
}
