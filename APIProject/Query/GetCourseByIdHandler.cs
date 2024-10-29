using APIProject.Models;
using APIProject.Repo;
using MediatR;

namespace APIProject.Query
{

    public class GetCourseByIdHandler : IRequestHandler<GetCourseByIdQuery, Course>
    {
        private readonly ICourseRepo _courseRepo;

        public GetCourseByIdHandler(ICourseRepo courseRepo)
        {
            _courseRepo = courseRepo;
        }

        public async Task<Course> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
        {
            return _courseRepo.GetCourseById(request.Id);
        }
    }

}
