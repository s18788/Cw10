using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Cw10.Models;
using System;

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
        public IActionResult ModifyStudent(string id, string fname, string name, DateTime bday, int idEnroll)
        {

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(fname) || string.IsNullOrEmpty(name))
            {
                return NotFound();
            }

            Student exists = _context.Find<Student>(id);
            if (exists == null)
            {
                return NotFound("Nie istnieje taki student");
            }

            exists.FirstName = fname;
            exists.LastName = name;
            exists.BirthDate = bday;
            exists.IdEnrollment = idEnroll;

            _context.Update(exists);
            _context.SaveChanges();

            return Ok("Zmodyfikowano studenta");
        }

        [HttpDelete]
        public IActionResult DeleteStudent(string IndexNumber)
        {

            if (IndexNumber != null)
            {
                Student exists = _context.Find<Student>(IndexNumber);
                if (exists != null)
                {
                    _context.Remove<Student>(exists);
                    _context.SaveChanges();
                    return Ok("usunięto studenta");
                }
            }

            return NotFound();
        }

        [Route("api/enrollments")]
        [HttpPost]
        public IActionResult EnrollStudent(string id, string fname, string name, DateTime bday, string studiesName)
        {

            if (id == null || string.IsNullOrEmpty(fname) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(studiesName))
            {
                return BadRequest();
            }

            Student student = _context.Find<Student>(id);

            if (student != null)
            {
                return NotFound("Student już istnieje");
            }



            var studies = _context.Studies.Where(s => s.Name.Equals(studiesName));

            var idStudy = studies.Select(x => x.IdStudy);

            if (studies == null)
            {
                return NotFound("nie ma takiego kierunku");
            }


            var maxEnrollment = _context.Enrollment.Max(x => x.IdEnrollment);
            Enrollment enrollment = new Enrollment
            {
                IdEnrollment = maxEnrollment + 1,
                Semester = 1,
                IdStudy = 1,
                StartDate = new DateTime(2020, 10, 1)
            };

            student = new Student
            {
                IndexNumber = id,
                FirstName = fname,
                LastName = name,
                BirthDate = bday,
                IdEnrollment = maxEnrollment + 1

            };

            _context.Add<Enrollment>(enrollment);
            _context.Add<Student>(student);
            _context.SaveChanges();

            return Ok("zrekrutowano studenta");
        }

        [Route("api/enrollments/promotions")]
        [HttpPost]
        public IActionResult PromoteStudents(string studies, int semester)
        {

            var res = _context.Studies.Where(x => x.Name.Equals(studies));

            if (res == null)
            {
                return NotFound("nie ma takiego kierunku");
            }

            var idStudy = res.Select(x => x.IdStudy);

            var toupdate = _context.Enrollment.Where(x => x.Semester == semester).Where(x => x.IdStudy == 2).ToList();

            foreach (var e in toupdate)
            {
                e.Semester += 1;
                _context.Enrollment.Update(e);
            }
            return Ok("Wypromowano studentów");
        }


    }
}
