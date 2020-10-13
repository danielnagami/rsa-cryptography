using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RSA_Cryptography.Services
{
    public class RSACrypto
    {
        /// <summary>
        /// Atributos da classe
        /// </summary>
        private static RSACryptoServiceProvider _rsa;

        /// <summary>
        /// Atualiza Parâmetros do RSA
        /// </summary>
        public void AtualizaParametrosRSA()
        {
            const int PROVIDER_RSA_FULL = 1;
            const string CONTAINER_NAME = "SpiderContainer";

            CspParameters cspParams;
            cspParams = new CspParameters(PROVIDER_RSA_FULL);
            cspParams.KeyContainerName = CONTAINER_NAME;
            cspParams.Flags = CspProviderFlags.UseMachineKeyStore;
            //cspParams.Flags = CspProviderFlags.CreateEphemeralKey;
            cspParams.ProviderName = "Microsoft Strong Cryptographic Provider";
            _rsa = new RSACryptoServiceProvider(cspParams);
        }
        /// <summary>
        /// Encryta Dados usando algoritimo RSA (Rivest Shamir Adleman 'Criadores do algoritmo')
        /// </summary>
        /// <param name="textoClaro">Texto Claro a ser encriptado</param>
        /// <param name="chavepublica">Endereço completo do arquivo .xml com chave publica</param>
        /// <returns>Texto Criptografado</returns>
        public string EncryptaData(string text, string publicKey)
        {
            //Atualiza Parâmetros
            AtualizaParametrosRSA();

            //Realiza Leitura da chave publica
            //StreamReader reader = new StreamReader(@"C:\temp\chavePublica.xml");
            StreamReader reader = new StreamReader(publicKey);
            string publicOnlyKeyXml = reader.ReadToEnd();
            _rsa.FromXmlString(publicOnlyKeyXml);
            reader.Close();
            _rsa.KeySize = 2048;
            //lê texto Claro, encripta para texto cifrado
            byte[] textoClarobytes = System.Text.Encoding.UTF8.GetBytes(text);
            byte[] textoCifradobytes = _rsa.Encrypt(textoClarobytes, false);

            return Convert.ToBase64String(textoCifradobytes);
        }

        /// <summary>
        /// Dencryta Dados usando algoritimo RSA (Rivest Shamir Adleman 'Criadores do algoritmo')
        /// </summary>
        /// <param name="textoCifrado">Texto Cifrado a ser dencriptado</param>
        /// <param name="chaveprivada">Endereço completo do arquivo .xml com chave privada</param>
        /// <returns>Texto Claro</returns>        
        public string DecryptaData(string textoCifrado, string chaveprivada)
        {
            AtualizaParametrosRSA();

            byte[] getpassword = Convert.FromBase64String(textoCifrado);

            //Leitura da chave Privada
            //StreamReader reader = new StreamReader(@"c:\temp\chavePrivada.xml");
            StreamReader reader = new StreamReader(chaveprivada);
            string publicPrivateKeyXml = reader.ReadToEnd();

            try
            {
                _rsa.FromXmlString(publicPrivateKeyXml);
            }
            catch (Exception e)
            {
                return e.Message;
            }

            reader.Close();

            //lê texto cifrado, decripta o texto e retorna texto claro.
            byte[] plain = _rsa.Decrypt(getpassword, false);
            return System.Text.Encoding.UTF8.GetString(plain);
        }

        /// <summary>
        /// Salva chaves geradas em diretório informado
        /// </summary>
        /// <param name="chavepublica">Endereço completo do arquivo xml a ser criado com chave pública (Ex. c:\temp\chavepublica.xml).</param>
        /// <param name="chaveprivada">Endereço completo do arquivo xml a ser criado com chave privada (Ex. c:\temp\chaveprivada.xml).</param>
        public void SalvaChavesRSA(String chavepublica, String chaveprivada)
        {
            AtualizaParametrosRSA();

            //Grava chave publica e privada do RSA
            //StreamWriter writer = new StreamWriter(@"c:\temp\chavePrivada.xml");
            StreamWriter writer = new StreamWriter(chaveprivada);
            string publicPrivateKeyXml = _rsa.ToXmlString(true);
            writer.Write(publicPrivateKeyXml);
            writer.Close();

            //Grava somente chave publica do RSA
            //writer = new StreamWriter(@"c:\temp\chavePublica.xml");
            writer = new StreamWriter(chavepublica);
            string publicOnlyKeyXml = _rsa.ToXmlString(false);
            writer.Write(publicOnlyKeyXml);
            writer.Close();
        }
    }
}
