namespace EcomAPI.Models
{
    public class CustomerVM
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? City { get; set; }

        public string? Country { get; set; }

        public string? Phone { get; set; }
    }
}
