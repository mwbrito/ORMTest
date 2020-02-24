namespace WebApiWithEntity.Models
{
    public class MovieModel
    {  
        public int Id { get; set; }  
        public string Name { get; set; }
        public int DirectorId { get; set; }
        public DirectorModel Director { get; set; }  
        public short ReleaseYear { get; set; }  
    }
}