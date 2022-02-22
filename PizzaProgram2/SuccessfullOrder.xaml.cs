using System;
using System.Text;
using System.Windows;

namespace PizzaProgram2
{
    /// <summary>
    /// Interaction logic for SuccessfullOrder.xaml
    /// </summary>
    public partial class SuccessfullOrder : Window
    {
        public MainWindow page; // The main GUI page object so elements on the page can be accessed.
        public Checkout checkoutPage; // The checkout page object so data from that page can be accessed.

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="page">Main GUI page</param>
        /// <param name="checkoutPage">Checkout page object</param>
        public SuccessfullOrder(MainWindow page, Checkout checkoutPage)
        {
            this.page = page;
            this.checkoutPage = checkoutPage;
            InitializeComponent();
            GenerateOrderNumber();
        }

        /// <summary>
        /// This method will generate an order number that the
        /// user can use to track their order's progress. The number
        /// will be 10 digits long.
        /// </summary>
        public void GenerateOrderNumber()
        {
            StringBuilder orderNumber = new StringBuilder("#");
            Random randomNumber = new Random();

            for (int i = 0; i < 10; i++) orderNumber.Append(randomNumber.Next(1, 10));
            orderNum.Content = $"Order number: {orderNumber}";
            GetOrderDetails();
        }

        /// <summary>
        /// Grabs all necessary information from the checkout page and main GUI page
        /// that needs to be outputted on the final bill's details. This includes the address,
        /// the total cost of the order, payment method used etc. 
        /// </summary>
        public void GetOrderDetails()
        {
            delivery.Content += (page.deliveryApplied == true) ? "Yes" : "No";
            address.Content += (page.deliveryApplied == true) ? checkoutPage.address.Text : "N/A";
            town.Content += (page.deliveryApplied == true) ? checkoutPage.town.Text : "N/A";
            postcode.Content += (page.deliveryApplied == true) ? checkoutPage.postcode.Text : "N/A";
            totalCost.Content += $"£{page.totalCost}";

            if (checkoutPage.card.IsChecked == true) paymentMethod.Content += "Card";
            else if (checkoutPage.bank.IsChecked == true) paymentMethod.Content += "Bank";
            else paymentMethod.Content += "Cash";
        }
    }
}
