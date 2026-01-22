using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Application.Interfaces
{
    public interface ISqlConnectionFactory
    {
        IDbConnection CreateConnection();
    }

}
