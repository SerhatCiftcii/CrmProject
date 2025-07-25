// CrmProject.Application/DTOs/Product/UpdateProductDto.cs

namespace CrmProject.Application.DTOs.ProductDtos
{
    public class UpdateProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
