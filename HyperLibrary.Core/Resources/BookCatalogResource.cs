using System.Collections.Generic;

namespace HyperLibrary.Core.Resources
{
    public class BookCatalogResource : LinkableResource
    {
        public IList<BookResource> Catalog { get; set; }
    }

}