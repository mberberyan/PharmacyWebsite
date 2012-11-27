using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Melon.Components.ProductCatalog.Providers;

namespace Melon.Components.ProductCatalog
{
    /// <summary>
    /// Summary description for CustomProductCatalogUserProvider
    /// </summary>
    public class CustomProductCatalogUserProvider: UserProvider
    {
        public CustomProductCatalogUserProvider()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public override User Load()
        {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public override UserDataTable List(User user)
        {
            throw new System.Exception("The method or operation is not implemented.");
        }
    }
}
