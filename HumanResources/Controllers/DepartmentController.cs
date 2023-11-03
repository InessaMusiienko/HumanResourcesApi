using HumanResources.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HumanResources.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7175/api");
        private readonly HttpClient _client;

        public DepartmentController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult GetAllDepartments(string SearchString="")
        {
            List<DepartmentViewModel> departmentList = new List<DepartmentViewModel>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + $"/departments/getdepartments?searchstring={SearchString}").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                departmentList = JsonConvert.DeserializeObject<List<DepartmentViewModel>>(data);
            }

            return View(departmentList);
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            try
            {
                List<AllEmployeeViewModel> employees = new List<AllEmployeeViewModel>();
                HttpResponseMessage response = _client
                    .GetAsync(_client.BaseAddress + $"/employees/getdepartmentemployees/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    employees = JsonConvert.DeserializeObject<List<AllEmployeeViewModel>>(data);
                }
                return View(employees);
            }
            catch (Exception)
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateDepartment(DepartmentViewModel model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = _client
                    .PostAsync(_client.BaseAddress + "/departments/postdepartment", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Department Created.";
                    return RedirectToAction("GetAllDepartments");
                }
            }
            catch(Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
            //return View();
            return this.RedirectToAction("GetAllDepartments");
        }

        //[HttpGet]
        //public IActionResult Delete(string departmentName)
        //{
        //    try
        //    {
        //        DepartmentViewModel dep = new DepartmentViewModel();
        //        HttpResponseMessage responseMessage = _client
        //            .GetAsync(_client.BaseAddress + "/departments/getdepartment/" + departmentName).Result;

        //        if (responseMessage.IsSuccessStatusCode)
        //        {
        //            string data = responseMessage.Content.ReadAsStringAsync().Result;
        //            dep = JsonConvert.DeserializeObject<DepartmentViewModel>(data);
        //        }
        //        return View(dep);
        //    }
        //    catch (Exception ex)
        //    {

        //        TempData["errorMessage"] = ex.Message;
        //        return View();
        //    }            
        //}

        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeleteConfirmed(string departmentName)
        //{
        //    HttpResponseMessage response = _client
        //        .DeleteAsync(_client.BaseAddress + "/departments/DeleteDepartment" + departmentName).Result;

        //    try
        //    {
        //        if (response.IsSuccessStatusCode)
        //        {
        //            TempData["successMessage"] = "Department Deleated.";
        //            return RedirectToAction("GetAllDepartments");
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        TempData["errorMessage"] = ex.Message;
        //        return View();  
        //    }
        //    return View();
        //}
    }
}
