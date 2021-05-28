using Saugumas5Darbas;
using SimpleTCP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        SimpleTcpClient client;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            btnConnect.Enabled = false;
            client.Connect(txtHost.Text, Convert.ToInt32(txtPort.Text));
        }
        //WHEN FORM JUST LOADS
        private void Form1_Load(object sender, EventArgs e)
        {
            //creating new tcp client
            client = new SimpleTcpClient();
            client.StringEncoder = Encoding.UTF8;
            client.DataReceived += Client_DataReceived;

        }

        private void Client_DataReceived(object sender, SimpleTCP.Message e)
        {
            txtStatus.Invoke((MethodInvoker)delegate ()
            {
                //set txtStatus(field in form) equal to siple tcp message
                txtStatus.Text += e.MessageString;
            });
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            //write line and get reply,
            var document = Encoding.UTF8.GetBytes("Document to Sign");
            byte[] hashedDocument;

            using (var sha256 = SHA256.Create())
            {
                hashedDocument = sha256.ComputeHash(document);
            }

            var digitalSignature = new DigitalSignature();
            digitalSignature.AssignNewKey();

            var signature = digitalSignature.SignData(hashedDocument);
            string signatureString = UTF8Encoding.UTF8.GetString(signature);
            string hashedDocumentString = UTF8Encoding.UTF8.GetString(hashedDocument);
            string message = signatureString + ";" + hashedDocumentString;


            client.WriteLineAndGetReply(message, TimeSpan.FromSeconds(3));
        }
    }
}
