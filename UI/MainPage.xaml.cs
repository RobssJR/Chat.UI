using Core.Infra.Models.Client;
using UI.Pages;
using UI.Services;
using UI.Singleton;

namespace UI
{
    public partial class MainPage : ContentPage
    {
        private Manager _myManager;
        private ClientService _clientService;

        public MainPage()
        {
            try
            {
                InitializeComponent();
                _myManager = Manager.GetInstance();
                _clientService = new ClientService();
                _clientService.Start();
            } catch (Exception ex)
            {
            }
        }

        private async void btnConectar_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbLogin.Text) || string.IsNullOrEmpty(tbPassword.Text))
                    throw new Exception("Preencha todos os campos");
                
                _myManager.clientModel = new ClientModel() 
                { 
                   Name = tbLogin.Text,
                   Password = tbPassword.Text,
                };

                await Navigation.PushAsync(new ChatPage());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", ex.Message, "OK");
                return;
            }
        }
    }

}


