using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SalesWebMvc.Models.Entities
{
    public class Seller
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} requerido.")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "{0} deve ser entre {2} e {1} caracteres.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} requerido")]
        [EmailAddress(ErrorMessage = "Email invalido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} requerido.")]
        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "{0} requerido")]
        [Range(100.0, 50000.0, ErrorMessage = "{0} deve estar entre {1} e {2}")]
        [Display(Name = "Base Salary")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double BaseSalary { get; set; }

        public Department Department { get; set; }
        public int DepartmentId { get; set; }
        public ICollection<SalesRecord> Sales { get; set; } = new List<SalesRecord>();

        public Seller()
        {

        }

        public Seller(string name, string email, DateTime birthDate, double baseSalary, Department department)
        {
            Name = name;
            Email = email;
            BirthDate = birthDate;
            BaseSalary = baseSalary;
            Department = department;
        }

        public void AddSales(SalesRecord sales)
        {
            Sales.Add(sales);
        }

        public void RemoveSales(SalesRecord sales)
        {
            Sales.Remove(sales);
        }

        public double TotalSales(DateTime initial, DateTime final)
        {
            return Sales
                .Where(x => x.Date >= initial && x.Date <= final)
                .Sum(x => x.Amount);
        }
    }
}
