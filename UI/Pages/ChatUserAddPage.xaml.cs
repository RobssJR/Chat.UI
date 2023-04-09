using Core.Enums;
using Core.Infra.Models.Chat;
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

        lvUsuarios.ItemsSource = chat.Users;
        _chat = chat;
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
}