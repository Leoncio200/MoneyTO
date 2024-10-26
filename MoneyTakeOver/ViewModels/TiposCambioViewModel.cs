using MoneyTakeOver.DataAccess;
using MoneyTakeOver.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTakeOver.ViewModels
{
    private readonly DivisasDbContext _dbContext;
    private ObservableCollection<TiposCambio> _TCambios;
    public class TiposCambioViewModel : INotifyPropertyChanged
    { 
        public TiposCambioViewModel(DivisasDbContext dbContext)
        {
            _dbContext = dbContext;
            Cambios = new ObservableCollection<TiposCambio>();
            //TxtSearch = string.Empty;
            //_ = GetDatosAsync();
        }
        public ObservableCollection<TiposCambio> Cambios
        {
            get { return _TCambios; }
            set { SetProperty(ref _TCambios, value); }
        }
    }
}
