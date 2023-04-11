﻿using System;

namespace PrismWorkApp.OpenWorkLib.Data.Service
{
    public delegate int SaveChangesEventHandler(object sender, JornalEventsArgs e);
    public class JornalEventsArgs : EventArgs
    {
        public IUnDoReDoSystem UnDoReDo { get; set; }
    }
}