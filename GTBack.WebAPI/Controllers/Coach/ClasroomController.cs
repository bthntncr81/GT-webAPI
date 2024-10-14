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
    [Route("api/Classroom")]
    public class ClasroomController : CoachBaseController
    {
        private readonly IClassroomService _classroomService;
        private readonly ILessonService _lessonService;

        public ClasroomController(IClassroomService classroomService, ILessonService lessonService)
        {
            _classroomService = classroomService;
            _lessonService = lessonService;
        }

        [HttpPost("AddClassroom")]
        public async Task<IActionResult> ClassroomAdd(ClassroomAddDTO model)
        {
            var result = await _classroomService.AddClassroom(model);
            return ApiResult(result);
        }

        [Authorize]
        [HttpPost("AddStudentToClassroom")]
        public async Task<IActionResult> AddStudentToClassroom(AddStudentToClassroom model)
        {
            var result = await _classroomService.AddClassRoomToStudents(model);
            return ApiResult(result);
        }

        [Authorize]
        [HttpPost("AddSubjectOnSubLesson")]
        public async Task<IActionResult> AddSubjectOnSubLesson(AddSubjectAllClassesDTO model)
        {
            var result = await _classroomService.AddSubjectOnSubLesson(model);
            return ApiResult(result);
        }


        [Authorize]
        [HttpPost("AddLessonToClass")]
        public async Task<IActionResult> DeleteSubjectForStudent(AddScheduleAllClass model)
        {
            var result = await _classroomService.AddScheduleToClass(model);
            return ApiResult(result);
        }

        [Authorize]
        [HttpGet("GetClassrooms")]
        public async Task<IActionResult> GetClassrooms()
        {
            var result = await _classroomService.GetClassrooms();
            return ApiResult(result);
        }


        [Authorize]
        [HttpGet("GetStudentsByClassroomId/{id}")]
        public async Task<IActionResult> GetClassStudents(int id)
        {
            var result = await _classroomService.GetClassStudents(id);
            return ApiResult(result);
        }





    }
}