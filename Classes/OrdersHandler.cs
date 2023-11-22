using System;

public class OrdersHandler
{
    public event Action allOrdersCompleted;
    public event Action<Order> orderGenerated;
    public event Action<int>orderRemovedAt;

    private int _amountLeft;
    private Order[] _currentOrders;
    public OrdersHandler(int ordersAvailable, int ordersInTotal)
    {
        _amountLeft = ordersInTotal;
        _currentOrders = new Order[ordersAvailable];
    }

    public void GenerateOrder()
    {
        for (int i = 0;i<_currentOrders.Length;i++)
        {

            if (_currentOrders[i]==null)
            {
                Order order = new Order();
                orderGenerated.Invoke(order);
                _currentOrders[i] = order;
                return;
            }

        }
    }

    public void CheckOrderComplete(int i, Field field)
    {
        if (_currentOrders[i]==null)
            return;
        
        if (field.TryFindCell(_currentOrders[i].Level,_currentOrders[i].Color, out int x, out int y))
        {
            field.RemoveCellAt(x,y);

            orderRemovedAt.Invoke(i);

            _amountLeft--;

            if (_amountLeft<=0)
                allOrdersCompleted.Invoke();
            else
            {
                _currentOrders[i] = null;
                GenerateOrder();
            }

            
        }
    }
}