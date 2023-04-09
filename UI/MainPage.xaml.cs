using Core.Infra.Models.Client;
using Core.Instances;
using Core.Models;
using Core.Models.Exception;
using UI.Pages;
using UI.Services;
using UI.Singleton;

namespace UI
{
    public partial class MainPage : ContentPage
    {
        private Manager _myManager;
        private ClientInstance _clientService;

        public MainPage()
        {
            try
            {
                InitializeComponent();
                _myManager = Manager.GetInstance();
                _clientService = ClientInstance.GetInstance();
                _clientService.Start();
                tbLogin.Text = "adm@gmail.com";
                tbPassword.Text = "adm";
            } catch
            { throw; }
        }

        private async void btnConectar_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbLogin.Text) || string.IsNullOrEmpty(tbPassword.Text))
                    throw new ErrorHandled("Preencha todos os campos");

                ClientModel clientLogin = new ClientModel()
                {
                    Email = tbLogin.Text,
                    Password = tbPassword.Text,
                };

                TCPMessageModel<ClientModel> messageObj = new TCPMessageModel<ClientModel>()
                {
                    Type = Core.Enums.TypeMessage.Login,
                    Message = clientLogin,
                    Time = DateTime.Now,
                };

                _clientService.Send(messageObj);

                bool result = await ClientUtil.AwaitResponse();

                if (result == false)
                    throw new ErrorHandled("Email e senha estão incorretos");

                await Navigation.PushAsync(new ChatPage());
            }
            catch (ErrorHandled ex)
            {
                await DisplayAlert("", ex.Message, "OK");
                return;
            }
        }

        private async void btnRegistrar_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new RegisterPage());
            }
            catch (ErrorHandled ex)
            {
                await DisplayAlert("", ex.Message, "OK");
                return;
            }
        }
    }

}


