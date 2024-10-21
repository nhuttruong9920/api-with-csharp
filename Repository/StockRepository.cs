using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace api.Repository
{
  public class StockRepository : IStockRepository
  {
    private readonly ApplicationDBContext _context;

    public StockRepository(ApplicationDBContext context)
    {
      _context = context;
    }

    public async Task<Stock> CreateAsync(Stock stockModel)
    {
      await _context.Stocks.AddAsync(stockModel);
      await _context.SaveChangesAsync();
      return stockModel;
    }

    public async Task<Stock?> DeleteAsync(int id)
    {
      var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);

      if(stockModel == null)
      {
        return null;
      }

      _context.Stocks.Remove(stockModel);
      await _context.SaveChangesAsync();
      return stockModel;
    }

    public async Task<List<Stock>> GetAllAsync()
    {
      return await _context.Stocks.Include(c => c.Comments).ToListAsync();
    }

    public async Task<Stock?> GetByIdAsync(int id)
    {
      return await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Stock?> UpdateAsync(UpdateStockRequestDto stockDto)
    {
      var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == stockDto.StockId);

      if(stockModel == null)
      {
        return null;
      }

      stockModel.Symbol = stockDto.Symbol;
      stockModel.CompanyName = stockDto.CompanyName;
      stockModel.Purchase = stockDto.Purchase;
      stockModel.LastDiv = stockDto.LastDiv;
      stockModel.Industry = stockDto.Industry;
      stockModel.MarketCap=stockDto.MarketCap;

      await _context.SaveChangesAsync();

      return stockModel;
    }
  }
}
