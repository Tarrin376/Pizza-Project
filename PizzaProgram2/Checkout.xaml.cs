using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PizzaProgram2
{
    /// <summary>
    /// Interaction logic for Checkout.xaml
    /// </summary>
    public partial class Checkout : Window
    {
        public MainWindow page; // Page instance.
        private static readonly Regex cityPattern = new Regex(@"^[a-zA-Z]+$"); // Pattern for checking if the town/city textbox is valid.
        private static readonly Regex postcodePattern = new Regex(@"^([A-Z][A-HJ-Y]?\d[A-Z\d]? ?\d[A-Z]{2}|GIR ?0A{2})$"); // Pattern for checking if the postcode textbox is valid.

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="page">Page instance.</param>
        public Checkout(MainWindow page)
        {
            InitializeComponent();
            this.page = page;
        }

        /// <summary>
        /// Responsible for retrieving order information and displaying it 
        /// on the checkout page. This information includes the order cost, total cost, 
        /// discounts and delivery cost.
        /// </summary>
        public void GetDetails()
        {
            Show();
            AddItems(page.pizzaOrder, pizzasOrdered);
            AddItems(page.sideOrder, sidesOrdered);
            decimal delivery = (page.deliveryApplied == true) ? page.deliveryCost : 0m;
            orderCost.Content = $"£{page.totalCost - delivery}";
            deliveryCost.Content = $"£{delivery}";
            totalAmount.Content = $"£{page.totalCost}";
        }

        /// <summary>
        /// This method is responsible for showing all of the items that the 
        /// user has added to their order. It will add the pizzas to the 'pizza' 
        /// list box have 'yes' or 'no' next to each pizza 
        /// to show if the pizza has extra toppings. It will also insert all of
        /// the sides into the sides listbox. 
        /// </summary>
        /// <param name="listBox">The user's order.</param>
        /// <param name="orderSummary">The list box to copy the order into.</param>
        private void AddItems(ListBox listBox, ListBox orderSummary)
        {
            foreach (Button item in listBox.Items)
            {
                StringBuilder pizzaNum = new StringBuilder(); // Will hold the pizza's number. 
                string? content = item.Content.ToString();
                int index = 0;

                // Responsible for getting the number associated to each pizza.
                while (int.TryParse(content[index].ToString(), out _))
                {
                    pizzaNum.Append(content[index]);
                    index++;
                }

                // The item to be added to the order summary.
                ListBoxItem newItem = new ListBoxItem();
                newItem.Content = item.Content;
                newItem.Foreground = Brushes.White;
                newItem.Content = item.Content;

                int num = (pizzaNum.Length > 0) ? int.Parse(pizzaNum.ToString()) : -1;
                if (pizzaNum.Length > 0)
                {
                    newItem.Content += " (Extra toppings - ";
                    newItem.Content += (page.toppingPage.pizzaNumbers.Values.Contains(num)) ? " Yes)" : " No)";
                }

                // Adds the item to the order summary.
                orderSummary.Items.Add(newItem);
            }
        }

        /// <summary>
        /// This method is used to check if the user has entered all the necessary information
        /// to process and confirm the order. It will call the 'PlaceOrder' function if all information
        /// is entered correctly by the user.
        /// </summary>
        /// <param name="sender">Object that raises this event.</param>
        /// <param name="e">Event arguments.</param>
        private void SubmitOrder(object sender, RoutedEventArgs e)
        {
            bool addressDetails = (page.deliveryApplied == true) ? CheckAddressDetails() : true;
            bool paymentDetails = CheckPaymentDetails();
            if (addressDetails == false || paymentDetails == false) return;
            else PlaceOrder();
        }

        /// <summary>
        /// Method to check whether the user has entered all information accurately so the 
        /// order can be submitted to the chef. It checks the address, town/city and the postcode
        /// that the user has entered.
        /// </summary>
        /// <returns>A boolean flag to identify if all details have been entered correctly by the user.</returns>
        private bool CheckAddressDetails()
        {
            bool allValid = true;
            // Checks if the address is valid.
            if (address.Text.ToString() == string.Empty)
            {
                addressWarning.Content = "Invalid address";
                allValid = false;
            }
            else
            {
                addressWarning.Content = string.Empty;
            }

            // Checks if the city is valid.
            if (cityPattern.IsMatch(town.Text) == false)
            {
                townWarning.Content = "Invalid town or city";
                allValid = false;
            }
            else
            {
                townWarning.Content = string.Empty;
            }

            // Checks if the postcode is valid.
            if (postcodePattern.IsMatch(postcode.Text) == false)
            {
                postcodeWarning.Content = "Invalid postcode";
                allValid = false;
            }
            else
            {
                postcodeWarning.Content = string.Empty;
            }

            return allValid;
        }

        /// <summary>
        /// The purpose of this method is to check if the user has chosen a payment option
        /// from the radio buttons in the payment section. 
        /// </summary>
        /// <returns>A boolean flag indicating if the user has chosen a payment option.</returns>
        private bool CheckPaymentDetails()
        {
            if (card.IsChecked == true || bank.IsChecked == true || cash.IsChecked == true)
            {
                paymentWarning.Content = string.Empty;
                return true;
            }
            else
            {
                paymentWarning.Content = "Please choose a payment method";
                return false;
            }
        }

        /// <summary>
        /// This method is responsible for placing the order requested by the user. It will first check if the
        /// user has ordered anything and if they haven't, an error will be thrown to the user and if not,
        /// the user can submit the order.
        /// </summary>
        public void PlaceOrder()
        {
            if ((page.totalCost - page.deliveryCost == 0 && page.deliveryApplied == true) || page.totalCost == 0)
            {
                orderWarning.Text = "You have not added any items to your order!";
                return;
            }

            orderWarning.Text = string.Empty;
            ProcessOrder();
        }

        /// <summary>
        /// Will open the 'successful order' page when the user has inputted
        /// all required information in the 'checkout page'. 
        /// </summary>
        public void ProcessOrder()
        {
            Close();
            SuccessfullOrder order = new SuccessfullOrder(page, this);
            order.Show();
            ResetContent();
        }

        /// <summary>
        /// Responsible for clearing all of the added content in the main page.
        /// It will reset all values to default to ensure that no bugs occur
        /// if the user decides to complete another order straight after. 
        /// </summary>
        private void ResetContent()
        {
            page.Show();
            page.pizzaOrder.Items.Clear();
            page.toppingOrder.Items.Clear();
            page.sideOrder.Items.Clear();
            page.toppingPage.numPizzas = 0;
            page.totalCost = (page.deliveryApplied == true) ? page.deliveryCost : 0;
            page.toppingPage.pizzaNumbers = new Dictionary<string, int>();
            page.toppingPage.toppingLists = new Dictionary<string, ListBox>();
            page.UpdateTotal();
        }

        /// <summary>
        /// Will reassign the checkoutPage to null (prevents an exception from being raised
        /// when the program is closed).
        /// </summary>
        /// <param name="sender">Object that raises this event.</param>
        /// <param name="e">Event arguments.</param>
        private void WindowClosed(object sender, EventArgs e) =>
            page.Show();
    }
}
