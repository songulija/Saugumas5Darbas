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

                //string fullMessage = e.MessageString;//gauname
                //Console.WriteLine("Server received message: " + fullMessage);
                char[] MyChar = { '' };
                string fullMessage = e.MessageString;//gauname
                fullMessage = fullMessage.TrimEnd(MyChar);

                string[] data = fullMessage.Split(';');//split to get signature ; secureMessage
                string signatureString = data[0];
                string secureMessage = data[1];



                Console.WriteLine("\n Server full message received:\n" + fullMessage);
                Console.WriteLine("\n Server signature received:\n" + signatureString);
                Console.WriteLine("\n Server secureMessage received:\n" + secureMessage);

                // Recipient
                //get bytes of secureMessage
                byte[] messageHash = secureMessage.ComputeMessageHash();
                Console.WriteLine("\n Server hash: " + Encoding.UTF8.GetString(messageHash));


                //convert string to bytes
                byte[] digitalSignature = Encoding.UTF8.GetBytes(signatureString);
                

                if (DigitalSignature.VerifySignedMessage(messageHash, digitalSignature) == true)
                {
                    Console.WriteLine($"Message '{secureMessage}' is valid and can be trusted.");
                    txtStatus.Text = secureMessage;
                }
                else
                {
                    Console.WriteLine($"The following message: '{secureMessage}' is not valid. DO NOT TRUST THIS MESSAGE!");
                    txtStatus.Text = secureMessage;
                }
                
                //e.ReplyLine(string.Format("You said: {0}", e.MessageString));
                //e.ReplyLine(string.Format("You said: {0}", fullMessage));

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
            

            DigitalSignature.ContainerName = "KeyContainer";

            // Sender

            string secureMessage = $"Transfer $500 into account number 029192819283 on {DateTime.Now}";

            byte[] digitalSignature = DigitalSignature.SignMessage(secureMessage);

            // Message intercepted

            //secureMessage = $"Transfer $5000 into account number 849351278435 on {DateTime.Now}";

            // Recipient
            //get bytes of secureMessage
            byte[] messageHash = secureMessage.ComputeMessageHash();

            if (DigitalSignature.VerifySignedMessage(messageHash, digitalSignature))
            {
                Console.WriteLine($"Message '{secureMessage}' is valid and can be trusted.");
            }
            else
            {
                Console.WriteLine($"The following message: '{secureMessage}' is not valid. DO NOT TRUST THIS MESSAGE!");
            }
                


        }
    }
}
