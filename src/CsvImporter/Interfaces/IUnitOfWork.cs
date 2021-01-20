using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace CsvImporter.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        SqlConnection Connection { get; }
        SqlTransaction Transaction { get; }
        SqlTransaction BeginTransaction();
        void Commit();
        void Rollback();
    }
}
