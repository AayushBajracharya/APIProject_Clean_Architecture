using APIProject.Handler;
using APIProject.Models;
using APIProject.Repo;

namespace APIProject.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepo _repo;
        private readonly IErrorHandlingService<string> _errorHandlingService;

        public CourseService(ICourseRepo repo, IErrorHandlingService<string> errorHandlingService)
        {
            _repo = repo;
            _errorHandlingService = errorHandlingService;
        }

        public IEnumerable<Course> GetAllCourses()
        {
            try
            {
                return _repo.GetAllCourses();
            }
            catch (Exception ex)
            {
                _errorHandlingService.SetError(ex.Message);  // Log the error
                throw;  // Rethrow the exception to be handled by the controller if necessary
            }
        }

        public Course GetCourseById(int id)
        {
            try
            {
                return _repo.GetCourseById(id);
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
                _repo.AddCourse(course);
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
                _repo.UpdateCourse(id, course);
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
                _repo.DeleteCourse(id);
            }
            catch (Exception ex)
            {
                _errorHandlingService.SetError(ex.Message);  // Log the error
                throw;  // Rethrow the exception
            }
        }
    }
}
