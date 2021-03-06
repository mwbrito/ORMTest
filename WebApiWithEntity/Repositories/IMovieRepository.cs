using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiWithEntity.Models;

namespace WebApiWithEntity.Repositories
{
    public interface IMovieRepository  
    {  
        Task<IEnumerable<MovieModel>> GetAllMovies();  
        Task<MovieModel> GetMovieById(int id); 
        Task<IEnumerable<DirectorModel>> GetAllDirectors();  
        Task<int> AddMovie(MovieModel movie);  
    }  
}