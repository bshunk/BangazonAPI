using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BangazonAPI.Models;
using System.Threading.Tasks;

namespace BangazonAPI.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BangazonAPIContext(serviceProvider.GetRequiredService<DbContextOptions<BangazonAPIContext>>()))
            {
                // Look for any Customers.
                if (context.Customer.Any())
                {
                    return;   // DB has been seeded
                }

                var customers = new Customer[]
                {
                    new Customer { 
                        FirstName = "Svetlana",
                        LastName = "Smith"

                    },
                    new Customer { 
                        FirstName = "Nigel",
                        LastName = "Thornberry"
                    },
                    new Customer { 
                        FirstName = "Sequina",
                        LastName = "Jones"
                    },
                };

                foreach (Customer i in customers)
                {
                    context.Customer.Add(i);
                }
                context.SaveChanges();


                var productTypes = new ProductType[]
                {
                    new ProductType { 
                        Name = "Food"
                    },
                     new ProductType { 
                        Name = "Automobile"
                    },
                    new ProductType { 
                        Name = "Furniture"
                    },
                };

                foreach (ProductType p in productTypes)
                {
                    context.ProductType.Add(p);
                }
                context.SaveChanges();

                var paymentTypes = new PaymentType[]
                {
                    new PaymentType{
                        AccountNumber = 123459889,
                        Name = "Visa",
                        CustomerID = customers.Single(s => s.FirstName == "Svetlana").CustomerID
                    },
                    new PaymentType{
                        AccountNumber = 555555555,
                        Name = "MasterCard",
                        CustomerID = customers.Single(c => c.FirstName == "Sequina").CustomerID
                    },
                    new PaymentType{
                        AccountNumber = 987654321,
                        Name = "SeaShells",
                        CustomerID = customers.Single(n => n.FirstName == "Nigel").CustomerID
                    },
                };

                foreach (PaymentType t in paymentTypes)
                {
                    context.PaymentType.Add(t);
                }
                context.SaveChanges();
                var products = new Product[]
                {
                    new Product{
                        Title  = "Taco",
                        Description = "Delisious beef tacos in a hard corn tortia shell",
                        Price = 0.99,
                        ProductTypeID = productTypes.Single(p => p.Name == "Food").ProductTypeID,
                        CustomerID = customers.Single(c => c.FirstName == "Svetlana").CustomerID
                    },
                    new Product{
                        Title = "VW Beetle",
                        Description = "A classic hippy-mobile from 1967",
                        Price = 1289.99,
                        ProductTypeID = productTypes.Single(i => i.Name == "Automobile").ProductTypeID,
                        CustomerID = customers.Single(c => c.FirstName == "Svetlana").CustomerID
                    },
                    new Product{
                        Title = "Loveseat",
                        Description = "A comfortable coach that seats two",
                        Price = 199,
                        ProductTypeID = productTypes.Single(i => i.Name == "Furniture").ProductTypeID,
                        CustomerID = customers.Single(c => c.FirstName == "Nigel").CustomerID
                    }
                };

                foreach(Product p in products)
                {
                    context.Add(p);
                }
                context.SaveChanges();

                var orders = new Order[]
                {
                    new Order{
                        CustomerID = customers.Single(c => c.FirstName == "Sequina").CustomerID
                        
                    },
                    new Order{
                        CustomerID = customers.Single(c => c.FirstName == "Sequina").CustomerID
                    }
                };
                
                foreach(Order p in orders)
                {
                    context.Add(p);
                }
                context.SaveChanges();

                var departments = new Department[]
                {
                    new Department { 
                        Name = "Marketing",
                        ExpenseBudget = 200000
                    },
                    new Department { 
                        Name = "Accounting",
                        ExpenseBudget = 120000
                    },
                    new Department { 
                        Name = "IT",
                        ExpenseBudget = 150000
                    }
                };
                foreach (Department dept in departments)
                {
                    context.Department.Add(dept);
                }
                context.SaveChanges();
                var computers = new Computer[]
                {
                    new Computer { 
                        DatePurchased = new DateTime(),
                    },
                    new Computer { 
                        DatePurchased = new DateTime(),
                    },
                    new Computer { 
                        DatePurchased = new DateTime(),
                    }
                };
                foreach (Computer comp in computers)
                {
                    context.Computer.Add(comp);
                }
                context.SaveChanges();
                var trainingPrograms = new TrainingProgram[]
                {
                    new TrainingProgram { 
                        DateStart = new DateTime(),
                        DateEnd = new DateTime(),
                        MaxAttendees = 50
                    },
                    new TrainingProgram { 
                        DateStart = new DateTime(),
                        DateEnd = new DateTime(),
                        MaxAttendees = 400
                    },
                    new TrainingProgram { 
                        DateStart = new DateTime(),
                        DateEnd = new DateTime(),
                        MaxAttendees = 200
                    }
                };
                foreach (TrainingProgram tp in trainingPrograms)
                {
                    context.TrainingProgram.Add(tp);
                }
                context.SaveChanges();
                var employees = new Employee[]
                {
                    new Employee { 
                        Name = "Joe Dirt",
                        DateStarted = new DateTime(),
                        JobTitle = "Graphic Designer",
                        IsSupervisor = 0,
                        DepartmentID = departments.Single(x => x.Name == "Marketing").DepartmentID
                    },
                    new Employee { 
                        Name = "Kevin Garvey",
                        DateStarted = new DateTime(),
                        JobTitle = "Head of Accounting",
                        IsSupervisor = 1,
                        DepartmentID = departments.Single(x => x.Name == "Accounting").DepartmentID
                    },
                    new Employee { 
                        Name = "Max Payne",
                        DateStarted = new DateTime(),
                        JobTitle = "Senior Developer",
                        IsSupervisor = 0,
                        DepartmentID = departments.Single(x => x.Name == "IT").DepartmentID
                    }
                };
                foreach (Employee emp in employees)
                {
                    context.Employee.Add(emp);
                }
                context.SaveChanges();

            }
       }
    }
}