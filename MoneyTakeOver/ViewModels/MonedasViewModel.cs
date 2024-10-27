﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using MoneyTakeOver.Models;
using MoneyTakeOver.Helpers;
using MoneyTakeOver.DataAccess;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using System.Net;

namespace MoneyTakeOver.ViewModels
{
    public class MonedasViewModel : INotifyPropertyChanged
    {
        private readonly DivisasDbContext _dbContext;
        private ObservableCollection<Monedas> _divisas;

        private ObservableCollection<TiposCambio> _tiposCambioList;
        private string _nuevaDivisa;
        private string _search;
        private bool _isDivisasEmpty;
        private bool _isInitialLoad = true;
        private bool _isLoading = false;

        public ObservableCollection<Monedas> Divisas { get { return _divisas; }set { SetProperty(ref _divisas, value); }
        }

        public ObservableCollection<TiposCambio> TiposCambioList { get; set; } // Propiedad para la lista de tipos de cambio


        public MonedasViewModel(DivisasDbContext dbContext)
        {
            _dbContext = dbContext;
            Divisas = new ObservableCollection<Monedas>();
            TiposCambioList = new ObservableCollection<TiposCambio>();
            TxtSearch = string.Empty;
            _ = GetDatosAsync();
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
                        _ = GetDivisas();
                    }
                }
            }
        }


        private decimal _tipoCambioCompra;
        public decimal TipoCambioCompra
        {
            get { return _tipoCambioCompra; }
            set { SetProperty(ref _tipoCambioCompra, value); }
        }

        private decimal _tipoCambioVenta;
        public decimal TipoCambioVenta
        {
            get { return _tipoCambioVenta; }
            set { SetProperty(ref _tipoCambioVenta, value); }
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

        public async Task GetDatosAsync()
        {
            await GetDivisas();
        }

        public async Task InsertarDivisas()
        {
            await AgregarTodasLasMonedas();
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
             
            }
            finally
            {
                DialogsHelper.HideLoadingMessage();
            }
        }
    

          public async Task GetDatosAsyncTipo()
            {
                try
                {
                    // Carga TiposCambioList y asegura que incluya la información de Moneda
                    var tiposCambio = await _dbContext.TiposCambio
                        .Include(tc => tc.Moneda) // Incluye la información de la moneda asociada
                        .ToListAsync();

                    TiposCambioList.Clear();
                    foreach (var tipoCambio in tiposCambio)
                    {
                        if (tipoCambio.Moneda != null) // Asegura que la moneda está enlazada
                        {
                            TiposCambioList.Add(tipoCambio);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al cargar tipos de cambio: {ex.Message}");
                }
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
                    await DialogsHelper.ShowWarningMessage("Warning", "El campo de la divisa no puede quedar vacío.");
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
                    Monedas monedaBaseNueva = new Monedas { Nombre = "Pesos Mexicanos", ActivoDivisa = true 
                        ,
                        
                    };
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
       
    
      public async Task ActualizarValorTipoCambio(int monedaId, decimal nuevoTipoCompra, decimal nuevoTipoVenta)
        {
            try
            {
                var tipoCambio = await _dbContext.TiposCambio
                    .FirstOrDefaultAsync(tc => tc.MonedaId == monedaId);

                if (tipoCambio != null)
                {
                    tipoCambio.TipoCambioCompra = nuevoTipoCompra;
                    tipoCambio.TipoCambioVenta = nuevoTipoVenta;

                    await _dbContext.SaveChangesAsync();
                    Console.WriteLine("Actualización Exitosa: Los tipos de cambio han sido actualizados.");
                }
                else
                {
                    Console.WriteLine("Error: No se encontró el tipo de cambio para esta moneda.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar el tipo de cambio: {ex.Message}");
            }
        }
        //obtener el valor de tipo de cambio por ID
        public async Task<TiposCambio?> GetTipoCambioById(int monedaId)
        {
            try
            {
                var tipoCambio = await _dbContext.TiposCambio
                    .Where(tc => tc.MonedaId == monedaId)
                    .FirstOrDefaultAsync();

                return tipoCambio; // Devolver el tipo de cambio si existe
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el tipo de cambio: {ex.Message}");
                return null; // Devuelve null si ocurre un error
            }
        }

        public async Task ActualizarTipoCambio(int monedaId, decimal nuevoTipoCompra, decimal nuevoTipoVenta)
        {
            try
            {
                var tipoCambio = await _dbContext.TiposCambio
                    .Where(tc => tc.MonedaId == monedaId)
                    .FirstOrDefaultAsync();

                if (tipoCambio != null)
                {
                    tipoCambio.TipoCambioCompra = nuevoTipoCompra;
                    tipoCambio.TipoCambioVenta = nuevoTipoVenta;

                    await _dbContext.SaveChangesAsync();
                    await DialogsHelper.ShowSuccessMessage("Actualización Exitosa", "Los tipos de cambio han sido actualizados.");
                }
                else
                {
                    await DialogsHelper.ShowWarningMessage("Error", "No se encontró el tipo de cambio para esta moneda.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar el tipo de cambio: {ex.Message}");
                await DialogsHelper.ShowErrorMessage("Error", "Hubo un problema al actualizar el tipo de cambio.");
            }
        }

        //METODOS HARD CODEADOS
        public async Task AgregarTodasLasMonedas()
        {
            try
            {
                // Lista de monedas predefinidas
                var monedas = new List<Monedas>
                {
                    new Monedas {  Id = 1, Nombre = "Peso mexicano (MXN)", ActivoDivisa = true },
                new Monedas { Id = 2, Nombre = "Dólar estadounidense (USD)", ActivoDivisa = true },
                new Monedas { Id = 3, Nombre = "Euro (EUR)", ActivoDivisa = true },
                new Monedas { Id = 4, Nombre = "Libra esterlina (GBP)", ActivoDivisa = true },
                new Monedas { Id = 5, Nombre = "Dólar canadiense (CAD)", ActivoDivisa = true },
                new Monedas { Id = 6, Nombre = "Franco suizo (CHF)", ActivoDivisa = true },
                new Monedas { Id = 7, Nombre = "Yuan chino (CNY)", ActivoDivisa = true },
                new Monedas { Id = 8, Nombre = "Corona danesa (DKK)", ActivoDivisa = true },
                new Monedas { Id = 9, Nombre = "Corona noruega (NOK)", ActivoDivisa = true },
                new Monedas { Id = 10, Nombre = "Corona sueca (SEK)", ActivoDivisa = true },
                new Monedas { Id = 11, Nombre = "Rublo ruso (RUB)", ActivoDivisa = true },
                new Monedas { Id = 12, Nombre = "Real brasileño (BRL)", ActivoDivisa = true },
                new Monedas { Id = 13, Nombre = "Dólar australiano (AUD)", ActivoDivisa = true },
                new Monedas { Id = 14, Nombre = "Dólar neozelandés (NZD)", ActivoDivisa = true },
                new Monedas { Id = 15, Nombre = "Rupia india (INR)", ActivoDivisa = true },
                new Monedas { Id = 16, Nombre = "Won surcoreano (KRW)", ActivoDivisa = true },
                new Monedas { Id = 17, Nombre = "Dólar de Hong Kong (HKD)", ActivoDivisa = true },
                new Monedas { Id = 18, Nombre = "Dólar de Singapur (SGD)", ActivoDivisa = true },
                new Monedas { Id = 19, Nombre = "Rand sudafricano (ZAR)", ActivoDivisa = true },
                new Monedas { Id = 20, Nombre = "Peso cubano (CUP)", ActivoDivisa = true },
                    
                    // Agrega las demás monedas según sea necesario
                };

                foreach (var moneda in monedas)
                {
                    // Verificar si la moneda ya existe en la base de datos
                    var existeMoneda = await _dbContext.Monedas.AnyAsync(m => m.Nombre == moneda.Nombre);
                    if (!existeMoneda)
                    {
                        // Agregar la moneda si no existe
                        _dbContext.Monedas.Add(moneda);
                    }
                }

                await _dbContext.SaveChangesAsync();
                await DialogsHelper.ShowSuccessMessage("Success", "Todas las monedas han sido agregadas exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al procesar la solicitud Message: {ex.Message}");
                Console.WriteLine($"Error al procesar la solicitud StackTrace: {ex.StackTrace}");
                Console.WriteLine("=======================");
                await DialogsHelper.ShowErrorMessage("Error", $"Fallo al procesar la solicitud: {ex.Message}");
            }
            finally
            {
                DialogsHelper.HideLoadingMessage();
            }
        }

        public async Task AgregarTiposCambio()
        {
            try
            {
                // Lista de tipos de cambio predefinidos según el ID de la moneda
                var tiposCambio = new List<TiposCambio>
                {
                    new TiposCambio { MonedaId = 1, TipoCambioCompra = 18.00m, TipoCambioVenta = 18.50m },
                    new TiposCambio { MonedaId = 2, TipoCambioCompra = 20.00m, TipoCambioVenta = 20.50m },
                    new TiposCambio { MonedaId = 3, TipoCambioCompra = 22.00m, TipoCambioVenta = 22.50m },
                    new TiposCambio { MonedaId = 4, TipoCambioCompra = 25.00m, TipoCambioVenta = 25.50m },
                    new TiposCambio { MonedaId = 5, TipoCambioCompra = 15.00m, TipoCambioVenta = 15.50m },
                    new TiposCambio { MonedaId = 6, TipoCambioCompra = 18.50m, TipoCambioVenta = 19.00m },
                    new TiposCambio { MonedaId = 7, TipoCambioCompra = 3.00m, TipoCambioVenta = 3.50m },
                    new TiposCambio { MonedaId = 8, TipoCambioCompra = 2.50m, TipoCambioVenta = 2.80m },
                    new TiposCambio { MonedaId = 9, TipoCambioCompra = 2.20m, TipoCambioVenta = 2.50m },
                    new TiposCambio { MonedaId = 10, TipoCambioCompra = 2.00m, TipoCambioVenta = 2.30m },
                    new TiposCambio { MonedaId = 11, TipoCambioCompra = 0.25m, TipoCambioVenta = 0.30m },
                    new TiposCambio { MonedaId = 12, TipoCambioCompra = 4.00m, TipoCambioVenta = 4.50m },
                    new TiposCambio { MonedaId = 13, TipoCambioCompra = 14.00m, TipoCambioVenta = 14.50m },
                    new TiposCambio { MonedaId = 14, TipoCambioCompra = 15.00m, TipoCambioVenta = 15.60m },
                    new TiposCambio { MonedaId = 15, TipoCambioCompra = 0.015m, TipoCambioVenta = 0.017m },
                    new TiposCambio { MonedaId = 16, TipoCambioCompra = 0.018m, TipoCambioVenta = 0.020m },
                    new TiposCambio { MonedaId = 17, TipoCambioCompra = 0.25m, TipoCambioVenta = 0.30m },
                    new TiposCambio { MonedaId = 18, TipoCambioCompra = 0.22m, TipoCambioVenta = 0.25m },
                    new TiposCambio { MonedaId = 19, TipoCambioCompra = 0.20m, TipoCambioVenta = 0.22m },
                    new TiposCambio { MonedaId = 20, TipoCambioCompra = 0.30m, TipoCambioVenta = 0.35m },
                };

                foreach (var tipoCambio in tiposCambio)
                {
                    // Verificar si el tipo de cambio ya existe para esta moneda
                    var existeTipoCambio = await _dbContext.TiposCambio
                        .AnyAsync(tc => tc.MonedaId == tipoCambio.MonedaId);
                    if (!existeTipoCambio)
                    {
                        // Agregar el tipo de cambio si no existe
                        _dbContext.TiposCambio.Add(tipoCambio);
                    }
                }

                await _dbContext.SaveChangesAsync();
                await DialogsHelper.ShowSuccessMessage("Éxito", "Todos los tipos de cambio han sido agregados exitosamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al procesar la solicitud Message: {ex.Message}");
                Console.WriteLine($"Error al procesar la solicitud StackTrace: {ex.StackTrace}");
                Console.WriteLine("=======================");
                await DialogsHelper.ShowErrorMessage("Error", $"Fallo al procesar la solicitud: {ex.Message}");
            }
            finally
            {
                DialogsHelper.HideLoadingMessage();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action onChanged = null!)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
