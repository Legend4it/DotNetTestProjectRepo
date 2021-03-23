using Newtonsoft.Json;
using System;
using System.Net.Sockets;
using System.Text;

namespace TcpListnerPosAsServerDemo
{
    public class ClientHandler
    {
        private const string ISREADY = "ISREADY";
        private const string GETPAYMENT = "GETPAYMENT";
        public RequestObject RequestObject { get; set; }

        public ClientHandler()
        {
        }

        public void Proccess(TcpClient client)
        {
            var x = RequestObject;
            try
            {
                var buf = new byte[client.ReceiveBufferSize];
                client.GetStream().Read(buf, 0, buf.Length);

                if (MessageIsCheckRequest(Encoding.Default.GetString(buf, 0, buf.Length)))
                {
                    
                    client.GetStream().WriteAsync(Encoding.UTF8.GetBytes(ISREADY), 0, Encoding.UTF8.GetBytes(ISREADY).Length);
                }

                if (MessageIsPayDataRequest(Encoding.Default.GetString(buf, 0, buf.Length)))
                {
                    //Get PAYDATA and Send it back in stream write
                    //Get pay RequestObject() and write it as answer to stream
                    //Get Pay data from posen, Set pay-data as property and map it to RequestObject()
                    //For test send test data back
                    var clientRequestObject = new RequestObject();
                    clientRequestObject.Amout = "5000";
                    clientRequestObject.OrderNr = "123456789";
                    clientRequestObject.Amout = "5000";
                    //clientRequestObject = JsonConvert.DeserializeObject<RequestObject>(Encoding.Default.GetString(buf));
                    byte[] answer = GetResponseMessage(clientRequestObject);

                    client.GetStream().WriteAsync(answer, 0, answer.Length);
                }





            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private bool MessageIsPayDataRequest(string data)
        {
            if (data.Remove(data.IndexOf('\0')).ToUpper().Contains(GETPAYMENT))
            {
                return true;
            }
            return false;
        }

        private bool MessageIsCheckRequest(string data)
        {
            if (data.Remove(data.IndexOf('\0')).ToUpper().Contains(ISREADY))
            {
                //Check if paymen data available from pos
                return true;
            }
            return false;
        }

        private static byte[] GetResponseMessage(RequestObject clientRequestObject)
        {
            //var content = String.Format("<html><body><h2>Hello</h2><h2>Word</h2><div>at {0} UTC.</div></body></html>\r\n\r\n", DateTime.UtcNow);
            //var buf2 = Encoding.UTF8.GetBytes(content);
            //var answer = Encoding.UTF8.GetBytes(String.Format("HTTP/1.1 200 OK\r\nContent-Type: text/html; charset=utf-8\r\nContent-Length: {0}\r\n\r\n{1}", buf2.Length, JsonConvert.SerializeObject(clientRequestObject)));

            var answer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(clientRequestObject));
            return answer;
        }

    }
}