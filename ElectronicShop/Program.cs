using ElectronicShop.Context;
using ElectronicShop.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;

ShopDbContext shopDbContext = new ShopDbContext();







Menu();
int request = int.Parse(Console.ReadLine());

int ProductChoose = 0;
int CategoryChoose = 0;


while (request != 0)
{
    if (request > 2)
    {
        Console.WriteLine("\x1b[91mPlease make the right choice!\x1b[0m");
    }


    switch (request)
    {
        case 1:
            ProductMenu();
            ProductChoose = int.Parse(Console.ReadLine());

            while (ProductChoose != 0)
            {
                if (ProductChoose > 5)
                {
                    Console.WriteLine("\x1b[91mPlease make the right choice!\x1b[0m");
                }
                switch (ProductChoose)
                {
                    case 1:
                        ProductCreate();
                        break;
                    case 2:
                        ProductGetAll();
                        break;
                    case 3:
                        ProductGetById();
                        break;
                    case 4:
                        ProductUpdate();
                        break;
                    case 5:
                        ProductDelete();
                        break;
                    default:
                        break;
                }

                ProductMenu();
                ProductChoose = int.Parse(Console.ReadLine());
            }
            break;


        case 2:
            CategoryMenu();
            CategoryChoose = int.Parse(Console.ReadLine());

            while (CategoryChoose != 0)
            {
                if (CategoryChoose > 5)
                {
                    Console.WriteLine("\x1b[91mPlease make the right choice!\x1b[0m");
                }
                switch (CategoryChoose)
                {

                    case 1:
                        CategoryCreate();
                        break;
                    case 2:
                        CategoryGetAll();
                        break;
                    case 3:
                        CategoryGetById();
                        break;
                    case 4:
                        CategoryUpdate();
                        break;
                    case 5:
                        ProductDelete();
                        break;
                    default:
                        break;
                }

                CategoryMenu();
                CategoryChoose = int.Parse(Console.ReadLine());
            }
            break;

        default:
            break;
    }
    Menu();
    request = int.Parse(Console.ReadLine());
}
Console.WriteLine("Exiting the program...");








void Menu()
{
    Console.WriteLine("1.Product");
    Console.WriteLine("2.Category");
    Console.WriteLine("0.Close");
}

void ProductMenu()
{
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.WriteLine("Add a process");
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("1.Create Product");
    Console.WriteLine("2.GetAll Product");
    Console.WriteLine("3.Get By Id Product");
    Console.WriteLine("4.Update Product");
    Console.WriteLine("5.Delete Product");
    Console.WriteLine("0.Close");
    Console.ResetColor();
}

void CategoryMenu()
{
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.WriteLine("Add a process");
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("1.Create Category");
    Console.WriteLine("2.GetAll Category");
    Console.WriteLine("3.Get By Id Category");
    Console.WriteLine("4.Update Category");
    Console.WriteLine("5.Delete Category");
    Console.WriteLine("0.Close");
    Console.ResetColor();
}







// PRODUCTS

void ProductGetAll()
{
    IQueryable<Product> query = shopDbContext.Product
        .Where(x => !x.IsDeleted)
        .Include(x => x.Category)
        .AsNoTracking()
        .Select(x => new Product
        {
            Id = x.Id,
            Name = x.Name,
            Price = x.Price,
            Category = new Category { Name = x.Category.Name }
        });

    IEnumerable<Product> products = query.ToList();

    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("ALL PRODUCTS:");
    Console.ResetColor();
    foreach (var item in products)
    {
        Console.WriteLine($"ID:\u001b[94m{item.Id}\u001b[0m , Product:\x1b[94m{item.Name}\x1b[0m , Category: \x1b[38;5;22m{item.Category.Name}\x1b[0m Price:  \x1b[32m{item.Price}$\x1b[0m");


    }
}

void ProductGetById()
{
    Console.WriteLine("Enter Id");
    int.TryParse(Console.ReadLine(), out int id);

    Product? query = shopDbContext.Product
        .Where(x => !x.IsDeleted && x.Id == id)
        .Include(x => x.Category)
        .AsNoTracking()
        .Select(
            x => new Product
            {
                Name = x.Name,
                Category = new Category { Name = x.Category.Name }
            }
        )
        .FirstOrDefault();

    if (query != null)
    {
        Console.WriteLine($"Product: {query.Name}, Category: {query.Category.Name}");
    }
    else
    {
        Console.WriteLine($"Product with 'Id {id}' not found.");
    }
}

void ProductUpdate()
{
    Console.WriteLine("Add Id");
    int.TryParse(Console.ReadLine(), out int Id);

    Console.WriteLine("Add New Product");
    string Name = Console.ReadLine();

    Product query = shopDbContext.Product
        .Where(x => x.Id == Id)
        .FirstOrDefault();


    if (query != null)
    {
        query.Name = Name;
        query.CreateAt = DateTime.UtcNow;
        query.UpdateAt = DateTime.UtcNow;
        shopDbContext.SaveChanges();

        Console.WriteLine($"{query.Name}, updated successfully.");

    }
    else
    {
        Console.WriteLine($"Product with Id {Id} not found.");
    }
}

