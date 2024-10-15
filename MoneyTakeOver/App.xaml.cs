using MoneyTakeOver.Views;

namespace MoneyTakeOver
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new CambioBase()); 
        }
    }
}
