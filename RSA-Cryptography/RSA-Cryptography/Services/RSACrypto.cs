using System;
using System.IO;
using System.Security.Cryptography;

namespace RSA_Cryptography.Services
{
    public class RSACrypto
    {
        private static RSACryptoServiceProvider _rsa;

        public void UpdateRSAParameters()
        {
            try
            {
                const int PROVIDER_RSA_FULL = 1;
                const string CONTAINER_NAME = "SpiderContainer";

                CspParameters cspParams;
                cspParams = new CspParameters(PROVIDER_RSA_FULL);
                cspParams.KeyContainerName = CONTAINER_NAME;
                cspParams.Flags = CspProviderFlags.UseMachineKeyStore;
                cspParams.ProviderName = "Microsoft Strong Cryptographic Provider";
                _rsa = new RSACryptoServiceProvider(cspParams);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string EncryptaData(string text, string publicKey)
        {
            UpdateRSAParameters();
            try
            {
                using (var reader = new StreamReader(publicKey))
                {
                    string publicOnlyKeyXml = reader.ReadToEnd();
                    _rsa.FromXmlString(publicOnlyKeyXml);
                }
                _rsa.KeySize = 2048;

                byte[] clearText = System.Text.Encoding.UTF8.GetBytes(text);
                byte[] textEncrypted = _rsa.Encrypt(clearText, false);

                return Convert.ToBase64String(textEncrypted);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
     
        public string DecryptaData(string encryptedText, string privateKey)
        {
            UpdateRSAParameters();
            try
            {
                byte[] getPassword = Convert.FromBase64String(encryptedText);

                using (var reader = new StreamReader(privateKey))
                {
                    string publicPrivateKeyXml = reader.ReadToEnd();
                    _rsa.FromXmlString(publicPrivateKeyXml);
                }

                byte[] plain = _rsa.Decrypt(getPassword, false);
                return System.Text.Encoding.UTF8.GetString(plain);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public void SalvaChavesRSA(String publicKey, String privateKey)
        {
            UpdateRSAParameters();

            try
            {
                using (var writer = new StreamWriter(privateKey))
                {
                    string publicPrivateKeyXml = _rsa.ToXmlString(true);
                    writer.Write(publicPrivateKeyXml);
                }

                using (var writer = new StreamWriter(publicKey))
                {
                    string publicOnlyKeyXml = _rsa.ToXmlString(false);
                    writer.Write(publicOnlyKeyXml);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
