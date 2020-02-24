using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiWithEntity.Data;
using WebApiWithEntity.Models;

namespace WebApiWithEntity.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly DataContext _context;

        public MovieRepository([FromServices] DataContext context)
        {
            _context = context;
        }

        public async Task<int> AddMovie(MovieModel movie)
        {
            _context.Movies.Add(movie);
            return _context.SaveChanges();
        }

        public async Task<IEnumerable<DirectorModel>> GetAllDirectors()
        {
            return await _context.Directors.ToListAsync();
        }

        public async Task<IEnumerable<MovieModel>> GetAllMovies()
        {
            return await _context.Movies.Include(r => r.Director).ToListAsync();
        }

        public async Task<MovieModel> GetMovieById(int id)
        {
            return await _context.Movies.Include(r => r.Director).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}