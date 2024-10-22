using APIProject.DTO;
using APIProject.Models;

namespace APIProject.Mapper
{
    public interface IMapper
    {
        CourseDto MapToDto(Course course);
        Course MapToEntity(CourseDto courseDto);

    }
}
