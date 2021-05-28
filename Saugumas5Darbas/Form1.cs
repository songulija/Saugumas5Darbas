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

namespace Saugumas5Darbas
{
    public partial class Form1 : Form
    {
        SimpleTcpServer server;
        public Form1()
        {
            InitializeComponent();
}

        
        private void Form1_Load(object sender, EventArgs e)
        {
            //creating new Simple server
            server = new SimpleTcpServer();
            server.Delimiter = 0x13;//enter
            server.StringEncoder = Encoding.UTF8;
            server.DataReceived += Server_DataReceived;

        }

        private void Server_DataReceived(object sender, SimpleTCP.Message e)
        {
            txtStatus.Invoke((MethodInvoker)delegate ()
            {
                //set txtStatus(field in form) equal to message that was sent from client
                string fullMessage = e.MessageString;

                //spliting, becouse we send signature + ";" + hashedDocument
                string[] data = fullMessage.Split(';');
                string signatureString = data[0];
                string hashedDocumentString = data[1];

                Console.WriteLine("signature : " + signatureString);
                Console.WriteLine("hashedDocumentString : " + hashedDocumentString);


                byte[] signature = UTF8Encoding.UTF8.GetBytes(signatureString);
                byte[] hashedDocument = UTF8Encoding.UTF8.GetBytes(hashedDocumentString);

                //creating DigitalSignature object and calling verify Signature method
                var digitalSignature = new DigitalSignature();
                var verified = digitalSignature.VerifySignature(hashedDocument, signature);

                string result = "";

                if (verified)
                {
                    result = "The digital signature has been correctly verified.";
                }


                //txtStatus.Text += ;

                //e.ReplyLine(string.Format("You said: {0}", e.MessageString));
                e.ReplyLine(string.Format("You said: {0}", result));
            });
        }

        //START SERVER
        private void buttonbtnStart_Click(object sender, EventArgs e)
        {
            txtStatus.Text += "Server starting ...";
            System.Net.IPAddress ip = System.Net.IPAddress.Parse(txtHost.Text);
            //start server at decrared IP and PORT 
            server.Start(ip, Convert.ToInt32(txtPort.Text));

        }

        //STOP SERVER
        private void btnStop_Click(object sender, EventArgs e)
        {
            //to stop server if its started
            if (server.IsStarted)
            {
                server.Stop();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var document = Encoding.UTF8.GetBytes("Document to Sign");
            byte[] hashedDocument;

            using (var sha256 = SHA256.Create())
            {
                hashedDocument = sha256.ComputeHash(document);
            }

            var digitalSignature = new DigitalSignature();
            digitalSignature.AssignNewKey();

            var signature = digitalSignature.SignData(hashedDocument);
            var verified = digitalSignature.VerifySignature(hashedDocument, signature);

            Console.WriteLine("Digital Signature Demonstration in .NET");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("   Original Text = " + System.Text.Encoding.Default.GetString(document));
            Console.WriteLine();
            Console.WriteLine("   Digital Signature = " + Convert.ToBase64String(signature));
            Console.WriteLine();

            if (verified)
            {
                Console.WriteLine("The digital signature has been correctly verified.");
            }
            else
            {
                Console.WriteLine("The digital signature has NOT been correctly verified.");
            }

        }
    }
}
