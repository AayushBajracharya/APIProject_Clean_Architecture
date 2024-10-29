using System.Threading;
using System.Threading.Tasks;
using APIProject.Command;
using APIProject.Models;
using APIProject.Repo;
using MediatR;

namespace APIProject.Handler
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Course>
    {
        private readonly ICourseRepo _courseRepository;

        public CreateProductCommandHandler(ICourseRepo courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<Course> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var course = new Course
            {
                Name = command.Name,
                Description = command.Description,
                ProductImage = command.ProductImage
            };

            _courseRepository.AddCourse(course);

            return course; // Return the newly created course
        }
    }
}
