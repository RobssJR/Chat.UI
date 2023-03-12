using Core.Infra.Models.Client;
using Core.Models.Exception;
using UI.Pages;
using UI.Services;
using UI.Singleton;

namespace UI
{
    public partial class MainPage : ContentPage
    {
        private Manager _myManager;
        private TCPClientService _clientService;

        public MainPage()
        {
            try
            {
                InitializeComponent();
                _myManager = Manager.GetInstance();
                _clientService = new TCPClientService();
                _clientService.Start();
            } catch
            { throw; }
        }

        private async void btnConectar_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbLogin.Text) || string.IsNullOrEmpty(tbPassword.Text))
                    throw new ErrorHandled("Preencha todos os campos");
                
                _myManager.clientModel = new ClientModel() 
                { 
                   Email = tbLogin.Text,
                   Password = tbPassword.Text,
                };

                await Navigation.PushAsync(new ChatPage());
            }
            catch (ErrorHandled ex)
            {
                await DisplayAlert("Alert", ex.Message, "OK");
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


