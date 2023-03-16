﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PublicApiRepo.Interfaces
{
    public interface IPublicAPIsDataService
    {
        Task<HttpResponseMessage> GetAPIsDataAsync();
    }
}
