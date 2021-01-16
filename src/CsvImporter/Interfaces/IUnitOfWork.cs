using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CsvImporter.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }
        IDbTransaction BeginTransaction();
        void Commit();
        void Rollback();
    }
}
