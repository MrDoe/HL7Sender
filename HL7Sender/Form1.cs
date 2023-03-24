using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Text;

namespace HL7Sender
{
    public partial class Form1 : Form
    {
        private int count = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            try
            {
                string ipAdress = txtAddress.Text;
                int port = int.Parse(txtPort.Text);
                string messageText = txtMessage.Text;

                txtLog.Text += "CONNECTING\n";

                messageText = (char)11 + messageText + (char)28 + (char)13;
                var byteMessage = Encoding.UTF8.GetBytes(messageText);

                TcpClient client = new(ipAdress, port);
                NetworkStream stream = client.GetStream();

                byte[] ACK = new byte[client.ReceiveBufferSize + 1];
               

                stream.Write(byteMessage, 0, byteMessage.Length);
                stream.Read(ACK, 0, ACK.Length);
                ++count;

                txtLog.Text += "MESSAGE: " + count + "\n";

                string HL7Clean = MyRegex().Replace(messageText, string.Empty);
                txtLog.Text += HL7Clean + "\nACK\n";

                string ACKClean = Encoding.UTF8.GetString(ACK);
                ACKClean = MyRegex().Replace(ACKClean, string.Empty);
                txtLog.Text += ACKClean + "\n\n";
            }
            catch (Exception ex)
            {
                txtLog.Text += ex.Message.ToString();
            }
        }

        [GeneratedRegex("[^\\u0020-\\u007E]")]
        private static partial Regex MyRegex();
    }
}