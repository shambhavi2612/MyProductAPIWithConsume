using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using ConsumeProducts.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace YourMVCAppName.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IHttpClientFactory httpClientFactory, ILogger<HomeController> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7015/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            _logger.LogInformation("Registering user: {Username}", user.Username);

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Users/register", user);
            response.EnsureSuccessStatusCode();

            _logger.LogInformation("User registered successfully: {Username}", user.Username);
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            _logger.LogInformation("Login attempt for user: {Username}", user.Username);

            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Users/login", user);
                string responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Login successful for user: {Username}", user.Username);
                    return RedirectToAction("Index");
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _logger.LogWarning("Invalid username or password for user: {Username}", user.Username);
                    ModelState.AddModelError(string.Empty, "Invalid username or password.");
                    return View(user);
                }
                else
                {
                    _logger.LogError("Login error for user: {Username}. Response: {Response}", user.Username, responseContent);
                    ModelState.AddModelError(string.Empty, "An error occurred during login.");
                    return View(user);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login for user: {Username}", user.Username);
                ModelState.AddModelError(string.Empty, $"An error occurred during login: {ex.Message}");
                return View(user);
            }
        }

       

        public async Task<IActionResult> Products()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("api/Products");
                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    var products = JsonSerializer.Deserialize<List<Product>>(responseData, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return View(products);
                }
                else
                {
                    _logger.LogError("Error fetching products. Response: {Response}", await response.Content.ReadAsStringAsync());
                    ModelState.AddModelError(string.Empty, "Error fetching products.");
                    return View(new List<Product>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching products");
                ModelState.AddModelError(string.Empty, $"Error fetching products: {ex.Message}");
                return View(new List<Product>());
            }
        }
    }
}
