using ApiProjectCamp.WebApi.Context;
using ApiProjectCamp.WebApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjectCamp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeTasksController : ControllerBase
    {
        private readonly ApiContext _apiContext;

        public EmployeeTasksController(ApiContext apiContext)
        {
            _apiContext = apiContext;
        }

        [HttpGet]
        public IActionResult ListEmployeeTasks()
        {
            List<EmployeeTask> EmployeeTasks = _apiContext.EmployeeTasks.ToList();
            return Ok(EmployeeTasks);
        }

        [HttpPost]
        public IActionResult CreateTestimonail(EmployeeTask EmployeeTask)
        {
            _apiContext.EmployeeTasks.Add(EmployeeTask);
            _apiContext.SaveChanges();
            return Ok("EmployeeTask added successfully!");
        }

        [HttpDelete]
        public IActionResult DeleteEmployeeTask(int id)
        {
            EmployeeTask EmployeeTaskToDelete = _apiContext.EmployeeTasks.Find(id);
            _apiContext.EmployeeTasks.Remove(EmployeeTaskToDelete);
            _apiContext.SaveChanges();
            return Ok("EmployeeTask deleted successfully!");
        }

        [HttpPut]
        public IActionResult UpdateEmployeeTask(EmployeeTask EmployeeTask)
        {
            _apiContext.Update(EmployeeTask);
            _apiContext.SaveChanges();
            return Ok("EmployeeTask updated successfully!");
        }

        [HttpGet("GetEmployeeTaskById")]
        public IActionResult GetEmployeeTaskById(int id)
        {
            EmployeeTask EmployeeTask = _apiContext.EmployeeTasks.Find(id);
            return Ok(EmployeeTask);
        }
    }
}
