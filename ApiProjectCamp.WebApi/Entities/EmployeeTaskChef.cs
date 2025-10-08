namespace ApiProjectCamp.WebApi.Entities
{
    public class EmployeeTaskChef
    {
        public int EmployeeTaskChefId { get; set; }
        public int EmployeeTaskId { get; set; }
        public EmployeeTask EmployeeTask { get; set; }
        public int ChefId { get; set; }
        public Chef Chef { get; set; }
    }
}
