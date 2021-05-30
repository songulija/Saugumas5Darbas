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

            // Sender
            //DigitalSignature.ContainerName = "KeyContainer";
            //string secureMessage = $"Transfer $500 into account number 029192819283 on {DateTime.Now}";
            string secureMessage = $"Transfer $500 into account number 029192819283 on {DateTime.Now}";
            byte[] digitalSignature = DigitalSignature.SignMessage(secureMessage);

            //var digitarSignatureString = Encoding.UTF8.GetString(digitalSignature);//convert to string
            //Console.WriteLine("Client Digital signature send to server is:\n" + secureMessage);
            //Console.WriteLine("Client Digital signature send to server is:\n" + digitarSignatureString);

            

            string message = Encoding.UTF8.GetString(digitalSignature) + ";" + secureMessage;

            Console.WriteLine("\n Client signature:\n" + Encoding.UTF8.GetString(digitalSignature));
            Console.WriteLine("\n Client secureMessage:\n" + secureMessage);
            Console.WriteLine("\n Client full message:\n" + message);

            client.WriteLine(message);
        }
    }
}
