using System.Collections.Generic;
using System.Threading.Tasks;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Models.Static;
using Microsoft.AspNetCore.Mvc;
using KokazGoodsTransfer.Services.Interfaces;
using KokazGoodsTransfer.DAL.Helper;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class OrderController : AbstractEmployeePolicyController
    {
        private readonly IIndexService<MoenyPlaced> _moneyPlacedIndexService;
        private readonly IIndexService<OrderPlaced> _orderPlacedIndexService;
        private readonly IOrderService _orderService;
        private readonly IAgentPrintService _agentPrintService;
        public OrderController(IIndexService<MoenyPlaced> moneyPlacedIndexService, IIndexService<OrderPlaced> orderPlacedIndexService, IOrderService orderService, IAgentPrintService agentPrintService)
        {

            _moneyPlacedIndexService = moneyPlacedIndexService;
            _orderPlacedIndexService = orderPlacedIndexService;
            _orderService = orderService;
            _agentPrintService = agentPrintService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagingDto pagingDto, [FromQuery] OrderFilter orderFilter)
        {
            var result = await _orderService.GetOrderFiltered(pagingDto, orderFilter);
            return Ok(new { data = result.Data, total = result.Total });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrdersFromEmployee createOrdersFromEmployee)
        {
            await _orderService.CreateOrder(createOrdersFromEmployee);
            return Ok();
        }
        [HttpPost("createMultiple")]
        public async Task<IActionResult> Create([FromBody] List<CreateMultipleOrder> createMultipleOrders)
        {
            await _orderService.CreateOrders(createMultipleOrders);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _orderService.Delete(id);
            return Ok();

        }
        [HttpGet("GetInStockToTransferToSecondBranch")]
        public async Task<ActionResult<PagingResualt<IEnumerable<OrderDto>>>> GetInStockToTransferToSecondBranch([FromQuery] PagingDto pagingDto, [FromQuery] OrderFilter orderFilter)
        {
            return Ok(await _orderService.GetInStockToTransferToSecondBranch(pagingDto, orderFilter));
        }
        [HttpGet("WithoutPaging")]
        public async Task<IActionResult> Get([FromQuery] OrderFilter orderFilter)
        {
            var result = await _orderService.GetOrderFiltered(null, orderFilter);
            return Ok(new { data = result.Data, total = result.Total });
        }
        [HttpPost("ForzenInWay")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> ForzenInWay([FromForm] FrozenOrder frozenOrder)
        {
            return Ok(await _orderService.ForzenInWay(frozenOrder));
        }
        [HttpGet("ReceiptOfTheOrderStatus/{id}")]
        public async Task<ActionResult<GenaricErrorResponse<ReceiptOfTheOrderStatusDetaliDto, string, IEnumerable<string>>>> ReceiptOfTheOrderStatusById(int id)
        {
            return Ok(await _orderService.GetReceiptOfTheOrderStatusById(id));
        }
        [HttpGet("ReceiptOfTheOrderStatus")]
        public async Task<ActionResult<PagingResualt<IEnumerable<ReceiptOfTheOrderStatusDto>>>> GetReceiptOfTheOrderStatus([FromQuery] PagingDto PagingDto, string code)
        {
            return Ok(await _orderService.GetReceiptOfTheOrderStatus(PagingDto, code));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _orderService.GetById(id));
        }
        [HttpGet("orderPlace")]
        public async Task<IActionResult> GetOrderPalce() => Ok(await _orderPlacedIndexService.GetAllLite());
        [HttpGet("MoenyPlaced")]
        public async Task<IActionResult> GetMoenyPlaced() => Ok(await _moneyPlacedIndexService.GetAllLite());
        [HttpPut("MakeOrderInWay")]
        public async Task<ActionResult<GenaricErrorResponse<int, string, string>>> MakeOrderInWay([FromBody] int[] ids)
        {
            var response = await _orderService.MakeOrderInWay(ids);
            return Ok(response);
        }
        [HttpPut("TransferToSecondBranch")]
        public async Task<IActionResult> TransferToSecondBranch([FromBody] int[] ids)
        {
            await _orderService.TransferToSecondBranch(ids);
            return Ok();
        }
        [HttpGet("GetOrderComeToBranch")]
        public async Task<ActionResult<PagingResualt<IEnumerable<OrderDto>>>> GetOrderComeToBranch(PagingDto pagingDto, OrderFilter orderFilter)
        {
            return Ok(await _orderService.GetOrderComeToBranch(pagingDto, orderFilter));
        }
        [HttpPut("ReceiptOfTheStatusOfTheDeliveredShipment")]
        public async Task<ActionResult<ErrorResponse<string, IEnumerable<string>>>> ReceiptOfTheStatusOfTheDeliveredShipment(IEnumerable<ReceiptOfTheStatusOfTheDeliveredShipmentWithCostDto> receiptOfTheStatusOfTheDeliveredShipmentWithCostDtos)
        {
            return Ok(await _orderService.ReceiptOfTheStatusOfTheDeliveredShipment(receiptOfTheStatusOfTheDeliveredShipmentWithCostDtos));
        }
        [HttpPut("ReceiptOfTheStatusOfTheReturnedShipment")]
        public async Task<ActionResult<ErrorResponse<string, IEnumerable<string>>>> ReceiptOfTheStatusOfTheReturnedShipment(IEnumerable<ReceiptOfTheStatusOfTheDeliveredShipmentDto> receiptOfTheStatusOfTheDeliveredShipmentDtos)
        {
            return Ok(await _orderService.ReceiptOfTheStatusOfTheReturnedShipment(receiptOfTheStatusOfTheDeliveredShipmentDtos));
        }
        [HttpGet("OrdersDontFinished")]
        public async Task<ActionResult<IEnumerable<PayForClientDto>>> Get([FromQuery] OrderDontFinishedFilter orderDontFinishedFilter)
        {
            return Ok(await _orderService.OrdersDontFinished(orderDontFinishedFilter));
        }



        [HttpGet("chekcCode")]
        public async Task<ActionResult<bool>> CheckCode([FromQuery] string code, int clientid)
        {
            return Ok(await _orderService.Any(c => c.ClientId == clientid && c.Code == code));
        }
        [HttpPost("CheckMulieCode/{clientId}")]
        public async Task<IActionResult> CheckMulieCode(int clientId, [FromBody] string[] codes)
        {
            return Ok(await _orderService.GetCodeStatuses(clientId, codes));
        }

        [HttpGet("NewOrders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetNewOrders()
        {
            var orders = await _orderService.GetAll(c => c.IsSend == true && c.OrderplacedId == (int)OrderplacedEnum.Client, new string[] { "Client.ClientPhones", "Client.Country", "Region", "Country.AgentCountries.Agent", "OrderItems.OrderTpye" });
            return Ok(orders);
        }

        [HttpGet("NewOrderDontSned")]
        public async Task<IActionResult> NewOrderDontSned()
        {
            return Ok(await _orderService.NewOrderDontSned());
        }

        [HttpGet("OrderAtClient")]
        public async Task<IActionResult> OrderAtClient([FromQuery] OrderFilter orderFilter)
        {
            return Ok(await _orderService.OrderAtClient(orderFilter));
        }
        [HttpGet("GetOrderToReciveFromAgent/{code}")]
        public async Task<ActionResult<GenaricErrorResponse<IEnumerable<OrderDto>, string, string>>> GetOrderToReciveFromAgent(string code)
        {
            return Ok(await _orderService.GetOrderToReciveFromAgent(code));
        }
        [HttpGet("GetOrderForPayBy/{clientId}/{code}")]
        public async Task<ActionResult<PayForClientDto>> GetByCodeAndClient(int clientId, string code)
        {
            return Ok(await _orderService.GetByCodeAndClient(clientId, code));
        }
        [HttpPut("ReiveMoneyFromClient")]
        public async Task<IActionResult> ReiveMoneyFromClient([FromBody] int[] ids)
        {
            await _orderService.ReiveMoneyFromClient(ids);
            return Ok();
        }
        [HttpGet("ShipmentsNotReimbursedToTheClient/{clientId}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> ShipmentsNotReimbursedToTheClient(int clientId)
        {
            var includes = new string[] { "Agent.UserPhones", "Region", "Country", "MoenyPlaced", "Orderplaced" };
            var orders = await _orderService.GetAsync(c => c.ClientId == clientId && c.IsClientDiliverdMoney == false && c.OrderplacedId >= (int)OrderplacedEnum.Way, includes);
            return Ok(orders);
        }
        [HttpGet("GetEarnings")]
        public async Task<IActionResult> GetEarnings([FromQuery] PagingDto pagingDto, [FromQuery] DateFiter dateFiter)
        {
            return Ok(await _orderService.GetEarnings(pagingDto, dateFiter));
        }
        /// <summary>
        /// طلبات في ذمة المندوب
        /// </summary>
        /// <param name="agnetId"></param>
        /// <returns></returns>
        [HttpGet("OrderVicdanAgent/{agnetId}")]
        public async Task<IActionResult> OrderVicdanAgent(int agnetId)
        {
            var includes = new string[] { "Client", "Region", "Country", "Orderplaced", "MoenyPlaced" };
            var orders = await _orderService.GetAsync(c => c.AgentId == agnetId && c.AgentRequestStatus == (int)AgentRequestStatusEnum.Pending || (c.MoenyPlacedId == (int)MoneyPalcedEnum.WithAgent) || (c.IsClientDiliverdMoney == true && c.OrderplacedId == (int)OrderplacedEnum.Way), includes);
            return Ok(orders);
        }

        [HttpPut("Accept")]
        public async Task<IActionResult> Accept([FromBody] IdsDto idsDto)
        {
            await _orderService.Accept(idsDto);
            return Ok();
        }
        [HttpPut("Acceptmultiple")]
        public async Task<IActionResult> AcceptMultiple([FromBody] List<IdsDto> idsDto)
        {
            await _orderService.AcceptMultiple(idsDto);
            return Ok();
        }

        [HttpPut("DisAccept")]
        public async Task<IActionResult> DisAccept([FromBody] DateWithId<int> dateWithId)
        {
            await _orderService.DisAccept(dateWithId);
            return Ok();
        }

        [HttpPut("DisAcceptmultiple")]
        public async Task<IActionResult> DisAcceptMultiple([FromBody] DateWithId<List<int>> dateWithIds)
        {
            await _orderService.DisAcceptMultiple(dateWithIds);
            return Ok();
        }

        [HttpPut("ReSend")]
        public async Task<IActionResult> ReSend([FromBody] OrderReSend orderReSend)
        {
            await _orderService.ReSend(orderReSend);
            return Ok();
        }

        [HttpPut("MakeStoreOrderCompletelyReturned")]
        public async Task<ActionResult<OrderDto>> MakeOrderCompletelyReturned([FromBody] int id)
        {
            return Ok(await _orderService.MakeOrderCompletelyReturned(id));
        }
        [HttpPatch("AddPrintNumber/{orderId}")]
        public async Task<IActionResult> AddPrintNumber(int orderId)
        {
            await _orderService.AddPrintNumber(orderId);
            return Ok();
        }
        [HttpPatch("AddPrintNumberMultiple")]
        public async Task<IActionResult> AddPrintNumber([FromBody] int[] orderids)
        {
            await _orderService.AddPrintNumber(orderids);
            return Ok();
        }
        [HttpGet("GetOrderByAgent/{orderCode}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrderByAgent(string orderCode)
        {
            return Ok(await _orderService.GetOrderByAgent(orderCode));
        }

        [HttpPut("TransferOrderToAnotherAgnet")]
        public async Task<IActionResult> TransferOrderToAnotherAgnet([FromBody] TransferOrderToAnotherAgnetDto transferOrderToAnotherAgnetDto)
        {
            await _orderService.TransferOrderToAnotherAgnet(transferOrderToAnotherAgnetDto);
            return Ok();
        }
        [HttpPatch]
        public async Task<IActionResult> Edit([FromBody] UpdateOrder updateOrder)
        {
            await _orderService.Edit(updateOrder);
            return Ok();

        }

        /// <summary>
        /// تسديد العميل
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut("DeleiverMoneyForClient")]
        public async Task<IActionResult> DeleiverMoneyForClient([FromBody] DeleiverMoneyForClientDto deleiverMoneyForClientDto)
        {
            var id = await _orderService.DeleiverMoneyForClient(deleiverMoneyForClientDto);
            return Ok(new { printNumber = id });
        }
        /// <summary>
        /// تسديد الشركات
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut("DeleiverMoneyForClientWithStatus")]
        public async Task<IActionResult> DeleiverMoneyForClientWithStatus(int[] ids)
        {
            var id = await _orderService.DeleiverMoneyForClientWithStatus(ids);
            return Ok(new { printNumber = id });
        }
        [HttpGet("GetOrderByClientPrintNumber")]
        public async Task<IActionResult> GetOrderByClientPrintNumber([FromQuery] int printNumber)
        {
            return Ok(await _orderService.GetOrderByClientPrintNumber(printNumber));
        }

        [HttpGet("GetClientprint")]
        public async Task<IActionResult> GetClientprint([FromQuery] PagingDto pagingDto, [FromQuery] int? number, string clientName, string code)
        {
            return Ok(await _orderService.GetClientprint(pagingDto, number, clientName, code));
        }
        [HttpGet("DisAccept")]
        public async Task<IActionResult> DisAccpted([FromQuery] PagingDto pagingDto, [FromQuery] OrderFilter orderFilter)
        {
            return Ok(await _orderService.DisAccpted(pagingDto, orderFilter));
        }

        [HttpGet("GetAgentPrint")]
        public async Task<IActionResult> GetAgentPrint([FromQuery] PagingDto pagingDto, [FromQuery] int? number, string agnetName)
        {
            return Ok(await _agentPrintService.GetAgentPrint(pagingDto, number, agnetName));
        }
        [HttpGet("GetOrderByAgnetPrintNumber")]
        public async Task<IActionResult> GetOrderByAgnetPrintNumber([FromQuery] int printNumber)
        {
            return Ok(await _agentPrintService.GetOrderByAgnetPrintNumber(printNumber));
        }

        [HttpGet("OrderRequestEditState")]
        public async Task<IActionResult> OrderRequestEditState()
        {
            return Ok(await _orderService.GetOrderRequestEditState());
        }

        [HttpPut("DisAproveOrderRequestEditState")]
        public async Task<IActionResult> DisAproveOrderRequestEditState([FromBody] int[] ids)
        {
            await _orderService.DisAproveOrderRequestEditState(ids);
            return Ok();
        }

        [HttpPut("AproveOrderRequestEditState")]
        public async Task<IActionResult> AproveOrderRequestEditState([FromBody] int[] ids)
        {
            await _orderService.AproveOrderRequestEditState(ids);
            return Ok();
        }
        [HttpGet("GetCreatedByNames")]
        public async Task<ActionResult<IEnumerable<string>>> GetCreatedByNames()
        {
            return Ok(await _orderService.GetCreatedByNames());
        }
        [HttpGet("ReSendMultiple")]
        public async Task<IActionResult> ReSendMultiple([FromQuery] string code)
        {
            return Ok(await _orderService.GetForReSendMultiple(code));
        }
        [HttpPut("ReSendMultiple")]
        public async Task<IActionResult> ReSendMultiple(List<OrderReSend> orderReSends)
        {
            await _orderService.ReSendMultiple(orderReSends);
            return Ok();
        }
    }
}