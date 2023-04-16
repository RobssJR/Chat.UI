using Core.Enums;
using Core.Infra.Models.Chat;
using Core.Infra.Models.Client;
using Core.Instances;
using Core.Models;
using Core.Models.Geral;
using Core.Services;
using System;
using UI.Services;
using UI.Singleton;

namespace UI.Pages;

public partial class ChatUserAddPage : ContentPage
{
    private ClientInstance _clientInstance;
    private Manager _myManager;

	public ChatUserAddPage()
	{
		InitializeComponent();
        _myManager = Manager.GetInstance();
        _clientInstance = ClientInstance.GetInstance();
    }

    private async void CarregarPagina()
    {
        TCPMessageModel<ChatModel> tcpMessage = new TCPMessageModel<ChatModel>()
        {
            Message = _myManager.chatSelecionado,
            Time = DateTime.Now,
            Type = TypeMessage.GetChat
        };

        _clientInstance.Send<ChatModel>(tcpMessage);
        bool result = await ClientUtil.AwaitResponse();

        if (result == false)
            return;

        List<ClientModel> clients = _myManager.chatSelecionado.Users.Where(client => client.Id != _myManager.clientModel.Id).ToList();
        lvUsuarios.ItemsSource = clients;
        lbTitle.Text = _myManager.chatSelecionado.Name;
    }

    private void btnAddUser_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(tbEmail.Text))
            return;

        AddUserChatModel addUserChatModel = new AddUserChatModel()
        {
            Chat = _myManager.chatSelecionado,
            Email = tbEmail.Text
        };

        TCPMessageModel<AddUserChatModel> messageObj = new TCPMessageModel<AddUserChatModel>()
        {
            Type = TypeMessage.AddUserChat,
            Message = addUserChatModel,
            Time = DateTime.Now,
        };

        _clientInstance.Send(messageObj);

        CarregarPagina();
    }

    private void btnRemoveUser_Clicked(object sender, EventArgs e)
    {
        ClientModel client = (ClientModel)lvUsuarios.SelectedItem;

        if (client == null) 
            return;

        _myManager.chatSelecionado.Users.Remove(client);

        TCPMessageModel<ChatModel> messageObj = new TCPMessageModel<ChatModel>()
        {
            Type = TypeMessage.UpdateChat,
            Message = _myManager.chatSelecionado,
            Time = DateTime.Now,
        };

        _clientInstance.Send(messageObj);

        CarregarPagina();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        NavigationPage.SetHasBackButton(this, false);
        NavigationPage.SetHasNavigationBar(this, false);

        CarregarPagina();
    }
}