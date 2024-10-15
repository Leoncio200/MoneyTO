using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyTakeOver.Models
{
   public static class ConexionDB
    {
        public static string DevolverRuta(string nombreBaseDatos)
        {
            string rutaBaseDatos = string.Empty;

            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                rutaBaseDatos = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                rutaBaseDatos = Path.Combine(rutaBaseDatos, nombreBaseDatos);
            } else if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                rutaBaseDatos = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                rutaBaseDatos = Path.Combine(rutaBaseDatos,"..","Library",nombreBaseDatos);
            }

            return rutaBaseDatos;
        }

        public static void BorrarBaseDeDatos(string nombreBaseDatos)
        {
            string rutaBaseDatos = DevolverRuta(nombreBaseDatos);

            if (File.Exists(rutaBaseDatos))
            {
                File.Delete(rutaBaseDatos);
                Console.WriteLine("Base de datos eliminada.");
            }
            else
            {
                Console.WriteLine("No se encontró la base de datos para eliminar.");
            }
        }
    }
}