using DentHub.Data.Common;
using DentHub.Web.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentHub.Data
{
	public class DbRepository<TEntity> : IRepository<TEntity>, IDisposable
		where TEntity : class
	{
		private readonly DentHubContext context;
		private DbSet<TEntity> dbSet;

		public DbRepository(DentHubContext context)
		{
			this.context = context;
			this.dbSet = this.context.Set<TEntity>();
		}


		public Task AddAsync(TEntity entity)
		{
			return this.dbSet.AddAsync(entity);
		}

		public IQueryable<TEntity> All()
		{
			return this.dbSet;
		}

		public void Delete(TEntity entity)
		{
			this.dbSet.Remove(entity);
		}

		public void Dispose()
		{
			this.context.Dispose();
		}

		public Task<int> SaveChangesAsync()
		{
			return this.SaveChangesAsync();
		}

		public void Update(TEntity entity)
		{
			throw new NotImplementedException();
		}
	}
}
