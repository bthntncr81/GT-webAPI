namespace GTBack.Core.DTO.Coach.Request;

public class AddStudentToClassroom
{
    public int ClassroomId { get; set; } 
  public   List<int> StudentIds { get; set; }
}