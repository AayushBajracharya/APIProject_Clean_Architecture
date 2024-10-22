using APIProject.Command;
using APIProject.Models;
using APIProject.Repo;

namespace APIProject.Handler
{
    public class CreateProductCommandHandler
    {
        private readonly ICourseRepo _courseRepository;

        public CreateProductCommandHandler(ICourseRepo courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public void Handle(CreateProductCommand command)
        {
            var course = new Course
            {
                Name = command.Name,
                Description = command.Description,
                ProductImage = command.ProductImage,
                ImagePath = command.ImagePath
            };

            _courseRepository.AddCourse(course);  // Assuming AddProduct is a synchronous method
        }
    }

}
