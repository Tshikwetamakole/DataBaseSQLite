using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseSQLite.Model
{
    public class ModelCustomers
    {
        [SQLite.Table("customer")]
        public class Customer 
        {
            [PrimaryKey]
            [AutoIncrement]
            [SQLite.Column("Id")]
            public int Id { get; set; }


            [SQLite.Column("name")]
            public string Name { get; set; }


            [SQLite.Column("password")]
            public string Password { get; set; }


            [SQLite.Column("email")]
            public string Email { get; set; }
        }
    }
}
