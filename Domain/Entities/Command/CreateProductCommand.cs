using APIProject.Models;
using MediatR;

namespace APIProject.Command
{
    public class CreateProductCommand : IRequest<Course>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProductImage { get; set; }
    }
}
