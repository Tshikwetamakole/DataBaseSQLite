using DataBaseSQLite.View;

namespace DataBaseSQLite
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new ViewPage();
        }
    }
}
