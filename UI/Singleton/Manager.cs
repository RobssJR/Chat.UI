using Core.Infra.Models.Client;

namespace UI.Singleton
{
    public class Manager
    {
        private static Manager _instance;
        public ClientModel clientModel;
        private Manager() 
        {
        
        }

        public static Manager GetInstance()
        {
            if (_instance == null)
                _instance = new Manager();

            return _instance;
        }
    }
}
