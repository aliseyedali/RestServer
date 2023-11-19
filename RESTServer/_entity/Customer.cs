using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RESTServer
{
    public class Customer : IComparable<Customer>
    {
        [Required]
        public string FirstName { get; set; } = default!;

        [Required]
        public string LastName { get; set; } = default!;

        [Range(18, int.MaxValue, ErrorMessage = "Age must be 18 or older.")]
        public int Age { get; set; }

        [Required]
        public int Id { get; set; }

        [JsonIgnore]
        public string FullName
        {
            get 
            { 
            return  $"{FirstName} {LastName}";
            }
        }
        public int CompareTo(Customer other)
        {
            if (other == null)
            {
                return 1;
            }

            // Compare by last name first
            int lastNameComparison = string.Compare(this.LastName, other.LastName, StringComparison.Ordinal);
            if (lastNameComparison != 0)
            {
                return lastNameComparison;
            }

            // If last names are the same, compare by first name
            return string.Compare(this.FirstName, other.FirstName, StringComparison.Ordinal);
        }
    }
}
