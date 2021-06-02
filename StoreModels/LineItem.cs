namespace StoreModels
{

    public class LineItem
    {
        public LineItem(int productId, int quantity, int orderID)
        {
            this.ProductID = productId;
            this.Quantity = quantity;
            this.OrderID = orderID;
        }

        public LineItem()
        {

        }

        public LineItem(int lineItemId, int productId, int quantity, int orderID) : this(productId, quantity, orderID)
        {
            this.LineItemID = lineItemId;
        }

        public int Quantity { get; set; }
        public int LineItemID { get; set; }
        public int ProductID { get; set; }
        public int OrderID { get; set; }



        public override string ToString()
        {
            return $"{Quantity}";
        }
    }
}