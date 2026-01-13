using MyWinFormsApp.Model;
using MyWinFormsApp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWinFormsApp.Presenter
{
    public class CustomerPresenter
    {
        private readonly ICustomerView _view;
        private readonly CustomerModel _model;

        public CustomerPresenter(ICustomerView view, CustomerModel model)
        {
            _view = view;
            _model = model;

            // Subscribe to view events
            _view.SaveClicked += OnSaveClicked;
        }

        private void OnSaveClicked(object sender, EventArgs e)
        {
            // Update the model with data from the view
            _model.Name = _view.CustomerName;
            _model.Email = _view.CustomerEmail;

            // Validate data and provide feedback
            if (!_model.IsValidEmail())
            {
                MessageBox.Show("Please enter a valid email address.");
                return;
            }

            // Simulate saving data
            MessageBox.Show($"Customer '{_model.Name}' saved successfully!");
        }
    }
}
