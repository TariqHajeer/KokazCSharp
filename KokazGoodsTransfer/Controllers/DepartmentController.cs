using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KokazGoodsTransfer.Dtos.Department;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        KokazContext Context;
        public DepartmentController(KokazContext context)
        {
            this.Context = context;
        }
        [HttpPost]
        public IActionResult Crete(CreateDepartmentDto  departmentDto)
        {
            var similer = Context.Departments.Where(c => c.Name == departmentDto.Name).FirstOrDefault();
            if (similer != null)
                return Conflict();
            var department = new Department()
            {
                Name = departmentDto.Name
            };
            Context.Add(department);
            Context.SaveChanges();
            DepartmentDto departmentDto1 = new DepartmentDto()
            {
                Id = department.Id,
                Name = department.Name,
                UserCount = 0
            };
            return Ok(departmentDto1);
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var departments = this.Context.Departments
                .Include(c=>c.Users)
                .ToList();
            List<DepartmentDto> departmentDtos = new List<DepartmentDto>();
            foreach (var item in departments)
            {
                departmentDtos.Add(new DepartmentDto()
                {
                    Id = item.Id,
                    Name = item.Name,
                    UserCount = item.Users.Count()
                });
            }
            return Ok(departmentDtos);
        }

    }
}