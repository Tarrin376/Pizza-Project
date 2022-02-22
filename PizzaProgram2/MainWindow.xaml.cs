using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace PizzaProgram2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly decimal deliveryCost = 5.50m; // The delivery cost if the user decides to get their food delivered.
        private BrushConverter convert = new BrushConverter(); // Object to convert strings into hexidecimal values.
        public GetPage? pageNode = null; // Current step/node that the user is on (e.g. pizza, toppings, sides etc).
        public bool deliveryApplied = false; // Flag to determine whether the user has selected collection or delivery.
        public Button[] buttons; // Array used to link the buttons between different elements.
        public decimal totalCost = 0; // The total cost of the order (including delivery costs).

        /// <summary>
        /// Page objects used to access the different pages on
        /// the application.
        /// </summary>
        public readonly Pizza pizzaPage;
        public readonly Toppings toppingPage;
        public readonly Sides sidesPage;
        public Checkout? checkoutPage;

        /// <summary>
        /// This constructor is reponisble for setting the first process node to the "pizzaProcess" object.
        /// Initializes the window and applies default styling to the "pizzaProcess" Ellipse and "firstLine" line item.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            pageNode = new GetPage(this);
            pizzaPage = new Pizza(this);
            toppingPage = new Toppings(this);
            sidesPage = new Sides(this);
            buttons = new Button[] { button1, button2, button3, button4, button5, button6 };
            pageNode.curPage.ellipse.Fill = (Brush)convert.ConvertFrom("#22dd97");
            pizzas.Foreground = (Brush)convert.ConvertFrom("#22dd97");
            pageNode.curPage.ellipse.Stroke = Brushes.Black;
            firstLine.Stroke = Brushes.White;
            pizzaPage.ChangeContent();
        }

        /// <summary>
        /// This method will be executed when an item is added to the customer's
        /// cart. It checks which button the event was raised on and will
        /// call the "AddOrder" function after the button's name is obtained so the
        /// program can determine which item the user added.
        /// </summary>
        /// <param name="sender">Object that raises this event.</param>
        /// <param name="e">Event arguments.</param>
        private void AddToCart(object sender, RoutedEventArgs e)
        {
            string name = (sender as Button).Name;
            if (pageNode.curPage.ellipse.Name.Equals(Progress.pizzaProcess.ToString())) pizzaPage.GetComboBox(name);
            if (pageNode.curPage.ellipse.Name.Equals(Progress.toppingProcess.ToString())) toppingPage.GetComboBox(name, sender as Button);
            if (pageNode.curPage.ellipse.Name.Equals(Progress.sideProcess.ToString())) sidesPage.AddItem((Button)sender);
        }

        /// <summary>
        /// This method will run when the user selects the "delivery" button.
        /// It will add an extra delivery fee to the total cost and update it.
        /// Before the delivery fee is applied, the method will change the background
        /// of the "collection" and "delivery" buttons to show the user which button they 
        /// chose.
        /// </summary>
        /// <param name="sender">Object that raises this event.</param>
        /// <param name="e">Event arguments.</param>
        public void ChooseDelivery(object sender, RoutedEventArgs e)
        {
            // To make sure that the user cannot press the button multiple times.
            if (deliveryApplied == false)
            {
                collection.Background = (Brush)convert.ConvertFrom("#4b4c4c");
                collection.BorderBrush = (Brush)convert.ConvertFrom("#22dd97");
                delivery.Background = (Brush)convert.ConvertFrom("#22dd97");
                delivery.BorderBrush = Brushes.Black;
                totalCost += deliveryCost;
                UpdateTotal();
                deliver.Content = $"£{deliveryCost} (including VAT)";
                deliveryApplied = true;
            }
        }

        /// <summary>
        /// Updates the total order cost 
        /// in the main GUI.
        /// </summary>
        public void UpdateTotal() => total.Content = $"£{totalCost}";

        /// <summary>
        /// This method will run when the user selects the "collection" button.
        /// It will remove the delivery fee to the total cost and update it.
        /// Before the delivery fee is removed, the method will change the background
        /// of the "collection" and "delivery" buttons to show the user which button they 
        /// chose.
        /// </summary>
        /// <param name="sender">Object that raises this event.</param>
        /// <param name="e">Event arguments.</param>
        public void ChooseCollection(object sender, RoutedEventArgs e)
        {
            // To make sure that the user cannot press the button multiple times.
            if (deliveryApplied == true)
            {
                collection.Background = (Brush)convert.ConvertFrom("#22dd97");
                collection.BorderBrush = Brushes.Black;
                delivery.Background = (Brush)convert.ConvertFrom("#4b4c4c");
                delivery.BorderBrush = (Brush)convert.ConvertFrom("#22dd97");
                if (totalCost > 0) totalCost -= deliveryCost;
                UpdateTotal();
                deliver.Content = "£0";
                deliveryApplied = false;
            }
        }

        /// <summary>
        /// This method is responsible for changing the image of each image object
        /// on the page when the user moves between the pizza, toppings and sides pages.
        /// It will change the image to the appropriate image for the current page that 
        /// the user is on (e.g. changes a topping image to a pepperoni pizza image when the 
        /// user moves back to the pizza options).
        /// </summary>
        /// <param name="image">Image object to be changed.</param>
        /// <param name="source">New source that will override the previous image source.</param>
        public void ChangeImage(Image image, string source)
        {
            try
            {
                Image myImage = new Image();
                myImage.Width = 200;

                BitmapImage myBitmapImage = new BitmapImage();
                myBitmapImage.BeginInit();
                myBitmapImage.UriSource = new Uri(source);
                myBitmapImage.DecodePixelWidth = 230;
                myBitmapImage.EndInit();
                image.Source = myBitmapImage;
            }
            catch (DirectoryNotFoundException directoryError)
            {
                MessageBox.Show("Certain images were unable to load due to a directory issue." +
                    $"\n\nDetails about error - {directoryError.Message}");
                image.Source = null;
            }
            catch (FileNotFoundException fileError)
            {
                MessageBox.Show("Certain images were unable to load due to a file issue." +
                    $"\n\nDetails about error - {fileError.Message}");
                image.Source = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Certain images were unable to load due to an unexpected issue." +
                    $"\n\nDetails about error - {ex.Message}");
                image.Source = null;
            }
        }

        /// <summary>
        /// Responsible for allowing the user to move onto the next process of the order.
        /// E.g. (pizza choices, topping choices and side choices). It works by using
        /// a linked list to go foward and backward between pointers. Depending on which 
        /// process the user is currently on, the application will move one step foward. 
        /// </summary>
        /// <param name="sender">Object that raises this event.</param>
        /// <param name="e">Event arguments.</param>
        private void GoBack(object sender, RoutedEventArgs e)
        {
            if (pageNode.curPage.prevNode == null) return;
            if (pageNode.curPage.ellipse.Name == Progress.pizzaProcess.ToString()) firstLine.Stroke = Brushes.Black;
            if (pageNode.curPage.ellipse.Name == Progress.toppingProcess.ToString()) secondLine.Stroke = Brushes.Black;

            // Changes the colour of the previous and current element.
            pageNode.curPage.ellipse.Fill = (Brush)convert.ConvertFrom("#4b4c4c");
            pageNode.curPage.ellipse.Stroke = (Brush)convert.ConvertFrom("#22dd97");
            pageNode.curPage = pageNode.curPage.prevNode;
            pageNode.curPage.ellipse.Fill = (Brush)convert.ConvertFrom("#22dd97");
            pageNode.curPage.ellipse.Stroke = Brushes.Black;

            if (pageNode.curPage.ellipse.Name == Progress.pizzaProcess.ToString())
            {
                firstLine.Stroke = Brushes.White;
                pizzas.Foreground = (Brush)convert.ConvertFrom("#22dd97");
                toppings.Foreground = Brushes.White;
                pizzaPage.ChangeContent();
            }

            if (pageNode.curPage.ellipse.Name == Progress.toppingProcess.ToString())
            {
                secondLine.Stroke = Brushes.White;
                toppings.Foreground = (Brush)convert.ConvertFrom("#22dd97");
                sides.Foreground = Brushes.White;
                checkoutButton.IsEnabled = false;
                toppingPage.ChangeContent();
                ModifyButtons(false);
            }
        }

        /// <summary>
        /// Responsible for allowing the user to move back to the previous process of the order.
        /// E.g. (pizza choices, topping choices and side choices). It works by using
        /// a linked list to go foward and backward between pointers. Depending on which 
        /// process the user is currently on, the application will move one step backward.
        /// </summary>
        /// <param name="sender">Object that raises this event.</param>
        /// <param name="e">Event arguments.</param>
        private void GoFoward(object sender, RoutedEventArgs e)
        {
            if (pageNode.curPage.nextNode == null) return;

            // Changes the colour of the previous and current element.
            pageNode.curPage.ellipse.Fill = (Brush)convert.ConvertFrom("#4b4c4c");
            pageNode.curPage.ellipse.Stroke = (Brush)convert.ConvertFrom("#22dd97");
            pageNode.curPage = pageNode.curPage.nextNode;
            pageNode.curPage.ellipse.Fill = (Brush)convert.ConvertFrom("#22dd97");
            pageNode.curPage.ellipse.Stroke = Brushes.Black;

            if (pageNode.curPage.ellipse.Name.Equals(Progress.pizzaProcess.ToString()))
            {
                firstLine.Stroke = Brushes.White;
                pizzas.Foreground = (Brush)convert.ConvertFrom("#22dd97");
                pizzaPage.ChangeContent();
            }

            if (pageNode.curPage.ellipse.Name.Equals(Progress.toppingProcess.ToString()))
            {
                secondLine.Stroke = Brushes.White;
                toppings.Foreground = (Brush)convert.ConvertFrom("#22dd97");
                pizzas.Foreground = Brushes.White;
                toppingPage.ChangeContent();
            }

            if (pageNode.curPage.ellipse.Name.Equals(Progress.sideProcess.ToString()))
            {
                toppings.Foreground = Brushes.White;
                sides.Foreground = (Brush)convert.ConvertFrom("#22dd97");
                checkoutButton.IsEnabled = true;
                sidesPage.ChangeContent();
                ModifyButtons(true);
            }
        }

        /// <summary>
        /// Will replace the combo box's original position when the 'sides' page
        /// is visited by the user. (Essentially just centers the button as combo boxes
        /// are not needed on this page). It will do the opposite when the user goes
        /// back to the 'toppings' page.
        /// </summary>
        /// <param name="foward">Flag indicating if the user is going fowards or backwards</param>
        private void ModifyButtons(bool foward)
        {
            foreach (Button button in buttons)
            {
                Thickness margin = button.Margin;
                if (foward == true) margin.Left -= 160;
                else margin.Left += 160;
                button.Margin = margin;
            }
        }

        /// <summary>
        /// Opens the "checkout" page form so the user can
        /// pay for their order.
        /// </summary>
        /// <param name="sender">Object that raises this event.</param>
        /// <param name="e">Event arguments.</param>
        private void GoToCheckout(object sender, RoutedEventArgs e)
        {
            if (checkoutPage == null) checkoutPage = new Checkout(this);
            Hide();
            checkoutPage.GetDetails();
            checkoutPage = null;
        }

        /// <summary>
        /// Closes the checkout page when the user requests
        /// to exit the program.
        /// </summary>
        /// <param name="sender">Object that raises this event.</param>
        /// <param name="e">Event arguments.</param>
        private void WindowClosed(object sender, EventArgs e)
        {
            if (checkoutPage != null) checkoutPage.Close();
        } 
    }

    /// <summary>
    /// Enumeration used to store the main buttons that are used to
    /// add items to the customer's order.
    /// </summary>
    public enum Buttons
    {
        button1,
        button2,
        button3,
        button4,
        button5,
        button6,
    }

    /// <summary>
    /// Enumeration used to store the names of each of the processes that
    /// the user can go through.
    /// </summary>
    public enum Progress
    {
        pizzaProcess,
        toppingProcess,
        sideProcess,
    }
}
