using Consuming_API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Consuming_API.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger,  IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("myApi");
        }

        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> showData()
        {
            var response = await _httpClient.GetAsync("api/VillaAPI");
            response.EnsureSuccessStatusCode();

            var jason = await response.Content.ReadAsStringAsync();
            var villas = JsonSerializer.Deserialize<List<villaDTO>>(jason, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(villas);
        }

        public async Task<IActionResult> GetVillaById(int id)
        {
            var response = await _httpClient.GetAsync($"api/VillaAPI/{id}");

            if (!response.IsSuccessStatusCode)
            {
                    return NotFound("Villa not found!");
            }

            var jsonstring = await response.Content.ReadAsStringAsync();
            var villa = JsonSerializer.Deserialize<villaDTO>(jsonstring, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(villa);
        }

        [HttpGet]
        public async Task<IActionResult> createVilla()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> createVilla(villaDTO villaDTOparameter)
        {
            if (!ModelState.IsValid)
            {
                return View(villaDTOparameter);
            }

            var response = await _httpClient.PostAsJsonAsync("api/VillaAPI", villaDTOparameter);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ShowData");
            }

            else
            {
                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, error);
            }
            return View(villaDTOparameter);
        }


        public async Task<IActionResult> DeleteVilla(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }

            var response = await _httpClient.DeleteAsync($"api/VillaAPI/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("showdata");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, error);
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateVilla(int id)
        {
            var response = await _httpClient.GetAsync($"api/VillaAPI/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound("Villa not found!");
            }

            var jsonstring = await response.Content.ReadAsStringAsync();
            var villa = JsonSerializer.Deserialize<villaDTO>(jsonstring, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(villa);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateVilla(int id, villaDTO villaDTOparameter)
        {
            if (id == 0 || villaDTOparameter == null || id != villaDTOparameter.VillaId)
            {
                return BadRequest();
            }

            var response = await _httpClient.PutAsJsonAsync($"api/VillaAPI/{id}", villaDTOparameter);


            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("showdata"); 
            }
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
