using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configuration
{
    public class InfrastructureConfiguration : IInfrastructureConfiguration
    {
        private readonly IConfiguration _configuration;
        public InfrastructureConfiguration(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public string NorthwindConnection
        {
            get
            {
                return this._configuration["ConnectionStrings:NorthwindDatabase"];
            }
        }

        public string GetConnectionString(string connectionName)
        {
            return this._configuration.GetConnectionString(connectionName);
        }


        public string Key
        {
            get
            {
                return this._configuration["secret"];
            }
        }

        public IConfigurationSection GetConfigurationSection(string Key)
        {
            throw new NotImplementedException();
        }


    }
}
