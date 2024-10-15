using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using MoneyTakeOver.Models;
using MoneyTakeOver;
using System.Text;  
using System.Globalization;
using System.Text.RegularExpressions;


using System.Text;
using System.Globalization;

using System.Text.RegularExpressions;
using MoneyTakeOver.Helpers;



namespace MoneyTakeOver.Models
{
    public class MonedasViewModel : INotifyPropertyChanged
    {
        private readonly DivisasDbContext _dbContext;
        private ObservableCollection<Monedas> _divisas;
        private string _nuevaDivisa;
        private string _search;
        private bool _isDivisasEmpty;
        private bool _isInitialLoad = true;
        private bool _isLoading = false;

        public ObservableCollection<Monedas> Divisas
        {
            get { return _divisas; }
            set { SetProperty(ref _divisas, value); }
        }

        public string NuevaDivisa
        {
            get { return _nuevaDivisa; }
            set { SetProperty(ref _nuevaDivisa, value); }
        }

        public bool IsDivisasEmpty
        {
            get { return _isDivisasEmpty; }
            set { SetProperty(ref _isDivisasEmpty, value); }
        }

        public string TxtSearch
        {
            get { return _search; }
            set
            {
                if (SetProperty(ref _search, value))
                {
                    if (!_isLoading)
                    {
                        GetDivisas();
                    }
                }
            }
        }

        private string NormalizeString(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var normalizedString = input.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            var cleanString = stringBuilder.ToString()
                .Replace(",", " ")
                .Replace(".", " ")
                .Replace("á", "a")
                .Replace("é", "e")
                .Replace("í", "i")
                .Replace("ó", "o")
                .Replace("ú", "u");

            cleanString = Regex.Replace(cleanString, @"\s+", " ").Trim();

            return cleanString.ToLower();
        }

        public async Task GetDivisas()
        {
            if (_isLoading)
                return;

            try
            {
                if (_isInitialLoad)
                {
                    DialogsHelper.ShowLoadingMessage("Cargando, por favor espere...");
                    _isInitialLoad = false;
                }

                _isLoading = true;
                await Task.Delay(100);

                var query = _dbContext.Monedas.AsQueryable();
                query = query.Where(m => m.Nombre != "Pesos Mexicanos");

                var normalizedSearch = NormalizeString(TxtSearch);

                if (!string.IsNullOrWhiteSpace(normalizedSearch))
                {
                    var allDivisas = await query.ToListAsync();
                    Divisas.Clear();
                    foreach (var divisa in allDivisas)
                    {
                        var normalizedNombre = NormalizeString(divisa.Nombre!);
                        if (normalizedNombre.Contains(normalizedSearch))
                        {
                            Divisas.Add(divisa);
                        }
                    }
                }
                else
                {
                    var allDivisas = await query.ToListAsync();
                    Divisas.Clear();
                    foreach (var divisa in allDivisas)
                    {
                        Divisas.Add(divisa);
                    }
                }

                IsDivisasEmpty = Divisas.Count == 0;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error al procesar la solicitud Message: {ex.Message}";
                var errorStackTrace = $"Error al procesar la solicitud StackTrace: {ex.StackTrace}";
                var errorMessageDialog = $"Fallo al procesar la solicitud: {ex.Message}";
                Console.WriteLine("=== ERROR DETECTADO ===");
                Console.WriteLine(errorMessage);
                Console.WriteLine(errorStackTrace);
                Console.WriteLine("=======================");
                await DialogsHelper.ShowErrorMessage("Error", errorMessageDialog);
            }
            finally
            {
                _isLoading = false;
                if (!_isInitialLoad)
                {
                    DialogsHelper.HideLoadingMessage();
                }
            }
        }

