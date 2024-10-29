using Microsoft.AspNetCore.Mvc;
using APIProject.Services;
using APIProject.Models;
using APIProject.Handler;
using APIProject.Command;
using APIProject.Mapper;
using APIProject.DTO;
using MediatR;
using System.Threading;

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
        private readonly IMediator _mediator;


        // Dependency Injection
        public ControllerCourse(ICourseService courseService, IErrorHandlingService<string> errorHandlingService, CreateProductCommandHandler commandHandler, IMapper mapper, IMediator mediator)
        {
            _courseService = courseService;
            _errorHandlingService = errorHandlingService;
            _commandHandler = commandHandler;
            _mapper = mapper;
            _mediator = mediator;
        }

        // GET: api/<ControllerCourse>
        [HttpGet]
        public ActionResult<IEnumerable<Course>> Get()
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
        public async Task<IActionResult> Post([FromForm] CourseDto courseDto, IFormFile productImage)
        {
            try
            {
                if (productImage != null && productImage.Length > 0)
                {
                    // Define the path to save the image
                    var filePath = Path.Combine("C:\\Users\\DELL\\source\\repos\\APIProject\\APIProject\\photo", productImage.FileName);

                    // Save the file to the defined path
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await productImage.CopyToAsync(stream);
                    }

                    // Map the DTO to the course entity
                    var course = _mapper.MapToEntity(courseDto);

                    // Assign the image path to the entity
                    course.ProductImage = filePath;

                    // Add course to the service (or DB)
                    _courseService.AddCourse(course);

                    return Ok(new { message = "Course and image added successfully" });
                }
                else
                {
                    return BadRequest("Product image is required.");
                }
            }
            catch (Exception ex)
            {
                var errorMessage = _errorHandlingService.GetError();
                return StatusCode(500, errorMessage);
            }
        }




        // PUT api/<ControllerCourse>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] CourseDto courseDto, IFormFile? productImage)
        {
            try
            {
                // Fetch the existing course by ID
                var existingCourse = _courseService.GetCourseById(id);
                if (existingCourse == null)
                {
                    return NotFound(new { message = "Course not found" });
                }

                // Check if an image file is provided
                if (productImage != null && productImage.Length > 0)
                {
                    // Define the path to save the image
                    var filePath = Path.Combine("C:\\Users\\DELL\\source\\repos\\APIProject\\APIProject\\photo", productImage.FileName);

                    // Save the new file to the defined path
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await productImage.CopyToAsync(stream);
                    }

                    // Update the image path in the course entity
                    existingCourse.ProductImage = filePath;
                }

                // Map other properties from DTO to the course entity
                existingCourse = _mapper.MapToEntity(courseDto);

                // Update the course in the database
                _courseService.UpdateCourse(id, existingCourse);

                return Ok(new { message = "Course updated successfully" });
            }
            catch (Exception ex)
            {
                // Log the exception and return a custom error message
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
                    ProductImage = fileName
                };

                // Provide the CancellationToken
                await _commandHandler.Handle(command, HttpContext.RequestAborted); // Save to the database using the handler
                return Ok(new { message = "File uploaded and product saved successfully" });
            }
            return BadRequest(new { message = "No file uploaded" });
        }

        [HttpPost("okay")]
        public async Task<IActionResult> CreateCourse(CreateProductCommand command)
        {
            var course = await _mediator.Send(command);
            return CreatedAtAction(nameof(CreateCourse), new { id = course.Id }, course);
        }



    }
}