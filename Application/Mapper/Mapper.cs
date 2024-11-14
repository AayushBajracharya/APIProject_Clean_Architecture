using APIProject.DTO;
using APIProject.Models;

namespace APIProject.Mapper
{
    public class Mapper : IMapper
    {
        public CourseDto MapToDto(Course course)
        {
            return new CourseDto
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description
            };
        }

        public Course MapToEntity(CourseDto courseDto)
        {
            return new Course
            {
                Id = courseDto.Id,
                Name = courseDto.Name,
                Description = courseDto.Description
            };
        }
    }

}
