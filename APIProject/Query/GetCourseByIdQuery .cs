using APIProject.Models;
using MediatR;

namespace APIProject.Query
{
    public class GetCourseByIdQuery : IRequest<Course>
    {
        public int Id { get; set; }

        public GetCourseByIdQuery(int id)
        {
            Id = id;
        }
    }

}
