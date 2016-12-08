﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Anything.Helpers
{
    public enum OrderType
    {
        None = 0,
        Unpaid,
        Paid,
        Expired
    }

    public enum RoomPriceType
    {
        Day = 0,
        Holiday,
        Fixed       
    }
}