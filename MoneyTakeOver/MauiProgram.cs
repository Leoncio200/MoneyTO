using Microsoft.Extensions.Logging;

using UraniumUI;
using CommunityToolkit.Maui;

namespace MoneyTakeOver
{

    public static class MauiProgram
    {

        public static MauiApp CreateMauiApp()
        {

            var builder = MauiApp.CreateBuilder();
            builder.Services.AddCommunityToolkitDialogs();

            builder.UseMauiApp<App>().UseUraniumUI().UseUraniumUIMaterial() // 👈 Don't forget these two lines.
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Font-Awesome-6-Brands-Regular-400.otf", "FABrands");
            }).UseMauiCommunityToolkit();
#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}