using GTBack.Core.Entities.Coach;
using GTBack.Core.Results;
using GTBack.Core.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using GTBack.Core.DTO.Coach.Request;
using GTBack.Core.Services.coach;
using Microsoft.EntityFrameworkCore;

public class LessonService : ILessonService
{
    private readonly IService<Lesson> _lessonService;
    private readonly IService<SubLesson> _subLessonService;
    private readonly IService<Subject> _subjectService;

    public LessonService(IService<Lesson> lessonService, IService<SubLesson> subLessonService, IService<Subject> subjectService)
    {
        _lessonService = lessonService;
        _subLessonService = subLessonService;
        _subjectService = subjectService;
    }

    public async Task<IResults> AddLessonWithSubLessonsAndSubjects(LessonAddDTO lessonDto)
    {
        // Check if the lesson already exists
        var existingLesson = await _lessonService.Where(x => x.Name == lessonDto.Name).FirstOrDefaultAsync();
        if (existingLesson != null)
        {
            // Create a new Lesson entity
         

            // Add SubLessons and Subjects from DTO
            foreach (var subLessonDto in lessonDto.SubLessons)
            {
                var subLesson = new SubLesson
                {
                    Name = subLessonDto.Name,
                    Description = subLessonDto.Description,
                    LessonId = existingLesson.Id,
                    Subjects = new List<Subject>()
                };

                foreach (var subjectDto in subLessonDto.Subjects)
                {
                    var subject = new Subject
                    {
                        Name = subjectDto.Name,
                        Description = subjectDto.Description,
                        SubLessonId = subLesson.Id
                    };

                    subLesson.Subjects.Add(subject);
                }
                await _subLessonService.AddAsync(subLesson);

            }

            // Add the lesson and associated sub-lessons and subjects to the database
            
        }
        else
        {
            // Create a new Lesson entity
            var lesson = new Lesson
            {
                Name = lessonDto.Name,
                Description = lessonDto.Description,
                SubLessons = new List<SubLesson>()
            };

            // Add SubLessons and Subjects from DTO
            foreach (var subLessonDto in lessonDto.SubLessons)
            {
                var subLesson = new SubLesson
                {
                    Name = subLessonDto.Name,
                    Description = subLessonDto.Description,
                    LessonId = lesson.Id,
                    Subjects = new List<Subject>()
                };

                foreach (var subjectDto in subLessonDto.Subjects)
                {
                    var subject = new Subject
                    {
                        Name = subjectDto.Name,
                        Description = subjectDto.Description,
                        SubLessonId = subLesson.Id
                    };

                    subLesson.Subjects.Add(subject);
                }

                lesson.SubLessons.Add(subLesson);
            }

            // Add the lesson and associated sub-lessons and subjects to the database
            await _lessonService.AddAsync(lesson);
        }

     
      

            return new SuccessResult("Lesson, SubLessons, and Subjects added successfully");
    }
}