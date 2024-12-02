using Core.Security.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using TechCareer.DataAccess.Contexts;
using TechCareer.DataAccess.Repositories.Abstracts;

namespace TechCareer.DataAccess.Repositories.Concretes;

public class InstructorRepository:IInstructorRepository
{
    private readonly BaseDbContext _context;

    public InstructorRepository(BaseDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Instructor>> GetAllAsync()
    {
        // Tüm eğitmenleri listele
        return await _context.Instructors.ToListAsync();
    }

    public async Task<Instructor> GetByIdAsync(Guid id)
    {
        // ID'ye göre eğitmen getir
        return await _context.Instructors.FindAsync(id);
    }

    public async Task AddAsync(Instructor instructor)
    {
        // Yeni bir eğitmen ekle
        await _context.Instructors.AddAsync(instructor);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Instructor instructor)
    {
        // Var olan bir eğitmeni güncelle
        _context.Instructors.Update(instructor);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        // ID'ye göre eğitmeni sil
        var instructor = await GetByIdAsync(id);
        if (instructor != null)
        {
            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();
        }
    }


}
