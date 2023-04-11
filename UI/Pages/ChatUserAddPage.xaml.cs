using Core.Enums;
using Core.Infra.Models.Chat;
using Core.Infra.Models.Client;
using Core.Instances;
using Core.Models;
using Core.Models.Geral;
using UI.Singleton;

namespace UI.Pages;

public partial class ChatUserAddPage : ContentPage
{
    private ClientInstance _clientInstance;
    private Manager _myManager;
    private ChatModel _chat;
	public ChatUserAddPage(ChatModel chat)
	{
		InitializeComponent();
        _myManager = Manager.GetInstance();
        _clientInstance = ClientInstance.GetInstance();

        List<ClientModel> clients = chat.Users.Where(client => client.Id != _myManager.clientModel.Id).ToList();

        lvUsuarios.ItemsSource = clients;
        _chat = chat;

        lbTitle.Text = chat.Name;
    }

    private void btnAddUser_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(tbEmail.Text))
            return;

        AddUserChatModel addUserChatModel = new AddUserChatModel()
        {
            Chat = _chat,
            Email = tbEmail.Text
        };

        TCPMessageModel<AddUserChatModel> messageObj = new TCPMessageModel<AddUserChatModel>()
        {
            Type = TypeMessage.AddUserChat,
            Message = addUserChatModel,
            Time = DateTime.Now,
        };

        _clientInstance.Send(messageObj);
    }

    private void btnRemoveUser_Clicked(object sender, EventArgs e)
    {
        ClientModel client = (ClientModel)lvUsuarios.SelectedItem;

        if (client == null) 
            return;

        _chat.Users.Remove(client);

        TCPMessageModel<ChatModel> messageObj = new TCPMessageModel<ChatModel>()
        {
            Type = TypeMessage.UpdateChat,
            Message = _chat,
            Time = DateTime.Now,
        };

        _clientInstance.Send(messageObj);
    }
}