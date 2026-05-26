using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class DeliveryNotFoundException(int id) : NotFoundException($"Delivery Method with id '{id}' not found.")
    {
    }
}
