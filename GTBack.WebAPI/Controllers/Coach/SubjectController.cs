using GTBack.Core.DTO;
using GTBack.Core.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using GTBack.Core.DTO.Coach.Request;
using GTBack.Core.Entities.Coach;
using GTBack.Core.Enums.Coach;
using GTBack.Core.Services.coach;

namespace GTBack.WebAPI.Controllers
{
    [ApiController]
    [Route("api/subjects")]
    public class SubjectController : CoachBaseController
    {
        private readonly ISubjectService _subjectService;
        private readonly ILessonService _lessonService;

        public SubjectController(ISubjectService subjectService,ILessonService lessonService)
        {
            _subjectService = subjectService;
            _lessonService = lessonService;
        }

        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> AddSubjectToStudent(long subjectId, DayOfWeekEnum day, string timeSlot)
        {
            var result = await _subjectService.AddSubjectToStudent(subjectId, day, timeSlot);
            return ApiResult(result);
        }

        [Authorize]
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateSubjectForStudent(long subjectId, DayOfWeekEnum day, string timeSlot)
        {
            var result = await _subjectService.UpdateSubjectForStudent(subjectId, day, timeSlot);
            return ApiResult(result);
        }

        [Authorize]
        [HttpDelete("Remove")]
        public async Task<IActionResult> DeleteSubjectForStudent(long subjectId, DayOfWeekEnum day, string timeSlot)
        {
            var result = await _subjectService.DeleteSubjectForStudent(subjectId, day, timeSlot);
            return ApiResult(result);
        }
        
        
        [HttpPost("CreateLesson")]
        public async Task<IActionResult> LessonAdd(LessonAddDTO lesson)
        {
               var result=  await _lessonService.AddLessonWithSubLessonsAndSubjects(lesson);
            return ApiResult(result);

        }
        
               
        [HttpGet("GetLessons")]
        public async Task<IActionResult> GetLessons()
        {
            var result=  await _subjectService.GetAllLessonsWithSubLessonsAndSubjects();
            return ApiResult(result);

        }
        
                    
        [HttpGet("GetDailySubjects/{studentId}")]
        public async Task<IActionResult> GetDailySubjects([FromRoute]int studentId,DayOfWeekEnum day)
        {
            var result=  await _subjectService.GetSubjectsByStudentIdAndDay(studentId,day);
            return ApiResult(result);

        }
        
                       
        [HttpGet("GetAllWeek/{studentId}")]
        public async Task<IActionResult> GetAllWeek([FromRoute]int studentId)
        {
            var result=  await _subjectService.GetSubjectsByStudentIdGroupedByDay(studentId);
            return ApiResult(result);

        }
    }
}