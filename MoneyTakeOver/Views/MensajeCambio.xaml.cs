using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace MoneyTakeOver.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MensajeCambio : ContentPage
    {
        public MensajeCambio()
        {
            InitializeComponent();
        }

         private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CambioBase());
    }

      
    }
}