        public async Task AddDivisa()
        {
            try
            {
                DialogsHelper.ShowLoadingMessage("Cargando, por favor espere...");

                await Task.Delay(100);

                if (string.IsNullOrWhiteSpace(NuevaDivisa))
                {
                    await DialogsHelper.ShowWarningMessage("Warning","El campo de la divisa no puede quedar vacío.");
                    return;
                }

                var configuracion = await _dbContext.Configuraciones.FirstOrDefaultAsync();
                if (configuracion == null)
                {
                    await DialogsHelper.ShowWarningMessage("Warning", "No se puede crear la divisa si no existe una configuración.");
                    return;
                }

                var monedaBase = await _dbContext.Monedas.FirstOrDefaultAsync(m => m.Nombre == "Pesos Mexicanos");
                if (monedaBase == null)
                {
                    Monedas monedaBaseNueva = new Monedas { Nombre = "Pesos Mexicanos", ActivoDivisa = true };
                    _dbContext.Monedas.Add(monedaBaseNueva);
                    await _dbContext.SaveChangesAsync();

                    configuracion.TipoCambioBaseId = monedaBaseNueva.Id;
                    await _dbContext.SaveChangesAsync();
                }

                string primeraLetraMayuscula = char.ToUpper(NuevaDivisa[0]) + NuevaDivisa.Substring(1).ToLower();
                Monedas nuevaDivisa = new Monedas { Nombre = primeraLetraMayuscula, ActivoDivisa = true };

                _dbContext.Monedas.Add(nuevaDivisa);
                await _dbContext.SaveChangesAsync();

                int nuevaDivisaId = nuevaDivisa.Id;

                if (nuevaDivisa.Nombre != "Pesos Mexicanos")
                {
                    TiposCambio nuevoTipoCambio = new TiposCambio
                    {
                        MonedaId = nuevaDivisaId,
                        TipoCambioCompra = 0.00m,
                        TipoCambioVenta = 0.00m
                    };
                    _dbContext.TiposCambio.Add(nuevoTipoCambio);
                    await _dbContext.SaveChangesAsync();
                }

                NuevaDivisa = "";
                await GetDivisas();
                await DialogsHelper.ShowSuccessMessage("Success", "Divisa creada exitosamente.");
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error al procesar la solicitud Message: {ex.Message}";
                var errorStackTrace = $"Error al procesar la solicitud StackTrace: {ex.StackTrace}";
                var errorMessageDialog = $"Fallo al procesar la solicitud: {ex.Message}";
                Console.WriteLine("=== ERROR DETECTADO ===");
                Console.WriteLine(errorMessage);
                Console.WriteLine(errorStackTrace);
                Console.WriteLine("=======================");
                await DialogsHelper.ShowErrorMessage("Error", errorMessageDialog);
            }
            finally
            {
                DialogsHelper.HideLoadingMessage();
            }
        }

        public async Task DeleteDivisa(Monedas divisa)
        {
            try
            {
                DialogsHelper.ShowLoadingMessage("Cargando, por favor espere...");

                await Task.Delay(100);

                if (divisa != null)
                {
                    var tipoCambio = await _dbContext.TiposCambio.FirstOrDefaultAsync(tc => tc.MonedaId == divisa.Id);
                    if (tipoCambio != null)
                    {
                        _dbContext.TiposCambio.Remove(tipoCambio);
                    }

                    _dbContext.Monedas.Remove(divisa);
                    await _dbContext.SaveChangesAsync();

                    await GetDivisas();
                    await DialogsHelper.ShowSuccessMessage("Success", "Divisa eliminada exitosamente.");
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error al procesar la solicitud Message: {ex.Message}";
                var errorStackTrace = $"Error al procesar la solicitud StackTrace: {ex.StackTrace}";
                var errorMessageDialog = $"Fallo al procesar la solicitud: {ex.Message}";
                Console.WriteLine("=== ERROR DETECTADO ===");
                Console.WriteLine(errorMessage);
                Console.WriteLine(errorStackTrace);
                Console.WriteLine("=======================");
                await DialogsHelper.ShowErrorMessage("Error", errorMessageDialog);
            }
            finally
            {
                DialogsHelper.HideLoadingMessage();
            }
        }

        public MonedasViewModel(DivisasDbContext dbContext)
        {
            _dbContext = dbContext;
            Divisas = new ObservableCollection<Monedas>();
            TxtSearch = string.Empty;
            GetDatosAsync();
        }

        public async Task GetDatosAsync()
        {
            await GetDivisas();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action onChanged = null!)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
