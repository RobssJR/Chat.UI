using UI.Singleton;

namespace UI;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		Manager manager = Manager.GetInstance();

		MainPage = new AppShell();
		
	}
}
