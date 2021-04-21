using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomeCacheConfigurationProvider.Infra
{
    public class NewsRepository
    {
        public int GetNewsCount()
        {
            System.Threading.Thread.Sleep(1000);
            return 10;
        }
    }
}
