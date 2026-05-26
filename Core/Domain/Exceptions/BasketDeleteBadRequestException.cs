using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class BasketDeleteBadRequestException(string id) : BadRequestException
        ($"Invalid Operation When Deleting Basket with ID: {id} !")
    {
    }
}
