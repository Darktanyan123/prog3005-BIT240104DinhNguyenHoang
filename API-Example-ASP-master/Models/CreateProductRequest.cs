using System.ComponentModel.DataAnnotations;

namespace API_Example_ASP.Models;

public class CreateProductRequest
{
    [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
    [MinLength(3, ErrorMessage = "Tên sản phẩm phải có ít nhất 3 ký tự.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Giá sản phẩm là bắt buộc.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn 0.")]
    public decimal Price { get; set; }
}
