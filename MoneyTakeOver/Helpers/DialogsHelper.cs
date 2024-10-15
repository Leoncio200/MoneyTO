using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace MoneyTakeOver.Helpers
{
    public static class DialogsHelper
    {
        private static bool _isLoading;

        public static async Task ShowLoadingMessage(string message)
        {
            if (_isLoading) return; // Evita mostrar múltiples diálogos de carga.

            _isLoading = true;

            // Crea una nueva página de carga
            var loadingPage = new ContentPage
            {
                BackgroundColor = Color.FromArgb("#80000000"), // Fondo semi-transparente
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    Children =
                    {
                        new ActivityIndicator
                        {
                            IsRunning = true,
                            IsVisible = true,
                            VerticalOptions = LayoutOptions.Center,
                            HorizontalOptions = LayoutOptions.Center
                        },
                        new Label
                        {
                            Text = message,
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center,
                        }
                    }
                }
            };

            // Agrega la página de carga a la navegación
            await Application.Current.MainPage.Navigation.PushModalAsync(loadingPage);
        }

        public static async Task HideLoadingMessage()
        {
            if (!_isLoading) return; // Si no está cargando, no hacer nada.

            _isLoading = false;

            // Cierra la página de carga
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        internal static async Task ShowErrorMessage(string v, string errorMessageDialog)
        {
            throw new NotImplementedException();
        }

        internal static async Task ShowWarningMessage(string v1, string v2)
        {
            throw new NotImplementedException();
        }

        internal static async Task ShowSuccessMessage(string v1, string v2)
        {
            throw new NotImplementedException();
        }
    }
}
