using MyWinFormsApp.Factory;

namespace MyWinFormsApp.View
{
  public partial class MainForm : Form
  {
    private readonly IFormFactory _formFactory;

    public MainForm(IFormFactory formFactory)
    {
      _formFactory = formFactory ?? throw new ArgumentNullException(nameof(formFactory));
      InitializeComponent();
    }

    private void InitializeComponent()
    {
      this.Text = "Main Application - IOC Container Demo";
      this.Size = new Size(500, 400);
      this.StartPosition = FormStartPosition.CenterScreen;

      // Create menu strip
      var menuStrip = new MenuStrip();

      // Forms menu
      var formsMenu = new ToolStripMenuItem("Forms");

      var customerMenuItem = new ToolStripMenuItem("Customer Form", null, (s, e) => ShowCustomerForm());
      var productMenuItem = new ToolStripMenuItem("Product Form", null, (s, e) => ShowProductForm());
      var customerDialogMenuItem = new ToolStripMenuItem("Customer Dialog", null, (s, e) => ShowCustomerDialog());

      formsMenu.DropDownItems.AddRange(new ToolStripItem[]
      {
                customerMenuItem,
                productMenuItem,
                new ToolStripSeparator(),
                customerDialogMenuItem
      });

      // Help menu
      var helpMenu = new ToolStripMenuItem("Help");
      var aboutMenuItem = new ToolStripMenuItem("About", null, (s, e) => ShowAbout());
      helpMenu.DropDownItems.Add(aboutMenuItem);

      menuStrip.Items.AddRange(new ToolStripItem[] { formsMenu, helpMenu });
      this.MainMenuStrip = menuStrip;
      this.Controls.Add(menuStrip);

      // Add some buttons for demonstration
      var btnCustomer = new Button
      {
        Text = "Open Customer Form",
        Location = new Point(50, 80),
        Size = new Size(150, 40),
        UseVisualStyleBackColor = true
      };
      btnCustomer.Click += (s, e) => ShowCustomerForm();

      var btnProduct = new Button
      {
        Text = "Open Product Form",
        Location = new Point(250, 80),
        Size = new Size(150, 40),
        UseVisualStyleBackColor = true
      };
      btnProduct.Click += (s, e) => ShowProductForm();

      var btnCustomerDialog = new Button
      {
        Text = "Customer Dialog",
        Location = new Point(150, 140),
        Size = new Size(150, 40),
        UseVisualStyleBackColor = true
      };
      btnCustomerDialog.Click += (s, e) => ShowCustomerDialog();

      this.Controls.AddRange(new Control[] { btnCustomer, btnProduct, btnCustomerDialog });
    }

    private void ShowCustomerForm()
    {
      try
      {
        // Create and show customer form using factory
        var customerForm = _formFactory.ShowForm<CustomerForm>();
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Error opening Customer Form: {ex.Message}", "Error",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void ShowProductForm()
    {
      try
      {
        // Create and show product form using factory
        var productForm = _formFactory.ShowForm<ProductForm>();
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Error opening Product Form: {ex.Message}", "Error",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void ShowCustomerDialog()
    {
      try
      {
        // Show customer form as dialog using factory
        var result = _formFactory.ShowFormAsDialog<CustomerForm>();
        MessageBox.Show($"Dialog closed with result: {result}", "Dialog Result");
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Error opening Customer Dialog: {ex.Message}", "Error",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void ShowAbout()
    {
      MessageBox.Show("WinForms IOC Container Demo\n\nDemonstrates using dependency injection with a factory pattern for creating forms.",
          "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
  }
}