using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.Statics;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Interfaces;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Concret
{
    public class StatisticsService : IStatisticsService
    {
        IRepository<User> _userRepository;
        private readonly IRepository<Client> _clientRepository;
        private readonly IRepository<Order> _orderRepository;
        public StatisticsService(IRepository<User> userRepository, IRepository<Client> clientRepository, IRepository<Order> orderRepository)
        {
            _userRepository = userRepository;
            _clientRepository = clientRepository;
            _orderRepository = orderRepository;
        }
        public async Task<MainStaticsDto> GetMainStatics()
        {
            MainStaticsDto mainStatics = new MainStaticsDto()
            {
                TotalAgent = await _userRepository.Count(c => c.CanWorkAsAgent == true),
                TotalClient= await _clientRepository.Count(),
                
            };
            throw new System.NotImplementedException();
        }
    }
}
