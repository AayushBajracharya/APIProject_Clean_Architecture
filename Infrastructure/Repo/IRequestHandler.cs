using APIProject.Models;

namespace APIProject.Repo
{
    public interface IRequestHandler
    {
        IEnumerable<Course> GetAllCourses();
        Course GetCourseById(int id);
        void AddCourse(Course value);
        void UpdateCourse(int id, Course course);
        void DeleteCourse(int id);

    }
}
