﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Enum
{
    public enum OrderStatus
    {
        Pending=1,  
        Processing=2,  
        Shipped=3,  
        Delivered=4, 
        Cancelled=5,
        Refunded=6,  
    }
}
