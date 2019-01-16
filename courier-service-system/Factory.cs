using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CSSEntity;
using CSSService;

namespace courier_service_system
{
    public class Factory
    {
        private Factory() { }
        public static Factory factory = null;
        public static Factory getInstance()
        {
            if (factory == null)
            {
                factory = new Factory();
                return factory;
            }
            return factory;
        }
    }
}