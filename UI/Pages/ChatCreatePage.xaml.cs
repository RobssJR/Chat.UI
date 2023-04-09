
using Core.Enums;
using Core.Infra.Models.Chat;
using Core.Infra.Models.Client;
using Core.Instances;
using Core.Models;
using Core.Models.Exception;
using Core.Services;
using UI.Services;
using UI.Singleton;

namespace UI.Pages;

public partial class ChatCreatePage : ContentPage
{
	private Manager _myManager;
	private ClientInstance _clientService;

    public ChatCreatePage()
	{
		InitializeComponent();
		_myManager = Manager.GetInstance();
        _clientService = ClientInstance.GetInstance();
    }

    private async void btnCadastrar_Clicked(object sender, EventArgs e)
    {
		try
		{
			ChatModel chat = new ChatModel()
			{
				Users = new List<ClientModel> { _myManager.clientModel },
				Name = tbNome.Text,
			};

			TCPMessageModel<ChatModel> tcpMessage = new TCPMessageModel<ChatModel>()
			{
				Message = chat,
				Type = TypeMessage.CreateChat,
				Time = DateTime.Now,
			};

			_clientService.Send<ChatModel>(tcpMessage);

        } catch (ErrorHandled ex) { await DisplayAlert("", ex.Message, "OK"); }
    }
}