using Core.Enums;
using Core.Instances;
using Core.Models;
using Core.Models.Client;
using Core.Services;
using SuperSimpleTcp;

namespace UI.Services
{
    public class TCPClientService
    {
        public ClientInstance _clientInstance;

        public TCPClientService()
        {
            ConfigureClientModel config = new ConfigureClientModel();
            config.dataReceivedEvent += DataReceived;
            config.dataSentEvent += DataSend;

            _clientInstance = ClientInstance.GetInstance();
            _clientInstance.Configure(config);
        }

        public void Start()
        {
            _clientInstance.Start();
        }

        public void Send<T>(TCPMessageModel<T> messageObj)
        {
            string messsageJson = Util.JsonUtil.ConvertToJson(messageObj);

            _clientInstance.client.Send(messsageJson);
        }


        static void DataReceived(object sender, DataReceivedEventArgs e)
        {
            string messageString = Util.GetStringMenssage(e.Data);
            dynamic messageTcp = Util.JsonUtil.ConvertToObject<TCPMessageModel<object>>(messageString);


            switch (messageTcp.ObjectMessage.Type)
            {
                case TypeMessage.ReceiveMessage:
                    break;
                default:
                    break;
            }
        }

        static void DataSend(object sender, DataSentEventArgs e)
        {
            Console.WriteLine("Teste");
        }
    }
}
