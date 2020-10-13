using RSA_Cryptography.Services;
using System;
using System.Windows.Forms;

namespace RSA_Cryptography
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBoxPublicKey.Text = "C:/Temp/PublicKey.xml";
            textBoxPrivateKey.Text = "C:/Temp/PrivateKey.xml";
        }

        private void buttonGenerateKeys_Click(object sender, EventArgs e)
        {
            try
            {
                var publicKeyPath = textBoxPublicKey.Text;
                var privateKeyPath = textBoxPrivateKey.Text;
                new RSACrypto().SalvaChavesRSA(publicKeyPath, privateKeyPath);
            }
            catch (Exception)
            {
                MessageBox.Show("Failed generating keys");
            }
        }

        private void buttonEncrypta_Click(object sender, EventArgs e)
        {
            try
            {
                var encryptedText = new RSACrypto().EncryptaData(textBoxText.Text, textBoxPublicKey.Text);
                textBoxText.Text = encryptedText;
            }
            catch (Exception)
            {
                MessageBox.Show("Failed encrypting");
            }
        }

        private void buttonDecrypta_Click(object sender, EventArgs e)
        {
            try
            {
                var encryptedText = new RSACrypto().DecryptaData(textBoxText.Text, textBoxPrivateKey.Text);
                textBoxText.Text = encryptedText;
            }
            catch (Exception)
            {
                MessageBox.Show("Failed decrypting");
            }
        }
    }
}
