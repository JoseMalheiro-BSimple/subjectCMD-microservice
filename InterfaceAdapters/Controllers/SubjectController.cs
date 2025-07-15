using Application.DTO;
using Application.IService;
using Microsoft.AspNetCore.Mvc;

namespace InterfaceAdapters.Controllers
{
    [Route("api/subjects")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpPost]
        public async Task<ActionResult<CreatedSubjectDTO>> Create([FromBody] CreateSubjectDTO createSubjectDTO)
        {
            var result = await _subjectService.Create(createSubjectDTO);

            return result.ToActionResult();
        }
    }
}
