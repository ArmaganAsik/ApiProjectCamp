namespace ApiProjectCamp.WebApi.Dtos.GroupReservationDtos
{
    public class CreateGroupReservationDto
    {
        public string ResponsibleCustomerName { get; set; }
        public string GroupTitle { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime LastProcessTime { get; set; }
        public string Priority { get; set; }
        public string Details { get; set; }
        public string ReservationStatus { get; set; }
        public int? NumberOfPeople { get; set; }
        public string? Email { get; set; }
    }
}