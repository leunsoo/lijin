using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace PublicLibsManagement
{
    public interface iCompanyLib
    {
        DataTable Company_GetByCode(string ComCode, IDbTransaction transaction = null);
        int Update(CompanyModel model, IDbTransaction transaction = null);
        DataTable GetAll(string ComCode, IDbTransaction transaction = null);

    }
}
