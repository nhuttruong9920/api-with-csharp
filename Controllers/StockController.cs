using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
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
      private readonly IStockRepository _stockRepo;
      public StockController(ApplicationDBContext context, IStockRepository stockRepo)
      {
        _stockRepo = stockRepo;
        _context = context;
      }

      [HttpGet]
      public async Task<IActionResult> GetAll()
      {
        var stocks = await _stockRepo.GetAllAsync();
        var stockDto = stocks.Select(s => s.ToStockDto());
        return Ok(stocks);
      }

      [HttpGet("{id}")]
      public async Task<IActionResult> GetById([FromRoute] int id)
      {
      var stock = await _stockRepo.GetByIdAsync(id);

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
        await _stockRepo.CreateAsync(stockModel);
        return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
      }

      [HttpPut]
      public async Task<IActionResult> Update([FromBody] UpdateStockRequestDto updateDto)
      {
        var stockModel = await _stockRepo.UpdateAsync(updateDto);

        if(stockModel == null){
          return NotFound(new { Message = "Stock not found" });
        }

        return Ok(stockModel.ToStockDto());
      }
      
      [HttpDelete]
      public async Task<IActionResult> Delete([FromBody] int id){
        var stockModel = await _stockRepo.DeleteAsync(id);

        if(stockModel == null){
          return NotFound(new { Message = "Stock not found" });
        }

        return Ok(new { Message = "Delete success!" });
      }
    }
}
