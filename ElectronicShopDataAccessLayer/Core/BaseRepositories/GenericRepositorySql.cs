using ElectronicShopDataAccessLayer.Contexts;
using ElectronicShopDataAccessLayer.Core.BaseModels;
using ElectronicShopDataAccessLayer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Shared.Constants;
using Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShopDataAccessLayer.Core.BaseRepositories
{

    public delegate bool ConflictResolveDetailedDelegate(PropertyValues proposedValues, PropertyValues databaseValues, IReadOnlyList<IProperty> properties);
    public delegate object ConflictResolveDelegate(string propertyName, object proposedValue, object databaseValue);

    public abstract class GenericRepositorySql<T> where T : class
    {
        protected DbContextSql _context;
        protected DbSet<T> _dbSet;

        public GenericRepositorySql(DbContextSql context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task AddAsync(T item)
        {
            await _dbSet.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(T item)
        {
            _dbSet.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<List<T>> GetAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task UpdateAsync(T item)
        {
            _context.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> GetByIdAsync(string id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddRangeAsync(IEnumerable<T> item)
        {
            await _dbSet.AddRangeAsync(item);
            await _context.SaveChangesAsync();
        }

        

        public async Task RemoveRangeAsync(IEnumerable<T> items)
        {
            _dbSet.RemoveRange(items);
            await _context.SaveChangesAsync();
        }
        /*
        public async Task RemoveRangeAsync(params T[] items)
        {
            _dbSet.RemoveRange(items);
            await _context.SaveChangesAsync();
        }*/

        public async Task<int> GetCountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public bool UpdateDetailedSafe(ConcurrentModelSql sqlConcurrentModel, ConflictResolveDetailedDelegate ConflictResolveDelegate)
        {
            bool isNotUpdated = true;

            while (isNotUpdated)
            {
                try
                {
                    _context.SaveChanges();
                    isNotUpdated = false;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (EntityEntry entry in ex.Entries)
                    {

                        var proposedValues = entry.CurrentValues;
                        var databaseValues = entry.GetDatabaseValues();

                        if (!ConflictResolveDelegate(proposedValues, databaseValues, proposedValues.Properties))
                        {
                            return false;
                        }

                        entry.OriginalValues.SetValues(databaseValues);
                    }
                }
            }


            return true;
        }
        public bool UpdateSafe(ConcurrentModelSql sqlConcurrentModel, ConflictResolveDelegate conflictResolveDelegate)
        {
            return UpdateSafe(sqlConcurrentModel, -1, conflictResolveDelegate);
        }
        public bool UpdateSafe(ConcurrentModelSql sqlConcurrentModel, int counter, ConflictResolveDelegate conflictResolveDelegate)
        {
            bool isNotUpdated = true;

            int j = 0;


            foreach (System.Reflection.PropertyInfo property in sqlConcurrentModel.GetType().GetProperties())
            {
                var value = property.GetValue(sqlConcurrentModel);
                var newVal = conflictResolveDelegate(property.Name, value, value);

                if (newVal != null)
                {
                    property.SetValue(sqlConcurrentModel, newVal);
                }

            }

            while (isNotUpdated && (j < counter || counter == -1))
            {
                j++;

                try
                {
                    _context.SaveChanges();

                    isNotUpdated = false;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (EntityEntry entry in ex.Entries)
                    {

                        var proposedValues = entry.CurrentValues;
                        var databaseValues = entry.GetDatabaseValues();

                        foreach (var property in proposedValues.Properties)
                        {
                            var proposedValue = proposedValues[property];
                            var databaseValue = databaseValues[property];

                            var newVal = conflictResolveDelegate(property.Name, proposedValue, databaseValue);

                            if (newVal == null)
                            {
                                proposedValues[property] = databaseValue;
                            }
                            else
                            {
                                proposedValues[property] = newVal;
                            }

                        }

                        entry.OriginalValues.SetValues(databaseValues);
                    }
                }
            }

            if (isNotUpdated)
            {
                
                throw new ServerException(Constants.Errors.SafeUpdate.MODEL_WAS_NOT_UPDATED);
            }


            return true;
        }

    }

}
