using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Cw10.Models;

namespace Cw10.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly SampleDbContext _context;

        public StudentsController(SampleDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetStudentsList()
        {
            List<Student> studentsList = _context.Student.ToList();

            return Ok(studentsList);
        }

        [HttpPost]
        public IActionResult ModifyStudent([Bind("IndexNumber,FirstName,LastName,BirthDate,IdEnrollment")] Student student)
        {

            Student exists = _context.Find<Student>(student.IndexNumber);
            if (exists != null)
            {
                _context.Update<Student>(student);
                _context.SaveChanges();
                return Ok("zmodyfikowano studenta");
            }

            return NotFound();
        }

        [HttpDelete]
        public IActionResult DeleteStudent([Bind("IndexNumber,FirstName,LastName,BirthDate,IdEnrollment")] Student student)
        {

            Student exists = _context.Find<Student>(student.IndexNumber);
            if (exists != null)
            {
                _context.Remove<Student>(student);
                _context.SaveChanges();
                return Ok("usunięto studenta");
            }

            return NotFound();
        }

        [Route("api/enrollstudent")]
        [HttpPost]
        public IActionResult EnrollStudent(Student student, string studies)
        {

            /*   if (student.IndexNumber == null
                     || string.IsNullOrEmpty(student.FirstName)
                     || string.IsNullOrEmpty(student.LastName)
                     || string.IsNullOrEmpty(studies))
               {
                   return BadRequest();
               }

               if (_context.Find<Student>(student) != null)
               {
                   if (_context.Find<Studies>(studies)) {


                       _context.Add<Student>(student);
                       _context.Add<Enrollment>(student.IdEnrollment);
                       _context.SaveChanges();
                   }
               }*/

            return NotFound();
        }

        [Route("api/promotestudents")]
        [HttpPost]
        public IActionResult PromoteStudents()
        {
            return NotFound();
        }


    }
}
