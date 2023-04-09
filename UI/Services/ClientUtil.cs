using Core.Models.Exception;
using UI.Singleton;

namespace UI.Services
{
    public class ClientUtil
    {
        private static Manager _myManager = Manager.GetInstance();

        public static async Task<bool> AwaitResponse()
        {
            try
            {
                while (_myManager.sucesso == false && _myManager.error == false) { }

                if (_myManager.error)
                    throw new ErrorHandled("Erro");

                _myManager.error = false;
                _myManager.sucesso = false;
                return true;
            }
            catch (ErrorHandled ex)
            {
                _myManager.error = false;
                _myManager.sucesso = false;
                return false;
            }

        }
    }
}
