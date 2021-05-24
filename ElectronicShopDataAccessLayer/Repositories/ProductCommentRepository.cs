using ElectronicShopDataAccessLayer.Contexts;
using ElectronicShopDataAccessLayer.Core;
using ElectronicShopDataAccessLayer.Core.BaseRepositories;
using ElectronicShopDataAccessLayer.Models;
using ElectronicShopDataAccessLayer.ViewModels;
using ElectronicShopDataAccessLayer.ViewModels.Product.Comments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShopDataAccessLayer.Repositories
{
    public class ProductCommentRepository : GenericRepositorySql<ProductComment>
    {
        public ProductCommentRepository(DbContextSql context) : base(context)
        {

        }

        public async Task<GetCommentsResponse> GetProductCommentsAsync(GetCommentsRequest filter)
        {
            List<ProductComment> productComments = null;

            if (filter.LastCommentId == 0)
            {
                productComments = await _dbSet
                  .OrderByDescending(pc => pc.Id)
                  .Include(pc => pc.User)
                  .Where(pc => pc.ProductId == filter.ProductId)
                  .Take(filter.Size)
                  .ToListAsync();
            }
            else 
            {
                productComments = await _dbSet
                   .OrderByDescending(pc => pc.Id)
                   .Include(pc => pc.User)
                   .Where(pc => pc.ProductId == filter.ProductId && pc.Id < filter.LastCommentId)
                   .Take(filter.Size)
                   .ToListAsync();
            }


            bool hasMore = true;

            if (productComments.Count < filter.Size)
            {
                hasMore = false;
            }
            else
            { 
                List<ProductComment> pComs = await _dbSet
                .OrderByDescending(pc => pc.Id)
                .Where(pc => pc.ProductId == filter.ProductId && pc.Id < productComments[productComments.Count - 1].Id)
                .Take(filter.Size)
                .ToListAsync();

                if (pComs.Count == 0)
                {
                    hasMore = false;
                }
            }

            foreach (ProductComment pc in productComments)
            {
                pc.Product = null;
                pc.User.Chats1 = null;
                pc.User.Chats2 = null;
                pc.User.ProductComments = null;
            }

            GetCommentsResponse commentsResponse = new GetCommentsResponse
            {
                Comments = productComments,
                HasMore = hasMore
            };

            return commentsResponse;

        }


    }
}
