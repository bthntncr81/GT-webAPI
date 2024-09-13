using GTBack.Core.Entities.Coach;
using GTBack.Core.Results;
using System.Collections.Generic;
using System.Threading.Tasks;
using GTBack.Core.DTO.Coach.Request;

namespace GTBack.Core.Services.Coach
{
    public interface IQuestionImageService
    {
        // Add Image to a SubjectScheduleRelation
        Task<IResults> AddImage(QuestionImageAddDTO model);
        // Delete Image by its ID
        Task<IResults> DeleteImage(long imageId);

        // List Images by SubjectScheduleRelationId
        Task<IDataResults<List<QuestionImage>>> ListImagesBySubjectScheduleRelationId(long subjectScheduleRelationId);

        // List Images by StudentId (through the associated Schedule)
        Task<IDataResults<List<QuestionImage>>> ListImagesByStudentId(long studentId);
    }
}