using API_Example_ASP.Models;

namespace API_Example_ASP.Services;

public class ProductService : IProductService
{
    private readonly List<Product> _products = [];
    private int _nextId = 1;

    public Product AddProduct(CreateProductRequest request)
    {
        var product = new Product
        {
            Id = _nextId++,
            Name = request.Name,
            Price = request.Price
        };

        _products.Add(product);
        return product;
    }

    public Product? GetProductById(int id) =>
        _products.FirstOrDefault(p => p.Id == id);
}
