using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataBaseSQLite.Model.ModelCustomers;

namespace DataBaseSQLite.ViewModel
{
    public class ViewModelDBService
    {
        //DB name
        private const string DB_NAME = "demo_local_db.db3";

        //Connect
        private readonly SQLiteAsyncConnection _connection; 


        //Class constructor
        public ViewModelDBService() 
        {
            //BD path as parameter
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME));
                _ = _connection.CreateTableAsync<Customer>().ConfigureAwait(false);
        }

        //Get Customers
        public async Task<List<Customer>> GetCustomers()
        {
            return await _connection.Table<Customer>().ToListAsync();
        }

        //Return a record baased on ID
        public async Task<Customer> GetbyId(int id)
        {
            return await _connection.Table<Customer>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        //Add a customer record
        public async Task Create(Customer customer)
        {
            await _connection.InsertAsync(customer);
        }

        //Update
        public async Task update(Customer customer) 
        {
            await _connection.UpdateAsync(customer);
        }
        
        //Delete
        public async Task delete(Customer customer) 
        {
            await _connection.DeleteAsync(customer);
        }
    }
}
