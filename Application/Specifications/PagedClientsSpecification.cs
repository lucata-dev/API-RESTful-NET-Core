using Ardalis.Specification;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Specifications
{
    public class PagedClientsSpecification : Specification<Client>
    {
        public PagedClientsSpecification(int pageNumber, int pageSize, string name, string lastName)
        {
            Query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            if (!string.IsNullOrEmpty(name))
            {
                Query.Search(c => c.Name, "%" + name + "%");
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                Query.Search(c => c.LastName, "%" + lastName + "%");
            }
        }
    }
}
