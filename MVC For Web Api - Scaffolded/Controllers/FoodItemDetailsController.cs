using Microsoft.AspNetCore.Mvc;
using MVC_For_Web_Api___Scaffolded.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MVC_For_Web_Api___Scaffolded.Controllers
{
    public class FoodItemDetailsController : Controller
    {
        public async Task<IActionResult> ViewFoodItems()
        {
            List<FoodItemDetail> foodItemsFromAPI = new List<FoodItemDetail>();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("https://localhost:7266/api/FoodItemDetails\r\n");

                if (Res.IsSuccessStatusCode)
                {
                    var apiResponse = Res.Content.ReadAsStringAsync().Result;

                    foodItemsFromAPI = JsonConvert.DeserializeObject<List<FoodItemDetail>>(apiResponse);

                }
                return View(foodItemsFromAPI);
            }
        }

        public async Task<IActionResult> ViewDetails(string id)
        {
            FoodItemDetail foodItemFromApi = new FoodItemDetail();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7266/api/FoodItemDetails/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    foodItemFromApi = JsonConvert.DeserializeObject<FoodItemDetail>(apiResponse);
                }
            }
            return View(foodItemFromApi);
        }
        public IActionResult AddFoodItems()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFoodItems(FoodItemDetail foodItemFromMVC)
        {
            FoodItemDetail foodItemFromAPI = new FoodItemDetail();
            using (var httpClient = new HttpClient())
            {
                StringContent valuesToAdd = new StringContent(JsonConvert.SerializeObject(foodItemFromMVC),
              Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("https://localhost:7266/api/FoodItemDetails/", valuesToAdd))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    foodItemFromAPI = JsonConvert.DeserializeObject<FoodItemDetail>(apiResponse);
                }
            }
            return RedirectToAction("ViewFoodItems");
        }

        public async Task<IActionResult> EditFoodDetail(string id)
        {
            TempData["foodId"] = id;
            FoodItemDetail foodItemFromApi = new FoodItemDetail();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7266/api/FoodItemDetails/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    foodItemFromApi = JsonConvert.DeserializeObject<FoodItemDetail>(apiResponse);
                }
            }
            return View(foodItemFromApi);
        }

        [HttpPost]
        public async Task<IActionResult> EditFoodDetail(FoodItemDetail foodItemFromMVC)
        {
            FoodItemDetail foodItemFromAPI = new FoodItemDetail();
            string foodId = TempData["foodId"].ToString();
            using (var httpClient = new HttpClient())
            {
                string id = foodItemFromMVC.FoodId;
                StringContent valueToUpdate = new StringContent(JsonConvert.SerializeObject(foodItemFromMVC)
         , Encoding.UTF8, "application/json");
                using (var response = await httpClient.PutAsync("https://localhost:7266/api/FoodItemDetails/" + foodId, valueToUpdate))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    foodItemFromAPI = JsonConvert.DeserializeObject<FoodItemDetail>(apiResponse);
                }
            }
            return RedirectToAction("ViewFoodItems");
        }

        public async Task<IActionResult> DeleteFoodItem(string id)
        {
            TempData["foodId"] = id;
            FoodItemDetail foodItemFromApi = new FoodItemDetail();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:7266/api/FoodItemDetails/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    foodItemFromApi = JsonConvert.DeserializeObject<FoodItemDetail>(apiResponse);
                }
            }
            return View(foodItemFromApi);
        }

        [HttpPost]
        [ActionName("DeleteFoodItem")]
        public async Task<IActionResult> DeleteFoodItemConfirmed(FoodItemDetail foodItemFromMVC)
        {

            string foodId = TempData["foodId"].ToString();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync("https://localhost:7266/api/FoodItemDetails/" + foodId))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("ViewFoodItems");
        }
    }
}
