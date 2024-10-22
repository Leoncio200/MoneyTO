using MoneyTakeOver.Views;

namespace MoneyTakeOver
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Configura AppShell como la página principal
            MainPage = new AppShell();
        }
    }
}
