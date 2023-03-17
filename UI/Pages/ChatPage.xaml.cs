using Core.Instances;
using UI.Components;
using UI.Singleton;

namespace UI.Pages;

public partial class ChatPage : ContentPage
{
    private ClientInstance _clientInstance;
    private Manager _myManager;
    private StackLayout stack;
    public ChatPage()
    {
		InitializeComponent();
        _clientInstance = ClientInstance.GetInstance();
        _myManager = Manager.GetInstance();

        stack = new StackLayout()
        {
            Spacing = 15
        };

        for (int i = 0; i <= 10; i++)
        {
            stack.Add(new MessageComponent("Bom dia " + i, false));
        }

        scrollViewMessages.Content = stack;

        StackLayout stackA = new StackLayout()
        {
            Spacing = 15
        };

        for (int i = 0; i <= 10; i++)
        {
            stackA.Add(new ChatComponent());
        }

        scrollViewChat.Content = stackA;
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
        } catch (Exception ex)
        {
            await DisplayAlert("", ex.Message, "OK");
            return;
        }
    }

    private async void btnCreateChat_Clicked(object sender, EventArgs e)
    {
        
    }
}