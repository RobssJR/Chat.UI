using Core.Enums;
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
    private StackLayout stackMessage;

    public ChatPage()
    {
        InitializeComponent();

        _clientService = ClientInstance.GetInstance();
        _myManager = Manager.GetInstance();
        _myManager.Refresh += RefreshAsync;

        Task task = GetChatsAsync();

        stackMessage = new StackLayout();

        scrollViewMessages.Content = stackMessage;
    }

    private void CarregarPagina()
    {
        RefreshAsync(_myManager.chatSelecionado);
    }

    private void RefreshAsync(ChatModel chat)
    {
        GetChatsAsync();

        if (_myManager.chatSelecionado == null)
            return;

        if (chat.Id == _myManager.chatSelecionado.Id)
            SelecionarChat(chat);
    }

    private async Task GetChatsAsync()
    {
        TCPMessageModel<ClientModel> tCPMessageModel = new TCPMessageModel<ClientModel>()
        {
            Message = _myManager.clientModel,
            Type = TypeMessage.GetChats,
            Time = DateTime.Now,
        };

        _clientService.Send<ClientModel>(tCPMessageModel);
        await ClientUtil.AwaitResponse();

        StackLayout view = new StackLayout();

        _myManager.Chats.ForEach(chat =>
        {
            ChatComponent chatComponent = new ChatComponent(chat);
            chatComponent.selecionarChatEvent += SelecionarChat;

            view.Add(chatComponent);
        });

        scrollViewChat.Content = view;
    }

    private async void MandarMessage()
    {
        try
        {
            if (_myManager.chatSelecionado == null)
                return;

            if (string.IsNullOrEmpty(tbMessage.Text))
                return;

            if (_myManager.chatSelecionado.Messages == null)
                _myManager.chatSelecionado.Messages = new List<MessageModel>();

            _myManager.chatSelecionado.Messages.Add(new MessageModel()
            {
                Client = _myManager.clientModel,
                DataSend = DateTime.Now,
                Message = tbMessage.Text,
            });

            TCPMessageModel<ChatModel> tcpMessage = new TCPMessageModel<ChatModel>()
            {
                Message = _myManager.chatSelecionado,
                Time = DateTime.Now,
                Type = TypeMessage.SendMessage
            };

            _clientService.Send<ChatModel>(tcpMessage);

            tbMessage.Text = string.Empty;

            LoadChat();

            await scrollViewMessages.ScrollToAsync(scrollViewMessages.Content, ScrollToPosition.End, true);
        }
        catch (ErrorHandled ex)
        {
            await DisplayAlert("", ex.Message, "OK");
            return;
        }

    }

    private async void Enviar_Clicked(object sender, EventArgs e)
    {
        try
        {
            MandarMessage();

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

    private void SelecionarChat(ChatModel chat)
    {
        scrollViewMessages.Content = new StackLayout();

        TCPMessageModel<ChatModel> tcpMessage = new TCPMessageModel<ChatModel>()
        {
            Message = chat,
            Time = DateTime.Now,
            Type = TypeMessage.GetChat
        };

        _clientService.Send<ChatModel>(tcpMessage);
        ClientUtil.AwaitResponse();

        tbNameChat.Text = _myManager.chatSelecionado.Name;

        if (_myManager.chatSelecionado.Messages == null)
            return;

        LoadChat();
    }

    private void LoadChat()
    {
        StackLayout newChat = new StackLayout();

        _myManager.chatSelecionado.Messages.OrderBy(message => message.DataSend);

        Dispatcher.DispatchAsync(() =>
        {
            _myManager.chatSelecionado.Messages.ForEach(message =>
            {
                newChat.Add(new MessageComponent(message.Message, (message.Client.Id == _myManager.clientModel.Id)));
            });

            scrollViewMessages.Content = newChat;
        });
    }

    private async void btnAddClient_Clicked(object sender, EventArgs e)
    {
        if (_myManager.chatSelecionado == null)
            return;

        await Navigation.PushAsync(new ChatUserAddPage(_myManager.chatSelecionado));
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        NavigationPage.SetHasBackButton(this, false);
        NavigationPage.SetHasNavigationBar(this, false);

        CarregarPagina();
    }
}