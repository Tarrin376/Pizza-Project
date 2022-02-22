using System.Windows.Shapes;

namespace PizzaProgram2
{
    /// <summary>
    /// Class to set and add nodes to a linked list
    /// so the user can move between pages efficiently.
    /// </summary>
    public class GetPage
    {
        public CurPage? curPage; // The current page that the user is on.
        public MainWindow page; // The page instance so elements can be modified in this class.

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="page">Page instance so elements on the GUI can be accessed.</param>
        public GetPage(MainWindow page)
        {
            this.page = page;
            curPage = null;
            AddProcesses();
        }

        /// <summary>
        /// The current page node that the user is on.
        /// It has two pointers to allow the user to go fowards
        /// and backwards between different stages.
        /// </summary>
        public class CurPage
        {
            public CurPage? nextNode;
            public CurPage? prevNode;
            public Ellipse ellipse;
            public CurPage(Ellipse ellipse)
            {
                this.ellipse = ellipse;
                nextNode = null;
                prevNode = null;
            }
        }

        /// <summary>
        /// Adds the page nodes to the linked list by linking their
        /// next and previous pointers together. 
        /// </summary>
        public void AddProcesses()
        {
            CurPage pizzaNode = new CurPage(page.pizzaProcess);
            CurPage toppingNode = new CurPage(page.toppingProcess);
            CurPage sideNode = new CurPage(page.sideProcess);
            curPage = pizzaNode;
            pizzaNode.nextNode = toppingNode;
            toppingNode.prevNode = pizzaNode;
            toppingNode.nextNode = sideNode;
            sideNode.prevNode = toppingNode;
        }
    }
}