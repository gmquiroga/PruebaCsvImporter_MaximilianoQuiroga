using CsvImporter.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace CsvImporter.Infrastructure.Data
{
    public class UnitOfWork: IUnitOfWork
    {
        public SqlConnection Connection { get; private set; }
        public SqlTransaction Transaction { get; private set; }

        public UnitOfWork(SqlConnection dbConnection)
        {
            Connection = dbConnection;

            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }
        }

        public SqlTransaction BeginTransaction()
        {
            if (Transaction != null)
            {
                throw new NullReferenceException("Not finished previous transaction");
            }

            Transaction = Connection.BeginTransaction();
            return Transaction;
        }

        public virtual void Commit()
        {
            if (Transaction != null)
            {
                try
                {
                    Transaction.Commit();
                }
                catch (Exception)
                {
                    Transaction.Rollback();
                }
                Transaction.Dispose();
                Transaction = null;
            }
            else
            {
                throw new NullReferenceException("Tryed commit not opened transaction");
            }
        }

        public virtual void Rollback()
        {
            if (Transaction != null)
            {
                Transaction.Rollback();
                Transaction.Dispose();
                Transaction = null;
            }
            else
            {
                throw new NullReferenceException("Tryed commit not opened transaction");
            }
        }

        public virtual void Dispose()
        {
            if (Transaction != null)
            {
                Transaction.Dispose();
                Transaction = null;
            }

            if (Connection != null)
            {
                Connection.Close();
                Connection = null;
            }

            
        }

    }
}
