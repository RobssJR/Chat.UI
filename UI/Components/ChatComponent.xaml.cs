namespace UI.Components;

public partial class ChatComponent : ContentView
{
    private Random rnd = new Random();
    public ChatComponent()
	{
		InitializeComponent();

        frmMain.BackgroundColor = Color.FromRgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));

    }
}