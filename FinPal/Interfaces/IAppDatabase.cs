using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinPal.Interfaces
{
    public interface IAppDatabase<T>
    {
        Task<List<T>> GetItemsAsync();
        Task<int> SaveItemAsync(T item);
        Task<int> DeleteItemAsync(T item);
    }
}
