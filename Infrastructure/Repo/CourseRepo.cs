using APIProject.Data;
using APIProject.Handler;
using APIProject.Models;

namespace APIProject.Repo
{
    public class CourseRepo : ICourseRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IErrorHandlingService<string> _errorHandlingService;

        public CourseRepo(ApplicationDbContext context, IErrorHandlingService<string> errorHandlingService)
        {
            _context = context;
            _errorHandlingService = errorHandlingService;
        }

        public IEnumerable<Course> GetAllCourses()
        {
            try
            {
                return _context.Courses.ToList();
            }
            catch (Exception ex)
            {
                _errorHandlingService.SetError(ex.Message);  // Log the error
                throw;  // Rethrow the exception to be handled at a higher level (e.g., service or controller)
            }
        }

        public Course GetCourseById(int id)
        {
            try
            {
                return _context.Courses.Find(id);
            }
            catch (Exception ex)
            {
                _errorHandlingService.SetError(ex.Message);  // Log the error
                throw;  // Rethrow the exception
            }
        }

        public void AddCourse(Course course)
        {
            try
            {
                _context.Courses.Add(course);
                Save();
            }
            catch (Exception ex)
            {
                _errorHandlingService.SetError(ex.Message);  // Log the error
                throw;  // Rethrow the exception
            }
        }

        public void UpdateCourse(int id, Course course)
        {
            try
            {
                var context = _context.Courses.Find(id);
                if (context != null)
                {
                    context.Name = course.Name;
                    context.Description = course.Description;
                    Save();
                }
                else
                {
                    throw new KeyNotFoundException("Course not found");
                }
            }
            catch (Exception ex)
            {
                _errorHandlingService.SetError(ex.Message);  // Log the error
                throw;  // Rethrow the exception
            }
        }

        public void DeleteCourse(int id)
        {
            try
            {
                var course = _context.Courses.Find(id);
                if (course != null)
                {
                    _context.Courses.Remove(course);
                    Save();
                }
                else
                {
                    throw new KeyNotFoundException("Course not found");
                }
            }
            catch (Exception ex)
            {
                _errorHandlingService.SetError(ex.Message);  // Log the error
                throw;  // Rethrow the exception
            }
        }

        // Save changes to the database
        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _errorHandlingService.SetError(ex.Message);  // Log the error
                throw;  // Rethrow the exception
            }
        }
    }
}
