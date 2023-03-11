using UI.Core;

namespace UI.Components;

public partial class MessageComponent : ContentView
{
    private Manager _myManager;
	public MessageComponent(string message, bool send)
	{
		InitializeComponent();
        _myManager = Manager.GetInstance();

        tbTextMessage.Text = message;

        if (send)
        {
            stackTextMessage.Style = (Style)Application.Current.Resources.MergedDictionaries.Last()["StyleMessageSend"];
            frmTextMessage.Style = (Style)Application.Current.Resources.MergedDictionaries.Last()["StyleMessageSendFrame"];
        }
    }
}