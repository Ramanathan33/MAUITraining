using JobCartAPI.DataServices;
using JobCartAPI.Views;

namespace JobCartAPI;

public partial class AppShell : Shell
{
  

    public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute(nameof(AddEditJob), typeof(AddEditJob));
    }
}
