using Core.Enums;
using Core.Instances;
using Core.Models;
using Core.Models.Geral;
using UI.Components;
using UI.Core;
using UI.Services;

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

        scrollView.Content = stack;
    }

    private async void Enviar_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(tbMessage.Text))
                return;


            TCPMessageModel<MessageSendModel> message = new TCPMessageModel<MessageSendModel>()
            {
                Time = DateTime.Now,
                ObjectMessage = new MessageObjectModel<MessageSendModel>()
                {
                    Message = new MessageSendModel()
                    {
                        IdSend = _myManager.clientModel.Id,
                        IdReceiving = _myManager.clientModel.Id,
                        Message = tbMessage.Text
                    },

                    Type = TypeMessage.SendMessage
                }
            };

            ClientService clientService = new ClientService();
            clientService.Send(message);

            stack.Add(new MessageComponent(tbMessage.Text, true));
            tbMessage.Text = string.Empty;
            await scrollView.ScrollToAsync(stack, ScrollToPosition.End, true);
        } catch
        {
            throw;
        }
    }
}