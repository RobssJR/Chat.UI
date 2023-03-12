using Core.Enums;
using Core.Infra.Models.Client;
using Core.Instances;
using Core.Models;
using Core.Models.Client;
using Core.Models.Geral;
using Core.Services;
using SuperSimpleTcp;
using UI.Singleton;

namespace UI.Services
{
    public class TCPClientService
    {
        public ClientInstance _clientInstance;
        private static Manager _myManager;
        private static TCPClientService _instance;

        public TCPClientService()
        {
            _myManager = Manager.GetInstance();

            ConfigureClientModel config = new ConfigureClientModel();
            config.dataReceivedEvent += DataReceived;
            config.dataSentEvent += DataSend;

            _clientInstance = ClientInstance.GetInstance();
            _clientInstance.Configure(config);
        }

        public static TCPClientService GetInstance()
        {
            if (_instance == null)
                _instance = new TCPClientService();

            return _instance;
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

            switch (messageTcp.Type)
            {
                case TypeMessage.ReceiveMessage:
                    break;
                case TypeMessage.Register:
                    messageTcp = Util.JsonUtil.ConvertToObject<TCPMessageModel<SuccessModel>>(messageString);
                    break;
                case TypeMessage.Login:
                    messageTcp = Util.JsonUtil.ConvertToObject<TCPMessageModel<ClientModel>>(messageString);

                    _myManager.clientModel = messageTcp.Message;
                    _myManager.logado = true;
                    break;
                case TypeMessage.Erro:
                    _myManager.error = true;
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