void ProductCreate()
{
    Product product = new Product();

    Console.WriteLine("Write the name of the product");
    string name = Console.ReadLine();

    Console.WriteLine("Add Price");
    int Price = int.Parse(Console.ReadLine());

    Console.WriteLine("Which category does it belong to with the specified \u001b[36m'ID'\u001b[0m?");


    GetAllCategoryForProduct();
    int id = int.Parse(Console.ReadLine());

    if (string.IsNullOrEmpty(name))
    {
        Console.WriteLine("Category name cannot be empty. Operation canceled.");
        return;
    }



    product.Name = name;
    product.Price = Price;
    product.CategoryId = id;
    product.IsDeleted = false;
    product.CreateAt = DateTime.UtcNow;
    product.UpdateAt = DateTime.UtcNow;

    shopDbContext.Product.Add(product);
    shopDbContext.SaveChanges();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Category created successfully.");
    Console.ResetColor();
}

void ProductDelete()
{
    Console.WriteLine("Enter Id to delete:");

    if (int.TryParse(Console.ReadLine(), out int id))
    {
        Product productToDelete = shopDbContext.Product.FirstOrDefault(p => p.Id == id && !p.IsDeleted);

        if (productToDelete != null)
        {
            productToDelete.IsDeleted = true;

            if (shopDbContext.SaveChanges() > 0)
            {
                Console.WriteLine("Product deleted successfully.");
            }
            else
            {
                Console.WriteLine("No changes to save. Deletion failed.");
            }
        }
        else
        {
            Console.WriteLine("Product not found or already deleted. Deletion failed.");
        }
    }
    else
    {
        Console.WriteLine("Invalid input for Id. Deletion failed.");
    }
}


void GetAllCategoryForProduct()
{
    IQueryable<Category> query = shopDbContext.Category
        .Where(x => !x.IsDeleted)
        .AsNoTracking()
        .Select(x => new Category { Id = x.Id, Name = x.Name });

    IEnumerable<Category> categories = query.ToList();


    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("ALL CATAGORIES:");
    Console.ResetColor();
    foreach (Category category in categories)
    {
        Console.WriteLine($"Id: \x1b[94m{category.Id}\x1b[0m , Name: \x1b[94m{category.Name}\x1b[0m ");
    }
}






//CATAGORIES


void CategoryGetAll()
{
    IQueryable<Category> query = shopDbContext.Category
        .Where(x => !x.IsDeleted)
        .AsNoTracking()
        .Select(x => new Category { Id = x.Id, Name = x.Name });

    IEnumerable<Category> categories = query.ToList();


    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("ALL CATAGORIES:");
    Console.ResetColor();
    foreach (Category category in categories)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"\x1b[39mId:\x1b[39m \u001b[32m{category.Id}\u001b[0m , Name: \u001b[38;5;22m{category.Name}\u001b[0m");

        Console.ResetColor();
    }
}

void CategoryGetById()
{
    Console.WriteLine("Enter Id");
    int.TryParse(Console.ReadLine(), out int id);

    Category? category = shopDbContext.Category
        .Where(x => x.Id == id && !x.IsDeleted)
        .Include(x => x.Products)
        .AsNoTracking()
        .FirstOrDefault();



    if (category != null)
    {
        if (category.Products != null)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"' {category.Name} ' category Products:");
            Console.ResetColor();
            foreach (var product in category.Products)
            {
                Console.WriteLine($"- {product.Name}");
            }
        }
        else
        {
            Console.WriteLine("No products found for this category.");
        }
    }
    else
    {
        Console.WriteLine($"Category with Id {id} not found.");
    }
}

void CategoryCreate()
{
    Category category = new Category();

    Console.WriteLine("Write the name of the ' Category '");
    string name = Console.ReadLine();


    category.Name = name;
    category.IsDeleted = false;
    category.CreateAt = DateTime.UtcNow;
    category.UpdateAt = DateTime.UtcNow;

    shopDbContext.Category.Add(category);
    shopDbContext.SaveChanges();
    Console.WriteLine("\x1b[32mCategory created successfully.\x1b[0m");

}


void CategoryUpdate()
{
    Console.WriteLine("Add Id");
    int.TryParse(Console.ReadLine(), out int Id);

    Console.WriteLine("Add New Category");
    string Name = Console.ReadLine();

    Category query = shopDbContext.Category
        .Where(x => x.Id == Id)
        .FirstOrDefault();


    if (query != null)
    {
        query.Name = Name;
        query.CreateAt = DateTime.UtcNow;
        query.UpdateAt = DateTime.UtcNow;
        shopDbContext.SaveChanges();

        Console.WriteLine($"{query.Name}, updated successfully.");

    }
    else
    {
        Console.WriteLine($"Category with Id {Id} not found.");
    }
}


void CategoryDelete()
{
    Console.WriteLine("Enter Id to delete:");

    if (int.TryParse(Console.ReadLine(), out int id))
    {
        Category CategoryToDelete = shopDbContext.Category.FirstOrDefault(p => p.Id == id && !p.IsDeleted);

        if (CategoryToDelete != null)
        {
            CategoryToDelete.IsDeleted = true;

            if (shopDbContext.SaveChanges() > 0)
            {
                Console.WriteLine("Category deleted successfully.");
            }
            else
            {
                Console.WriteLine("No changes to save. Deletion failed.");
            }
        }
        else
        {
            Console.WriteLine("Category not found or already deleted. Deletion failed.");
        }
    }
    else
    {
        Console.WriteLine("Invalid input for Id. Deletion failed.");
    }
}