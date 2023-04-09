using Core.Infra.Models.Chat;
using Core.Infra.Models.Client;

namespace UI.Singleton
{
    public class Manager
    {
        private static Manager _instance;
        public ClientModel clientModel;
        public bool sucesso;
        public bool error;
        public List<ChatModel> Chats;
        public ChatModel chatSelecionado;

        public event RefreshEvent Refresh;
        public delegate void RefreshEvent(ChatModel chat);

        private Manager()
        {
        
        }

        public static Manager GetInstance()
        {
            if (_instance == null)
                _instance = new Manager();

            return _instance;
        }

        public void RefreshExec(ChatModel chat)
        {
            Refresh(chat);
        }
    }
}
