using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.DepartmentDtos;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : AbstractController
    {
        public DepartmentController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }

        [HttpPost]
        public IActionResult Crete(CreateDepartmentDto departmentDto)
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
                .Include(c => c.Users)
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
        [HttpPatch]
        public IActionResult UpdateDepartment(UpdateDepartmentDto updateDepartmentDto)
        {
            var department = this.Context.Departments.Find(updateDepartmentDto.Id);
            var similerDepartment = this.Context.Departments.Where(c => c.Id != updateDepartmentDto.Id && c.Name == updateDepartmentDto.Name).FirstOrDefault();
            if (similerDepartment != null)
                return Conflict();
            department.Name = updateDepartmentDto.Name;
            this.Context.Update(department);
            this.Context.SaveChanges();
            return Ok();
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteDepartment(int id)
        {
            var departmnet= this.Context.Departments.Find(id);
            if (departmnet == null || departmnet.Users.Count() > 0)
                return Conflict();
            this.Context.Departments.Remove(departmnet);
            this.Context.SaveChanges();
            return Ok();
        }
    }
}