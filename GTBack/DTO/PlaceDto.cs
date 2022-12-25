﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTBack.Core.DTO
{
    public  class PlaceDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
     
        public string Mail { get; set; }

        public string Phone { get; set; }
        public string Address { get; set; }

        public string ProfilImage { get; set; }
        public string CoverImage { get; set; }
        public int CustomerId { get; set; }


    }
}
