using GTBack.Core.Entities.Coach;
using GTBack.Core.Results;
using GTBack.Core.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GTBack.Core.DTO.Coach.Request;
using GTBack.Core.Services.Coach;

namespace GTBack.Service.Services.Coach
{
    public class QuestionImageService : IQuestionImageService
    {
        private readonly IService<QuestionImage> _questionImageService;
        private readonly IService<SubjectScheduleRelation> _subjectScheduleRelationService;
        private readonly IService<Schedule> _scheduleService;

        public QuestionImageService(IService<QuestionImage> questionImageService, IService<SubjectScheduleRelation> subjectScheduleRelationService, IService<Schedule> scheduleService)
        {
            _questionImageService = questionImageService;
            _subjectScheduleRelationService = subjectScheduleRelationService;
            _scheduleService = scheduleService;
        }

        // Add Image
        public async Task<IResults> AddImage(QuestionImageAddDTO model)
        {
            var subjectScheduleRelation = await _subjectScheduleRelationService.GetByIdAsync(x => x.Id == model.SubjectScheduleRelationId);
            if (subjectScheduleRelation == null)
            {
                return new ErrorResult("Subject Schedule Relation not found.");
            }

            foreach (var item in model.Data)
            {
                var questionImage = new QuestionImage
                {
                    Data = item,
                    SubjectScheduleRelationId = model.SubjectScheduleRelationId
                };

                await _questionImageService.AddAsync(questionImage);
            }

         
            return new SuccessResult("Image added successfully.");
        }

        // Delete Image
        public async Task<IResults> DeleteImage(long imageId)
        {
            var image = await _questionImageService.GetByIdAsync(x => x.Id == imageId);
            if (image == null)
            {
                return new ErrorResult("Image not found.");
            }

            await _questionImageService.RemoveAsync(image);
            return new SuccessResult("Image deleted successfully.");
        }

        // List Images by SubjectScheduleRelationId
        public async Task<IDataResults<List<QuestionImage>>> ListImagesBySubjectScheduleRelationId(long subjectScheduleRelationId)
        {
            var images = await _questionImageService
                .Where(x => x.SubjectScheduleRelationId == subjectScheduleRelationId)
                .ToListAsync();

            return new SuccessDataResult<List<QuestionImage>>(images);
        }

        // List Images by Schedule.StudentId
        public async Task<IDataResults<List<QuestionImage>>> ListImagesByStudentId(long studentId)
        {
            var images = await _questionImageService
                .Where(x => x.SubjectScheduleRelation.Schedule.StudentId == studentId)
                .Include(x => x.SubjectScheduleRelation)
                .ToListAsync();

            return new SuccessDataResult<List<QuestionImage>>(images);
        }
    }
}