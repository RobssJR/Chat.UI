using Core.Infra.Models.Chat;

namespace UI.Components;

public partial class ChatComponent : ContentView
{
    private Random rnd = new Random();
    public ChatModel Chat { get; set; }
    public delegate void selecionarChatDelagate(ChatModel chat);
    public event selecionarChatDelagate selecionarChatEvent;
    public ChatComponent(ChatModel chat)
	{
		InitializeComponent();

        Chat = chat;
        frmMain.BackgroundColor = Color.FromRgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));

        frmMain.Text = Chat.Name;
    }

    private void selecionarChat(object sender, EventArgs e)
    {
        selecionarChatEvent(Chat);
    }
}