using Core.Enums;
using Core.Infra.Models.Chat;
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
    public class TCPClientConfig
    {
        public ClientInstance _clientInstance;
        private Manager _myManager;

        public TCPClientConfig()
        {
            _myManager = Manager.GetInstance();

            ConfigureClientModel config = new ConfigureClientModel();
            config.dataReceivedEvent += DataReceived;
            config.dataSentEvent += DataSend;

            _clientInstance = ClientInstance.GetInstance();
            _clientInstance.Configure(config);
        }

        private void DataReceived(object sender, DataReceivedEventArgs e)
        {
            string messageString = Util.GetStringMenssage(e.Data);
            dynamic messageTcp = Util.JsonUtil.ConvertToObject<TCPMessageModel<object>>(messageString);

            switch (messageTcp.Type)
            {
                case TypeMessage.ReceiveMessage:
                    break;
                case TypeMessage.Register:
                    messageTcp = Util.JsonUtil.ConvertToObject<TCPMessageModel<SuccessModel>>(messageString);
                    _myManager.sucesso = true;
                    break;
                case TypeMessage.Login:
                    messageTcp = Util.JsonUtil.ConvertToObject<TCPMessageModel<ClientModel>>(messageString);
                    _myManager.clientModel = messageTcp.Message;
                    _myManager.sucesso = true;
                    break;
                case TypeMessage.Erro:
                    _myManager.error = true;
                    break;
                case TypeMessage.GetChats:
                    messageTcp = Util.JsonUtil.ConvertToObject<TCPMessageModel<List<ChatModel>>>(messageString);
                    _myManager.Chats = messageTcp.Message;
                    _myManager.sucesso = true;
                    break;
                case TypeMessage.RefreshChat:
                    messageTcp = Util.JsonUtil.ConvertToObject<TCPMessageModel<ChatModel>>(messageString);
                    _myManager.RefreshExec(messageTcp.Message);
                    break;
                case TypeMessage.GetChat:
                    messageTcp = Util.JsonUtil.ConvertToObject<TCPMessageModel<ChatModel>>(messageString);
                    _myManager.chatSelecionado = messageTcp.Message;
                    _myManager.sucesso = true;
                    break;
                case TypeMessage.UpdateChat:
                    messageTcp = Util.JsonUtil.ConvertToObject<TCPMessageModel<ChatModel>>(messageString);
                    _myManager.RefreshExec(_myManager.chatSelecionado);
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
