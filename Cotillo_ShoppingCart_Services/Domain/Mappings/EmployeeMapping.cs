using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cotillo_ShoppingCart_Services.Domain.Mappings
{
    //public class EmployeeMapping : EntityTypeConfiguration<EmployeeEntity>
    //{
    //    public EmployeeMapping()
    //    {
    //        //Oracle uses table names in uppercase, make sure you set your table name in uppercase, this won't affect SQL Server.
    //        this.ToTable("EMPLOYEE");

    //        this.HasKey(employee => employee.Id);

    //        //Uncomment these lines if you want to give different column name to your property.
    //        //Use this code in case your column name in the database doesn't match your property name

    //        //this.Property(employee => employee.Id)
    //        //    .HasColumnName("EMPLOYEE_ID");

    //        //Setting column length
    //        this.Property(employee => employee.FirstName)
    //            .HasMaxLength(255);

    //        //One to many relationship that includes two endpoint references

    //        //This is read as: 
    //        //Employee has a non null FK referencing Employer. For null FK use HasOptional
    //        //Employer has a List referencing Employees. 
    //        //Employee is using EmployerId as FK Column. 
    //        //It won't cascade on delete
    //        this.HasRequired(employee => employee.Employer)
    //            .WithMany(employer => employer.Employees)
    //            .HasForeignKey(employee => employee.EmployerId)
    //            .WillCascadeOnDelete(false);
    //    }
    //}
}
