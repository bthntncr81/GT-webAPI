

namespace GTBack.Core.DTO
{
    public class UpdateEventDTO
    {
        public long Id { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int statusId { get; set; }
    }
}
