using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace UserSystem.EntityFrameworkCore
{
    public class WorkOfUnit : IWorkOfUnit, IDisposable
    {
        public UserSystemDbContext DbContext { get; set; }

        // 线程同步锁
        private static object _repositoryLock;

        // 数据库事务
        private IDbContextTransaction _dbContextTransaction;

        public WorkOfUnit(UserSystemDbContext context)
        {
            DbContext = context;
            _repositoryLock = new object();
        }

        public void BeginTransaction()
        {
            _dbContextTransaction = DbContext.Database.BeginTransaction();
        }

        public int Commit()
        {
            int result = 0;

            try
            {
                result = DbContext.SaveChanges();
                if (_dbContextTransaction != null)
                {
                    _dbContextTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                result = -1;
                _dbContextTransaction.Rollback();
                throw new Exception($"Commit异常：{ex.InnerException}/r{ex.Message}");
            }

            return result;
        }

        public void Dispose()
        {
            // 释放事务资源
            _dbContextTransaction?.Dispose();
            // 释放数据库上下文资源
            DbContext.Dispose();
            // 防止垃圾回收器调用类的析构函数
            GC.SuppressFinalize(this);
        }
    }
}