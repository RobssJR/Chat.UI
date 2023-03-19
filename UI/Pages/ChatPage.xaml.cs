using Core.Enums;
using Core.Infra;
using Core.Infra.Models.Chat;
using Core.Infra.Models.Client;
using Core.Instances;
using Core.Models;
using Core.Models.Exception;
using UI.Components;
using UI.Services;
using UI.Singleton;

namespace UI.Pages;

public partial class ChatPage : ContentPage
{
    private ClientInstance _clientService;
    private Manager _myManager;
    private StackLayout stack;
    public ChatPage()
    {
		InitializeComponent();
        _clientService = ClientInstance.GetInstance();
        _myManager = Manager.GetInstance();
        GetChats();
    }

    private List<ChatModel> GetChats()
    {
        List<ChatModel> listChats = new List<ChatModel>();

        TCPMessageModel<ClientModel> tCPMessageModel = new TCPMessageModel<ClientModel>()
        {
            Message = _myManager.clientModel,
            Type = TypeMessage.GetChats,
            Time = DateTime.Now,
        };

        _clientService.Send<ClientModel>(tCPMessageModel);

        return listChats;
    }

    private async void Enviar_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(tbMessage.Text))
                return;

            stack.Add(new MessageComponent(tbMessage.Text, true));
            tbMessage.Text = string.Empty;
            await scrollViewMessages.ScrollToAsync(stack, ScrollToPosition.End, true);
        } catch (ErrorHandled ex)
        {
            await DisplayAlert("", ex.Message, "OK");
            return;
        }
    }

    private async void btnCreateChat_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ChatCreatePage());
    }
}