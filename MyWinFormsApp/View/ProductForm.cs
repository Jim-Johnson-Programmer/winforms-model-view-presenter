using MyWinFormsApp.Services;
using MyWinFormsApp.View;

namespace MyWinFormsApp.View
{
  public partial class ProductForm : Form
  {
    private readonly ILoggingService _loggingService;
    private readonly IConfigurationService _configurationService;

    public ProductForm(ILoggingService loggingService, IConfigurationService configurationService)
    {
      _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
      _configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
      
      InitializeComponent();
      LoadSettings();
      
      _loggingService.LogFormOpened(this.GetType().Name);
    }

    // Default constructor for designer support
    public ProductForm() : this(null!, null!)
    {
    }

    private void InitializeComponent()
    {
      this.Text = "Product Management";
      this.Size = new Size(400, 300);
      this.StartPosition = FormStartPosition.CenterParent;

      // Add some basic controls for demonstration
      var lblProduct = new Label
      {
        Text = "Product Name:",
        Location = new Point(10, 20),
        Size = new Size(100, 23)
      };

      var txtProduct = new TextBox
      {
        Name = "txtProductName",
        Location = new Point(120, 20),
        Size = new Size(200, 23)
      };

      var lblPrice = new Label
      {
        Text = "Price:",
        Location = new Point(10, 50),
        Size = new Size(100, 23)
      };

      var txtPrice = new NumericUpDown
      {
        Name = "txtPrice",
        Location = new Point(120, 50),
        Size = new Size(200, 23),
        DecimalPlaces = 2,
        Maximum = 999999,
        Minimum = 0
      };

      var btnSave = new Button
      {
        Text = "Save Product",
        Location = new Point(120, 90),
        Size = new Size(100, 30),
        UseVisualStyleBackColor = true
      };
      btnSave.Click += BtnSave_Click;

      var btnCancel = new Button
      {
        Text = "Cancel",
        Location = new Point(230, 90),
        Size = new Size(75, 30),
        UseVisualStyleBackColor = true
      };
      btnCancel.Click += (s, e) => this.Close();

      this.Controls.AddRange(new Control[] { lblProduct, txtProduct, lblPrice, txtPrice, btnSave, btnCancel });
    }

    private void BtnSave_Click(object sender, EventArgs e)
    {
      var productName = ((TextBox)this.Controls["txtProductName"]).Text;
      var price = ((NumericUpDown)this.Controls["txtPrice"]).Value;
      
      if (string.IsNullOrWhiteSpace(productName))
      {
        MessageBox.Show("Please enter a product name.", "Validation Error", 
            MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      _loggingService?.LogInfo($"Product saved - Name: {productName}, Price: {price:C}");
      
      MessageBox.Show($"Product '{productName}' saved successfully!", "Success", 
          MessageBoxButtons.OK, MessageBoxIcon.Information);
      
      SaveSettings(productName, price);
    }

    private void LoadSettings()
    {
      if (_configurationService == null) return;

      var lastProduct = _configurationService.GetSetting("LastProduct.Name", "");
      var lastPrice = _configurationService.GetSetting("LastProduct.Price", 0m);

      if (this.Controls["txtProductName"] is TextBox txtProduct)
        txtProduct.Text = lastProduct;

      if (this.Controls["txtPrice"] is NumericUpDown txtPrice)
        txtPrice.Value = lastPrice;
    }

    private void SaveSettings(string productName, decimal price)
    {
      if (_configurationService == null) return;

      _configurationService.SetSetting("LastProduct.Name", productName);
      _configurationService.SetSetting("LastProduct.Price", price);
      _configurationService.SaveSettings();
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
      _loggingService?.LogFormClosed(this.GetType().Name);
      base.OnFormClosing(e);
    }
  }
}