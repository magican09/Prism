﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PrismWorkApp.OpenWorkLib.Data
{
    public class bldAggregationDocument: bldDocument, IbldCertificateDocument, IEntityObject
    {
        public bldAggregationDocument(string name) : base(name)
        {
           
            
        }
        public bldAggregationDocument():base()
        {

        }
    }
}
