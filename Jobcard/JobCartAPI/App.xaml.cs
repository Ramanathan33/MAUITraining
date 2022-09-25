using JobCartAPI.DataServices;

namespace JobCartAPI;

public partial class App : Application
{

    public App()
	{
		InitializeComponent();
        MainPage = new AppShell();
	}
}
