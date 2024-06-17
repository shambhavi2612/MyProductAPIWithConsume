//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using ProductsApp.Models;
//using System.Linq;

//namespace ProductsApp.Controllers
//{
// [Route("api/[controller]")]
// [ApiController]
// public class ProductsController : ControllerBase
// {
// private readonly ApplicationDbContext _context;

// public ProductsController(ApplicationDbContext context)
// {
// _context = context;
// }

// [HttpGet]
// public IActionResult GetProducts()
// {
// var products = _context.Products.ToList();
// return Ok(products);
// }

// [HttpPost]
// public IActionResult AddProduct([FromBody] Product product)
// {
// if (product == null)
// {
// return BadRequest("Product is null.");
// }

// _context.Products.Add(product);
// _context.SaveChanges();
// return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
// }

// [HttpGet("{id}")]
// public IActionResult GetProductById(int id)
// {
// var product = _context.Products.FirstOrDefault(p => p.Id == id);
// if (product == null)
// {
// return NotFound();
// }

// return Ok(product);
// }
// }
//}
//2

//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;

//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Hosting;
//using ProductsApp.Models;
//using System.Linq;

//namespace ProductsApp.Controllers
//{
// [Route("api/[controller]")]
// [ApiController]
// [Authorize]
// public class ProductsController : ControllerBase
// {
// private readonly ApplicationDbContext _context;

// public ProductsController(ApplicationDbContext context)
// {
// _context = context;
// }

// [HttpGet]
// public IActionResult GetProducts()
// {
// var products = _context.Products.ToList();
// return Ok(products);
// }

// [HttpPost]
// [Authorize]
// public IActionResult AddProduct([FromBody] Product product)
// {
// if (!ModelState.IsValid)
// return BadRequest(ModelState);

// _context.Products.Add(product);
// _context.SaveChanges();
// return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
// }

// [HttpGet("{id}")]
// public IActionResult GetProductById(int id)
// {
// var product = _context.Products.FirstOrDefault(p => p.Id == id);
// if (product == null)
// {
// return NotFound();
// }

// return Ok(product);
// }
// }
//}

//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using ProductsApp.Models;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;

//namespace ProductsApp.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ProductsController : ControllerBase
//    {
//        private readonly ApplicationDbContext _context;
//        private readonly IWebHostEnvironment _hostEnvironment;



//        public ProductsController(IWebHostEnvironment hostEnvironment)
//        {
//            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
//        }

//        [HttpGet]
//        public IActionResult GetProducts()
//        {
//            var products = _context.Products.ToList();
//            return Ok(products);
//        }







//        [HttpPost]
//        [Authorize]
//        public async Task<IActionResult> AddProduct([FromForm] string name, [FromForm] string description, [FromForm] decimal price, IFormFile photo)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            var product = new Product
//            {
//                Name = name,
//                Description = description,
//                Price = price
//            };

//            if (photo != null)
//            {
//                if (_hostEnvironment.WebRootPath != null)
//                {
//                    var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads");
//                    Directory.CreateDirectory(uploadsFolder); // Ensure the uploads folder exists
//                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
//                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

//                    using (var stream = new FileStream(filePath, FileMode.Create))
//                    {
//                        await photo.CopyToAsync(stream);
//                    }

//                    product.PhotoUrl = Path.Combine("uploads", uniqueFileName);
//                }
//                else
//                {
//                    // Log or handle the case when WebRootPath is null
//                    return BadRequest("WebRootPath is null");
//                }
//            }

//            _context.Products.Add(product);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
//        }


//        [HttpGet("{id}")]
//        public IActionResult GetProductById(int id)
//        {
//            var product = _context.Products.FirstOrDefault(p => p.Id == id);
//            if (product == null)
//            {
//                return NotFound();
//            }

//            return Ok(product);
//        }
//    }
//}


// myyyyyyyyyyyy main product controller code 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using ProductsApp.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ProductsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductsController(IWebHostEnvironment hostEnvironment, ApplicationDbContext context)
        {
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        
        [HttpGet]
        public IActionResult GetProducts()
        {
            var products = _context.Products.ToList();
            return Ok(products);
        }



        

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddProduct([FromForm] string name, [FromForm] string description, [FromForm] decimal price, IFormFile photo)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = new Product
            {
                Name = name,
                Description = description,
                Price = price
            };

            if (photo != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await photo.CopyToAsync(memoryStream);
                    product.PhotoData = memoryStream.ToArray();
                }
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }


        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
    }
}







