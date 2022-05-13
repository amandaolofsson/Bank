using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    enum ServerMessageEnum {Response, Text};

    class ClientDisconnectException : Exception { }

    class Communication
    {
        Socket socket;

        public Communication(Socket socket)
        {
            this.socket = socket;
        }
        
        //In case server only sends text, doesn't expect response back
        public void SendText(string format, params object[] args)
        {
            Send(String.Format(format, args), ServerMessageEnum.Text);
        }

        //To being able to send meny as formated
        public void Send(string format, params object[] args)
        {
            Send(String.Format(format, args));
        }

        //Sends message and Enum type to Client
        public void Send(string message, ServerMessageEnum type = ServerMessageEnum.Response, bool newLine = true)
        {
            string responseType = "R";

            switch (type)
            {
                case ServerMessageEnum.Response:
                    responseType = "R";
                    break;
                case ServerMessageEnum.Text:
                    responseType = "T";
                    break;
            }

            //Adds Enum to message string
            message = responseType + message;

            if (newLine)
            {
                message += "\r\n";
            }

            //Adds EOM-symbol
            message += "¤";

            Byte[] bSend = System.Text.Encoding.UTF8.GetBytes(message);
            socket.Send(bSend);
        }

        public string Receive()
        {
            try
            {
                byte[] bRead = new byte[256];
                int bReadSize = socket.Receive(bRead);
                return System.Text.Encoding.UTF8.GetString(bRead, 0, bReadSize);
            }
            catch (SocketException)
            {
                throw new ClientDisconnectException();
            }
        }

        //Get input from user and only return valid entries
        //Expected format array or list of string
        public string Get(ICollection<string> expected)
        {
            while (true)
            {
                string input = this.Receive();

                //Check input against every string in expected
                foreach (var es in expected)
                {
                    if (input == es)
                    {
                        //Yes, this was an expected input
                        return input;
                    }
                }
                Send("Invalid input. Try again");
            }
        }

        //Ask the user for input
        public string Prompt(string text)
        {
            Send(text, ServerMessageEnum.Response, false);

            string value = Receive();

            return value;
        }

        //Converts string to int
        public int PromptInt(string text)
        {
            while (true)
            {
                string s = Prompt(text);

                try
                {
                    return int.Parse(s);
                }
                catch
                {
                    SendText("Invalid input. Try again");
                }
            }
        }
    }
}
