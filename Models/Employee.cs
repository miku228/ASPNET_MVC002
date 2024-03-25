using Lab8.Models.EntityMetadata;
using Microsoft.AspNetCore.Mvc;

namespace Lab8.Models.DataAccess
{
    [ModelMetadataType(typeof(EmployeeMetadata))]
    public partial class Employee
    {
    }
}
