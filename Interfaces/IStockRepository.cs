using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stock;
using api.Models;

namespace api.Interfaces
{
    public interface IStockRepository
    {
      Task<List<Stock>> GetAllAsync();
      Task<Stock?> GetByIdAsync(int id);
      Task<Stock> CreateAsync(Stock stockModel);
      Task<Stock?> UpdateAsync(UpdateStockRequestDto stockDto);
      Task<Stock?>  DeleteAsync(int id);
      Task<Boolean> StockExists(int id);
    }
}
