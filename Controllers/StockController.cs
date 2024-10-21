using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
      private readonly ApplicationDBContext _context;
      public StockController(ApplicationDBContext context)
      {
          _context = context;
      }

      [HttpGet]
      public async Task<IActionResult> GetAll()
      {
        var stocks = await _context.Stocks.Select(s => s.ToStockDto()).ToListAsync();
        return Ok(stocks);
      }

      [HttpGet("{id}")]
      public async Task<IActionResult> GetById([FromRoute] int id)
      {
        var stock = await _context.Stocks.FindAsync(id);

        if(stock == null)
        {
          return NotFound(new { Message = "Stock not found" });
        }
        return Ok(stock.ToStockDto());
      }

      [HttpPost]
      public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
      {
        var stockModel = stockDto.ToStockFromCreateDTO();
        await _context.Stocks.AddAsync(stockModel);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
      }

      [HttpPut]
      public async Task<IActionResult> Update([FromBody] UpdateStockRequestDto updateDto)
      {
        var stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == updateDto.StockId);

        if(stockModel == null){
          return NotFound(new { Message = "Stock not found" });
        }

        stockModel.Symbol = updateDto.Symbol;
        stockModel.CompanyName = updateDto.CompanyName;
        stockModel.Purchase = updateDto.Purchase;
        stockModel.LastDiv = updateDto.LastDiv;
        stockModel.Industry = updateDto.Industry;
        stockModel.MarketCap=updateDto.MarketCap;

        await _context.SaveChangesAsync();

        return Ok(stockModel.ToStockDto());
      }
      [HttpDelete]
      public async Task<IActionResult> Delete([FromBody] int id){
        var stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
        if(stockModel == null){
          return NotFound(new { Message = "Stock not found" });
        }
        _context.Stocks.Remove(stockModel);
        await _context.SaveChangesAsync();
        return Ok(new { Message = "Delete success!" });
      }
    }
}
