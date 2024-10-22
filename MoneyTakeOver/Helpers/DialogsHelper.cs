using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTakeOver.Helpers
{
    public static class DialogsHelper
    {
        public static void ShowLoadingMessage(string message)
        {
            // Implementa el código para mostrar un diálogo de carga con el mensaje dado
            Console.WriteLine($"[Loading]: {message}");
        }

        public static void HideLoadingMessage()
        {
            // Implementa el código para ocultar el diálogo de carga
            Console.WriteLine("[Loading]: Oculto");
        }

        public static async Task ShowSuccessMessage(string title, string message)
        {
            // Implementa el código para mostrar un mensaje de éxito
            Console.WriteLine($"[Success] {title}: {message}");
            await Task.CompletedTask;
        }

        public static async Task ShowErrorMessage(string title, string message)
        {
            // Implementa el código para mostrar un mensaje de error
            Console.WriteLine($"[Error] {title}: {message}");
            await Task.CompletedTask;
        }

        public static async Task ShowWarningMessage(string title, string message)
        {
            // Implementa el código para mostrar un mensaje de advertencia
            Console.WriteLine($"[Warning] {title}: {message}");
            await Task.CompletedTask;
        }
    }
}
