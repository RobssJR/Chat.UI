using Core.Infra.Models.Client;
using Core.Instances;
using Core.Models;
using Core.Models.Exception;
using Core.Services;
using UI.Services;
using UI.Singleton;

namespace UI.Pages;

public partial class RegisterPage : ContentPage
{
	private Manager _myManager;
	private ClientInstance _clientService;
	private bool response;
	public RegisterPage()
	{
		InitializeComponent();
		_myManager = Manager.GetInstance();
        _clientService = ClientInstance.GetInstance();

    }

    private async void btnRegistrar_Clicked(object sender, EventArgs e)
    {
		try
		{
			ValidateFields();

			TCPMessageModel<ClientModel> messageObj = new TCPMessageModel<ClientModel>()
			{
				Type = Core.Enums.TypeMessage.Register,
				Message = new ClientModel()
				{
					Name = tbUserName.Text,
					Email = tbLogin.Text,
					Password = tbSenha.Text,
				},
				Time = DateTime.Now,
			};

			_clientService.Send(messageObj);

        } catch (ErrorHandled ex)
		{
            await DisplayAlert("", ex.Message, "OK");
        }
    }

	private void ValidateFields()
	{
		try
		{
			bool fieldsEmpty = (
				string.IsNullOrEmpty(tbLogin.Text) ||
                string.IsNullOrEmpty(tbUserName.Text) ||
                string.IsNullOrEmpty(tbSenha.Text) ||
                string.IsNullOrEmpty(tbSenhaConfirmar.Text));

			if (fieldsEmpty)
				throw new ErrorHandled("Preencha todos os campos");

            bool emailValidate = (
				tbLogin.Text.Contains('@'));

            if (fieldsEmpty)
                throw new ErrorHandled("Email não valido");

            bool passwordEqual = (tbSenha.Text == tbSenhaConfirmar.Text);

            if (passwordEqual == false)
                throw new ErrorHandled("Senhas divergem");
        }
		catch 
		{
			throw;
		}
	}
}