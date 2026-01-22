using System.ComponentModel;
using MyWinFormsApp.Services;

namespace MyWinFormsApp.View
{
    public partial class CustomerForm : Form, ICustomerView
    {
        private readonly ILoggingService _loggingService;
        private readonly IConfigurationService _configurationService;

        // Constructor with dependency injection
        public CustomerForm(ILoggingService loggingService, IConfigurationService configurationService)
        {
            _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
            _configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));

            InitializeComponent();
            LoadFormSettings();

            _loggingService.LogFormOpened(this.GetType().Name);
        }

        // Default constructor for designer support (will be handled by IOC in runtime)
        public CustomerForm() : this(null!, null!)
        {
            // This constructor should not be used at runtime when IOC is configured
            // It's only here for designer support
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string CustomerName
        {
            get => txtName.Text;
            set => txtName.Text = value;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string CustomerEmail
        {
            get => txtEmail.Text;
            set => txtEmail.Text = value;
        }

        public event EventHandler SaveClicked;

        private void btnSave_Click(object sender, EventArgs e)
        {
            _loggingService?.LogInfo($"Customer save clicked - Name: {CustomerName}, Email: {CustomerEmail}");
            SaveFormSettings();
            SaveClicked?.Invoke(this, EventArgs.Empty);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _loggingService?.LogFormClosed(this.GetType().Name);
            Application.Exit();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SaveFormSettings();
            _loggingService?.LogFormClosed(this.GetType().Name);
            base.OnFormClosing(e);
        }

        private void LoadFormSettings()
        {
            if (_configurationService == null) return;

            // Load saved window position and size
            var left = _configurationService.GetSetting($"{this.Name}.Left", this.Left);
            var top = _configurationService.GetSetting($"{this.Name}.Top", this.Top);
            var width = _configurationService.GetSetting($"{this.Name}.Width", this.Width);
            var height = _configurationService.GetSetting($"{this.Name}.Height", this.Height);

            this.SetBounds(left, top, width, height);

            // Load last customer data
            CustomerName = _configurationService.GetSetting("LastCustomer.Name", "");
            CustomerEmail = _configurationService.GetSetting("LastCustomer.Email", "");
        }

        private void SaveFormSettings()
        {
            if (_configurationService == null) return;

            // Save window position and size
            _configurationService.SetSetting($"{this.Name}.Left", this.Left);
            _configurationService.SetSetting($"{this.Name}.Top", this.Top);
            _configurationService.SetSetting($"{this.Name}.Width", this.Width);
            _configurationService.SetSetting($"{this.Name}.Height", this.Height);

            // Save current customer data
            _configurationService.SetSetting("LastCustomer.Name", CustomerName);
            _configurationService.SetSetting("LastCustomer.Email", CustomerEmail);

            _configurationService.SaveSettings();
        }
    }
}
