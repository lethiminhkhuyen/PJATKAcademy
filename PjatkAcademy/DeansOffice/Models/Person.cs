﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeansOffice
{
    public interface Person
    {
        string IdNumber { get; set; }
        string Name { get; set; }
        string Surname { get; set; }
    }
}
