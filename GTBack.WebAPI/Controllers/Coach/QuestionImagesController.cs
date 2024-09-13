using GTBack.Core.Results;
using GTBack.Core.Services.Coach;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GTBack.Core.DTO.Coach.Request;

namespace GTBack.WebAPI.Controllers
{
    [ApiController]
    [Route("api/QuestionImages")]
    public class QuestionImageController : CoachBaseController
    {
        private readonly IQuestionImageService _questionImageService;

        public QuestionImageController(IQuestionImageService questionImageService)
        {
            _questionImageService = questionImageService;
        }

        // Add Image
        [Authorize]
        [HttpPost("AddImage")]
        public async Task<IActionResult> AddImage(QuestionImageAddDTO model)
        {
            var result = await _questionImageService.AddImage(model);
            return ApiResult(result);
        }

        // Delete Image
        [Authorize]
        [HttpDelete("DeleteImage/{id}")]
        public async Task<IActionResult> DeleteImage(long id)
        {
            var result = await _questionImageService.DeleteImage(id);
            return ApiResult(result);
        }

        // List Images by SubjectScheduleRelationId
        [Authorize]
        [HttpGet("ListBySubjectScheduleRelation/{subjectScheduleRelationId}")]
        public async Task<IActionResult> ListImagesBySubjectScheduleRelationId(long subjectScheduleRelationId)
        {
            var result = await _questionImageService.ListImagesBySubjectScheduleRelationId(subjectScheduleRelationId);
            return ApiResult(result);
        }

        // List Images by StudentId
        [Authorize]
        [HttpGet("ListByStudent/{studentId}")]
        public async Task<IActionResult> ListImagesByStudentId(long studentId)
        {
            var result = await _questionImageService.ListImagesByStudentId(studentId);
            return ApiResult(result);
        }

     
    }
}