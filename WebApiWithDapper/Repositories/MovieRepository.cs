using Dapper;
using Microsoft.Data.SqlClient;
using WebApiWithDapper.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiWithDapper.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ConnectionString _connectionString;

        public MovieRepository(ConnectionString connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<MovieModel>> GetAllMovies()
        {
            const string query = @"SELECT m.Id, m.Name,d.Name AS DirectorName, m.ReleaseYear  
                                    FROM Movies m  
                                    INNER JOIN Directors d  
                                    ON m.DirectorId = d.Id";

            using (var conn = new SqlConnection(_connectionString.Value))
            {
                var result = await conn.QueryAsync<MovieModel>(query);
                return result;
            }
        }

        public async Task<MovieModel> GetMovieById(int id)
        {
            const string query = @"SELECT m.Id  
                                        , m.Name  
                                        ,d.Name AS DirectorName  
                                        , m.ReleaseYear  
                                    FROM Movies m  
                                    INNER JOIN Directors d  
                                    ON m.DirectorId = d.Id  
                                    WHERE m.Id = @Id";

            using (var conn = new SqlConnection(_connectionString.Value))
            {
                var result = await conn.QueryFirstOrDefaultAsync<MovieModel>(query, new { Id = id });
                return result;
            }
        }

        public async Task<IEnumerable<DirectorModel>> GetAllDirectors()
        {
            const string query = @"SELECT d.Id  
                                        , d.Name  
                                        , m.DirectorId  
                                        , m.Id  
                                        , m.Name MovieName  
                                        , m.ReleaseYear  
                                    FROM dbo.Directors d  
                                    INNER JOIN dbo.Movies m  
                                    ON d.Id = m.DirectorId";

            using (var conn = new SqlConnection(_connectionString.Value))
            {
                var directorDictionary = new Dictionary<int, DirectorModel>();

                var result = await conn.QueryAsync<DirectorModel, DirectorMovie, DirectorModel>(
                    query,
                    (dir, mov) =>
                    {
                        if (!directorDictionary.TryGetValue(dir.Id, out DirectorModel director))
                        {
                            director = dir;
                            director.Movies = new List<DirectorMovie>();
                            directorDictionary.Add(director.Id, director);
                        }
                        director.Movies.Add(mov);
                        return director;
                    },
                    splitOn: "DirectorId");

                return result.Distinct();
            }
        }
        public async Task<int> AddMovie(CreateMovieModel movie)
        {
            const string query = @"INSERT INTO dbo.Movies ([Name], [DirectorId], [ReleaseYear]) VALUES(@Name, @DirectorId, @ReleaseYear)";

            using (var conn = new SqlConnection(_connectionString.Value))
            {
                var result = await conn.ExecuteAsync(
                    query,
                    new { Name = movie.Name, DirectorId = movie.DirectorId, ReleaseYear = movie.ReleaseYear });
                return result;
            }
        }
    }
}