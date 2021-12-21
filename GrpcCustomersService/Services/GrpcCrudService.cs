using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess = Ursu_Alexandra_Lab2.Data;
using ModelAccess = Lab2_2.Models;

namespace GrpcCustomersService
{
    public class GrpcCrudService : CustomerService.CustomerServiceBase
    {

        private DataAccess.LibraryContext db = null;
        public GrpcCrudService(DataAccess.LibraryContext db)
        {
            this.db = db;
        }
        public override Task<CustomerList> GetAll(Empty empty, ServerCallContext
       context)
        {

            CustomerList pl = new CustomerList();
            var query = from cust in db.Customers
                        select new Customer()
                        {
                            CustomerId = cust.CustomerID,
                            Name = cust.Name,
                            Adress = cust.Adress
                        };
            pl.Item.AddRange(query.ToArray());
            return Task.FromResult(pl);
        }
        public override Task<Empty> Insert(Customer requestData, ServerCallContext
       context)
        {
            db.Customers.Add(new ModelAccess.Customer
            {
                CustomerID = requestData.CustomerId,
                Name = requestData.Name,
                Adress = requestData.Adress,
                BirthDate = DateTime.Parse(requestData.Birthdate)
            });
            db.SaveChanges();
            return Task.FromResult(new Empty());
        }

        public override Task<Customer> Update(Customer requestData, ServerCallContext
      context)
        {

            db.Customers.Remove(new ModelAccess.Customer
            {
                CustomerID = requestData.CustomerId,
                Name = requestData.Name,
                Adress = requestData.Adress,
                BirthDate = DateTime.Parse(requestData.Birthdate)

            });
            db.SaveChanges();
            return Task.FromResult(new Customer());
        }

        public override Task<Empty> Delete(CustomerId requestData, ServerCallContext
       context)
        {

            db.Customers.Remove(new ModelAccess.Customer
            {
                CustomerID = requestData.Id,
            });
            db.SaveChanges();
            return Task.FromResult(new Empty());
        }
    }
}
