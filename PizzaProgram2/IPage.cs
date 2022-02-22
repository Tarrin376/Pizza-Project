using System.Windows;
using System.Windows.Controls;

namespace PizzaProgram2
{
    public interface IPage
    {
        void DeleteItem(object sender, RoutedEventArgs? e = null); // Used to delete an item from the user's order.
        void ChangeContent(); // Responsible for changing the content on the user's page depending on which page they are on.
        Button CreateButton(string content); // Allows each page to add an item to the order list.
    }
}
