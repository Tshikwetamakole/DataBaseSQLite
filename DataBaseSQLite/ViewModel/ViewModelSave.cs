using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static DataBaseSQLite.Model.ModelCustomers;

namespace DataBaseSQLite.ViewModel
{
    public class ViewModelSave : INotifyPropertyChanged
    {
        private readonly ViewModelDBService _dbService = new ViewModelDBService();

        //use Observable collection to notify the UI changes
        public ObservableCollection<Customer> Customers { get; set; } = new ObservableCollection<Customer>();

        private string _name;
        private string _email;
        private string _password;

        public bool IsEdiMode { get; set; }
        public Customer EditingCustomer { get; set; }

        public string Name 
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }

        //DisplayAlert
        private Page _page;

        //Class constructor
        public ViewModelSave(Page page)
        {
            _page = page;
            SaveCommand = new Command(async () => await SaveMethod());
        }

        private async Task SaveMethod() 
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(Email))
            {
                MessagingCenter.Send(this, "DisplayAlert", "Please fill all the fields");
                return;
            }

            if (IsEdiMode && EditingCustomer != null)
            {
                //Update existing customer
                EditingCustomer.Name = Name;
                EditingCustomer.Email = Email;
                EditingCustomer.Password = Password;

                //Save updated data
                await _dbService.update(EditingCustomer);

                //Refresh (Re-add and Delete)
                int index = Customers.IndexOf(EditingCustomer);
                if (index >= 0)
                {
                    Customers.RemoveAt(index);
                    Customers.Insert(index, EditingCustomer);
                }

                MessagingCenter.Send(this, "DisplayAlert", "Data updated successfully");
                IsEdiMode = false;
                EditingCustomer = null;

            }

            else
            {
                //Existing data
                var existingCustomer = await _dbService.GetCustomers();
                bool emailExist = existingCustomer.Any(c => c.Email == Email);
                if (emailExist)
                {
                    MessagingCenter.Send(this, "DisplayAlert", "Email already exist");
                    return;
                }

                //Add customer
                var newCustomer = new Customer { Name = Name, Email = Email, Password = Password };
                await _dbService.Create(newCustomer);
                Customers.Add(newCustomer);

                MessagingCenter.Send(this, "DisplayAlert", "Customer Added");
            }
            
            //Clear fields or reset
            Name = Email = Password = string.Empty;
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(Password));
        }

        private async void Loadcustomer()
        {
            var customerList = await _dbService.GetCustomers();
            Customers.Clear();

            foreach (var customer in customerList)
            {
                Customers.Add(customer);
            }
        }

        public async Task OnCustomerTapped(Customer customer) 
        {
            var action = await _page.DisplayActionSheet("Action", "Cancel", null, "Edit", "Delete");

            switch (action) 
            {
                case "Edit":
                    Name = customer.Name;
                    Email = customer.Email;
                    Password = customer.Password;
                    IsEdiMode = true;
                    EditingCustomer = customer;
                    break;

                case "Delete":
                    await _dbService.delete(customer);
                    Customers.Remove(customer);
                    break;
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
