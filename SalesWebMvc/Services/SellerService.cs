﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models.Entities;
using SalesWebMvc.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Services
{
    public class SellerService
    {
        private readonly SalesWebMvcContext _context;

        public SellerService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.OrderByDescending(x => x.Id).ToListAsync();
        }

        public async Task InsertAsync(Seller seller)
        {
            _context.Add(seller);
            await _context.SaveChangesAsync();
        }

        public async Task<Seller> FindByIdAsync(int id)
        {
            return await _context.Seller
                .Include(x => x.Department)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            try
            {
                var seller = await _context.Seller.FindAsync(id);
                _context.Seller.Remove(seller);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new IntegrityException("Não é possivel deletar um vendedor que tenha vendas!");
            }
        }

        public async Task UpdateAsync(Seller seller)
        {
            bool hasAny = await _context.Seller.AnyAsync(x => x.Id == seller.Id);

            if (!hasAny)
            {
                throw new NotFoundException("Id não encontrado!");
            }

            try
            {
                _context.Update(seller);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {

                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}
