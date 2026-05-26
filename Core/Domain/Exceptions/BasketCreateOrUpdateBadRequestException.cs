using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class BasketCreateOrUpdateBadRequestException() : BadRequestException
        ($"Invaild Opertation When Create Or Update Basket !")
    {
    }
}
