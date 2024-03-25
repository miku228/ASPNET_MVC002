using System.ComponentModel.DataAnnotations;

namespace Lab8.Models.DataAccess
{
    public class AcademicRecordMetadata
    {
        [Required(ErrorMessage = "Must enter a grade!")]
        [Range(0, 100, ErrorMessage = "Must between 0 and 100")]
        [RegularExpression(@"\b([0-9]|[1-9][0-9]|100)\b",
            ErrorMessage = "The value is not valid for grade1")]
        
        public int? Grade { get; set; }
    }
}
