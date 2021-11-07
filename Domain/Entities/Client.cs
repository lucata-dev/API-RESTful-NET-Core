using Domain.Common;
using System;

namespace Domain.Entities
{
    public class Client : AuditableBaseEntity
    {
        private int _age;

        public string Name { get; set; }

        public string LastName { get; set; }

        public DateTime Birthday { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public int Age 
        {
            get
            {
                if (_age <= 0)
                {
                    _age = new DateTime(DateTime.Now.Subtract(Birthday).Ticks).Year - 1;
                }

                return _age;
            }
        }
    }
}
