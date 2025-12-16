namespace DataAccessLibrary.Models
{
    // Minimal placeholder for FaceImage referenced by ApplicationDbContext
    public class FaceImage
    {
        public int Id { get; set; }
        public string? FileName { get; set; }
        public byte[]? Data { get; set; }
    }
}
