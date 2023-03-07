using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace TrusteeApp.Models
{
    public class ParamSpec
    {
        public string ReferenceNbr { get; set; }
        public string DocType { get; set; }
        public string UserEmail { get; set; }
        //public string ContentTypeCode { get; set; }
        //public IFormFileCollection Data { get; set; }

    }
}

