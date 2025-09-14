using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            return await _context.Product
                .AsNoTracking()
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    CreatedBy = p.CreatedBy,
                    CreatedOn = p.CreatedOn,
                    ModifiedBy = p.ModifiedBy,
                    ModifiedOn = p.ModifiedOn
                }).ToListAsync();
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var p = await _context.Product.FindAsync(id);
            if (p == null) return null;
            return new ProductDto
            {
                Id = p.Id,
                ProductName = p.ProductName,
                CreatedBy = p.CreatedBy,
                CreatedOn = p.CreatedOn,
                ModifiedBy = p.ModifiedBy,
                ModifiedOn = p.ModifiedOn
            };
        }

        public async Task<ProductDto> CreateAsync(ProductDto dto)
        {
            var product = new Product
            {
                ProductName = dto.ProductName,
                CreatedBy = dto.CreatedBy,
                CreatedOn = DateTime.UtcNow
            };
            _context.Product.Add(product);
            await _context.SaveChangesAsync();
            dto.Id = product.Id;
            dto.CreatedOn = product.CreatedOn;
            return dto;
        }

        public async Task<ProductDto?> UpdateAsync(int id, ProductDto dto)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null) return null;
            product.ProductName = dto.ProductName;
            product.ModifiedBy = dto.ModifiedBy;
            product.ModifiedOn = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            dto.Id = product.Id;
            dto.CreatedOn = product.CreatedOn;
            dto.ModifiedOn = product.ModifiedOn;
            return dto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null) return false;
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
