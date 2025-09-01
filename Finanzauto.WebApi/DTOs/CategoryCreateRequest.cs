namespace Finanzauto.WebApi.DTOs
{
    public class CategoryCreateRequest
    {
        public string CategoryName { get; set; } = null!;
        public string? Description { get; set; }
        public IFormFile? PictureFile { get; set; }
    }
}
