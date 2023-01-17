using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TakraonlineCRM.Shared.Graphics;

namespace TakraonlineCRM.Client.DataInterface
{
    public interface IGraphicRepository
    {
        Task<IList<Graphic>> GetByOrderId(int orderId);
        Task<Graphic> GetByGraphicId( int graphicID );
        Task<Graphic> CreateGraphic(Graphic graphic);
        Task<Graphic> EditGraphic(Graphic graphic);
        void DeleteGraphic(int id);
    }
}
