﻿using Catel.Data;
using Catel.Examples.WP7.ShoppingList.Data;
using Catel.MVVM;
using Catel.MVVM.Services;

namespace Catel.Examples.WP7.ShoppingList.ViewModels
{
    /// <summary>
    /// Shop view model.
    /// </summary>
    public class ShopViewModel : ViewModelBase
    {
        #region Variables
        private int _shopIndex = -1;
        #endregion

        #region Constructor & destructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ShopViewModel"/> class.
        /// </summary>
        public ShopViewModel()
        {
            OK = new Command<object, object>(OnOKExecute, OnOKCanExecute);
            Cancel = new Command<object>(OnCancelExecute);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the title of the view model.
        /// </summary>
        /// <value>The title.</value>
        public override string Title { get { return "Shop"; } }

        #region Models
        /// <summary>
        /// Gets or sets the shop.
        /// </summary>
        [Model]
        public IShop Shop
        {
            get { return GetValue<IShop>(ShopProperty); }
            private set { SetValue(ShopProperty, value); }
        }

        /// <summary>
        /// Register the Shop property so it is known in the class.
        /// </summary>
        public static readonly PropertyData ShopProperty = RegisterProperty("Shop", typeof(IShop));
        #endregion

        #region View model
        /// <summary>
        /// Gets or sets the name of the shop.
        /// </summary>
        [ViewModelToModel("Shop")]
        public string Name
        {
            get { return GetValue<string>(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        /// <summary>
        /// Register the Name property so it is known in the class.
        /// </summary>
        public static readonly PropertyData NameProperty = RegisterProperty("Name", typeof(string));
        #endregion
        #endregion

        #region Commands
        /// <summary>
        /// Gets the OK command.
        /// </summary>
        public Command<object, object> OK { get; private set; }

        /// <summary>
        /// Method to check whether the OK command can be executed.
        /// </summary>
        /// <param name="parameter">The parameter of the command.</param>
        private bool OnOKCanExecute(object parameter)
        {
            return !Shop.HasErrors;
        }

        /// <summary>
        /// Method to invoke when the OK command is executed.
        /// </summary>
        /// <param name="parameter">The parameter of the command.</param>
        private void OnOKExecute(object parameter)
        {
            var navigationService = GetService<INavigationService>();
            navigationService.Navigate<ManageShopsViewModel>();
            //navigationService.Navigate("/UI/Pages/ManageShopsPage.xaml");
        }

        /// <summary>
        /// Gets the Cancel command.
        /// </summary>
        public Command<object> Cancel { get; private set; }

        /// <summary>
        /// Method to invoke when the Cancel command is executed.
        /// </summary>
        /// <param name="parameter">The parameter of the command.</param>
        private void OnCancelExecute(object parameter)
        {
            CancelViewModel();

            var navigationService = GetService<INavigationService>();
            navigationService.GoBack();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Called when the navigation has completed.
        /// </summary>
        /// <remarks>
        /// This should of course be a cleaner solution, but there is no other way to let a view-model
        /// know that navigation has completed. Another option is injection, but this would require every
        /// view-model for Windows Phone 7 to accept only the navigation context, which has actually nothing
        /// to do with the logic.
        /// <para/>
        /// It is also possible to use the <see cref="NavigationCompleted"/> event.
        /// </remarks>
        protected override void OnNavigationCompleted()
        {
            if (NavigationContext.ContainsKey("ShopIndex"))
            {
                int.TryParse(NavigationContext["ShopIndex"], out _shopIndex);
            }

            Shop = (_shopIndex != -1) ? UserData.Instance.Shops[_shopIndex] : new Shop();
        }

        /// <summary>
        /// Saves the data.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if successful; otherwise <c>false</c>.
        /// </returns>	
        protected override bool Save()
        {
            if (_shopIndex == -1)
            {
                UserData.Instance.Shops.Add(Shop);
                _shopIndex = UserData.Instance.Shops.IndexOf(Shop);
            }

            return true;
        }
        #endregion
    }
}
