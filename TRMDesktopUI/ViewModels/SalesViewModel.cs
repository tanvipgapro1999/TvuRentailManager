using AutoMapper;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Helpers;
using TRMDesktopUI.Library.Models;
using TRMDesktopUI.Models;

namespace TRMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        IProductEndPoint _productEndPoint;
        IConfigHelper _configHelper;
        ISaleEndPoint _saleEndPoint;
        IMapper _mapper;
        private readonly StatusInfoViewModel _status;
        private readonly IWindowManager _window;

        public SalesViewModel(IProductEndPoint productEndPoint, IConfigHelper configHelper,
            ISaleEndPoint saleEndPoint, IMapper mapper, StatusInfoViewModel status, IWindowManager window) 
        {
            _productEndPoint = productEndPoint;
            _saleEndPoint = saleEndPoint;
            _configHelper = configHelper;
            _mapper = mapper;
            _status = status;
            _window = window;
        }
       
        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            try
            {
                await LoadProducts();
            }
            catch (Exception ex)
            {
                dynamic settings = new ExpandoObject();
                settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                settings.ResizeMode = ResizeMode.NoResize;
                settings.Title = "System Error";
                if (ex.Message == "Unauthorized")
                {
                    _status.UpdateMessage("Unauthorized Access", "You do not have permission to interact with the Sales Form");
                    await _window.ShowDialogAsync(_status, null, settings);
                }
                else 
                {
                    _status.UpdateMessage("Fatal Exception", ex.Message);
                    await _window.ShowDialogAsync(_status, null, settings);
                }
                await TryCloseAsync();
            }
        }

        private async Task LoadProducts()
        {
            var productList = await _productEndPoint.GetAll();
            var products = _mapper.Map<List<ProductDisplayModel>>(productList);
            Products = new BindingList<ProductDisplayModel>(products);
        }

        private BindingList<ProductDisplayModel> _Products;

        public BindingList<ProductDisplayModel> Products
        {
            get { return _Products; }
            set 
            { 
                _Products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        private ProductDisplayModel _selectedProduct;

        public ProductDisplayModel SelectedProduct
        {
            get { return _selectedProduct; }
            set 
            { 
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        private async Task ResetSaleViewModel() 
        {
            Cart = new BindingList<CartItemDisplayModel>();
            // TODO - Add clearing the selectedCartItem if it does not to do it itself
            await LoadProducts();

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
        }

        private CartItemDisplayModel _selectedCartItem;
        // notify remove from cart
        public CartItemDisplayModel SelectedCartItem
        {
            get { return _selectedCartItem; }
            set
            {
                _selectedCartItem = value;
                NotifyOfPropertyChange(() => SelectedCartItem);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
            }
        }
        private BindingList<CartItemDisplayModel> _cart = new BindingList<CartItemDisplayModel>();

        public BindingList<CartItemDisplayModel> Cart
        {
            get { return _cart; }
            set 
            { 
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        private int _itemQuantity = 1;

        public int ItemQuantity
        {
            get { return _itemQuantity; }
            set 
            {
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }


        public string SubTotal
        {
            get 
            {
                // ToString "C" is currency 
                return CaculateSubTotal().ToString("C"); 
            }
        }
        private decimal CaculateSubTotal() 
        {
            decimal subTotal = 0;
            foreach (var item in Cart)
            {
                subTotal += (item.Product.RentailPrice * item.QuantityInCart);
            }
            return subTotal;
        }
        private decimal CaculateTax()
        {
            decimal TaxAmount = 0;
            decimal taxRate = _configHelper.GetTaxRate()/100;
            TaxAmount = Cart
                .Where(x => x.Product.IsTaxable)
                .Sum(x => x.Product.RentailPrice * x.QuantityInCart * taxRate);
            //foreach (var item in Cart)
            //{
            //    if (item.Product.IsTaxable)
            //    {
            //        TaxAmount += (item.Product.RentailPrice * item.QuantityInCart * taxRate);
            //    }
            //}
            return TaxAmount;
        }
        public string Tax
        {
            get
            {
                return CaculateTax().ToString("C");
            }
        }
       
        public string Total
        {
            get
            {
                decimal total = CaculateSubTotal() + CaculateTax();
                return total.ToString("C");
            }
        }
        public bool CanAddToCart 
        {
            get
            {
                bool output = false;
                // Make sure something is selected
                // Make sure there is an item quantity
                // Check make sure SelectedProduct not null
                // ItemQuantity is easy check why put this one first
                if (ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity)
                {
                    output = true;
                }

                return output;
            }
        }
        public void AddToCart() 
        {
            CartItemDisplayModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);
            if (existingItem != null)
            {
                // Add quantity if item exist
                existingItem.QuantityInCart += ItemQuantity;
            }
            else
            {
                CartItemDisplayModel item = new CartItemDisplayModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity
                };
                Cart.Add(item);
            }
            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
            NotifyOfPropertyChange(() => CanAddToCart);

        }
        public bool CanRemoveFromCart
        {
            get
            {
                bool output = false;
                // Make sure something is selected
                if (SelectedCartItem != null && SelectedCartItem?.QuantityInCart > 0)
                {
                    output = true;
                }
                // Make sure there is an item quantity
                return output;
            }
        }
        public void RemoveFromCart()
        {


            SelectedCartItem.Product.QuantityInStock += 1;

            if (SelectedCartItem.QuantityInCart > 1)
            {
                SelectedCartItem.QuantityInCart -= 1;
            }
            else
            {
                Cart.Remove(SelectedCartItem);
            }
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
        }
        public bool CanCheckOut
        {
            get
            {
                bool output = false;
                // Make sure something in Cart
                if (Cart.Count >0)
                {
                    output = true;
                }
                return output;
            }
        }
        public async Task CheckOut()
        {
            // Create a SaleModel and post to the API
            SaleModel sale = new SaleModel();
            foreach (var item in Cart)
            {
                sale.SaleDetails.Add(new SaleDetailModel
                {
                    ProductId = item.Product.Id,
                    Quantity = item.QuantityInCart
                });
            }

            await _saleEndPoint.PostSale(sale);

            await ResetSaleViewModel();
        }
    }
}
