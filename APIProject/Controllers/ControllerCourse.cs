using Microsoft.AspNetCore.Mvc;
using APIProject.Services;
using APIProject.Models;
using APIProject.Handler;
using APIProject.Command;
using APIProject.Mapper;

namespace APIProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControllerCourse : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly IErrorHandlingService<string> _errorHandlingService;
        private readonly CreateProductCommandHandler _commandHandler;
        private readonly IMapper _mapper;

        // Dependency Injection
        public ControllerCourse(ICourseService courseService, IErrorHandlingService<string> errorHandlingService, CreateProductCommandHandler commandHandler, IMapper mapper)
        {
            _courseService = courseService;
            _errorHandlingService = errorHandlingService;
            _commandHandler = commandHandler;
            _mapper = mapper;
        }

        // GET: api/<ControllerCourse>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var courses = _courseService.GetAllCourses();
                return Ok(courses);
            }
            catch
            {
                var errorMessage = _errorHandlingService.GetError();
                return StatusCode(500, errorMessage);
            }
        }

        // GET api/<ControllerCourse>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var course = _courseService.GetCourseById(id);
                return Ok(course);
            }
            catch
            {
                var errorMessage = _errorHandlingService.GetError();
                return StatusCode(500, errorMessage);
            }
        }

        // POST api/<ControllerCourse>
        [HttpPost]
        public IActionResult Post([FromBody] CreateProductCommand command)
        {
            try
            {
                _commandHandler.Handle(command); // Synchronously handle the command
                return Ok(); 
            }
            catch (Exception ex)
            {
                var errorMessage = _errorHandlingService.GetError(); // Retrieve error message
                return StatusCode(500, errorMessage); // Return 500 status with error message
            }
        }


        // PUT api/<ControllerCourse>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Course value)
        {
            try
            {
                _courseService.UpdateCourse(id, value);
                return Ok();
            }
            catch
            {
                var errorMessage = _errorHandlingService.GetError();
                return StatusCode(500, errorMessage);
            }
        }

        // DELETE api/<ControllerCourse>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _courseService.DeleteCourse(id);
                return Ok();
            }
            catch
            {
                var errorMessage = _errorHandlingService.GetError();
                return StatusCode(500, errorMessage);
            }
        }

        [HttpPost("upload-file")]
        public async Task<IActionResult> UploadFile(IFormFile productImage, string Name, string Description)
        {
            if (productImage != null && productImage.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(productImage.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "C:\\Users\\DELL\\source\\repos\\APIProject\\APIProject\\photo", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await productImage.CopyToAsync(stream);
                }

                // Save product info to the database
                var command = new CreateProductCommand
                {
                    Name = Name,
                    Description = Description,
                    ProductImage = fileName,
                    ImagePath = filePath
                };

                _commandHandler.Handle(command); // Save to the database using the handler
                return Ok(new { message = "File uploaded and product saved successfully" });
            }
            return BadRequest(new { message = "No file uploaded" });
        }


    }
}
