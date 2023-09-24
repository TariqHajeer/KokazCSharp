using System.Collections.Generic;
using System.Threading.Tasks;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Dtos.OrdersDtos;
using Quqaz.Web.Models.Static;
using Microsoft.AspNetCore.Mvc;
using Quqaz.Web.Services.Interfaces;
using Quqaz.Web.DAL.Helper;
using Wkhtmltopdf.NetCore;
using Quqaz.Web.Dtos.OrdersDtos.OrderWithBranchDto;
using Quqaz.Web.Dtos.OrdersDtos.Queries;
using Quqaz.Web.Models;
using Quqaz.Web.Dtos.OrdersDtos.Commands;
using System;
using Microsoft.AspNetCore.Authorization;

namespace Quqaz.Web.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class OrderController : AbstractEmployeePolicyController
    {
        private readonly IOrderService _orderService;
        private readonly IAgentPrintService _agentPrintService;
        private readonly IGeneratePdf _generatePdf;
        public OrderController(IOrderService orderService, IAgentPrintService agentPrintService, IGeneratePdf generatePdf)
        {
            _orderService = orderService;
            _agentPrintService = agentPrintService;
            _generatePdf = generatePdf;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagingDto pagingDto, [FromQuery] OrderFilter orderFilter)
        {
            var result = await _orderService.GetOrderFiltered(pagingDto, orderFilter);
            return Ok(new { data = result.Data, total = result.Total });
        }
#if DEBUG
        [AllowAnonymous]
        [HttpGet("TestCreateMultile")]
        public async Task<IActionResult> TestCreateMultile([FromQuery] int startCode)
        {
            var c = new List<CreateMultipleOrder>();
            for (int i = startCode; i < startCode + 30; i++)
            {
                c.Add(new CreateMultipleOrder()
                {
                    ClientId = 1,
                    Code = i.ToString(),
                    CountryId = 8,
                    RecipientPhones = "65465465465",
                    Cost = 9000,
                    DeliveryCost = 5000,
                    Date = DateTime.UtcNow
                });
            }
            await _orderService.CreateOrders(c);
            return Ok();
        }
#endif
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderFromEmployee createOrdersFromEmployee)
        {
            createOrdersFromEmployee.BranchId = null;
            await _orderService.CreateOrder(createOrdersFromEmployee);
            return Ok();
        }
        [HttpGet("GetOrdersByAgentRegionAndCode")]
        public async Task<IActionResult> GetOrdersByAgentRegionAndCode([FromQuery] GetOrdersByAgentRegionAndCodeQuery getOrderByAgentRegionAndCode)
        {
            return Ok(await _orderService.GetOrdersByAgentRegionAndCode(getOrderByAgentRegionAndCode));
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
        [HttpPost("GetInStockToTransferToSecondBranch")]
        public async Task<ActionResult<PagingResualt<IEnumerable<OrderDto>>>> GetInStockToTransferToSecondBranch([FromBody] SelectedOrdersWithFitlerDto selectedOrdersWithFitlerDto)
        {
            return Ok(await _orderService.GetInStockToTransferToSecondBranch(selectedOrdersWithFitlerDto));
        }
        [HttpGet("GetInStockToTransferWithAgent")]
        public async Task<ActionResult<PagingResualt<IEnumerable<OrderDto>>>> GetInStockToTransferWithAgent([FromQuery] PagingDto pagingDto, [FromQuery] OrderFilter orderFilter)
        {
            return Ok(await _orderService.GetInStockToTransferWithAgent(pagingDto, orderFilter));
        }
        [HttpGet("WithoutPaging")]
        public async Task<IActionResult> Get([FromQuery] OrderFilter orderFilter)
        {
            var result = await _orderService.GetOrderFiltered(null, orderFilter);
            return Ok(new { data = result.Data, total = result.Total });
        }
        [HttpPost("ForzenInWay")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> ForzenInWay([FromQuery] PagingDto paging, [FromBody] FrozenOrder frozenOrder)
        {
            return Ok(await _orderService.ForzenInWay(paging, frozenOrder));
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
        [HttpPut("MakeOrderInWay")]
        public async Task<ActionResult<int>> MakeOrderInWay([FromBody] MakeOrderInWayDto makeOrderInWayDto)
        {
            var response = await _orderService.MakeOrderInWay(makeOrderInWayDto);
            return Ok(response);
        }
        [HttpPut("TransferToSecondBranch")]
        public async Task<ActionResult<int>> TransferToSecondBranch([FromBody] TransferToSecondBranchDto transferToSecondBranchDto)
        {
            return Ok(await _orderService.TransferToSecondBranch(transferToSecondBranchDto));
        }
        [HttpGet("GetPrintsTransferToSecondBranch")]
        public async Task<ActionResult<PagingResualt<IEnumerable<TransferToSecondBranchReportDto>>>> GetPrintsTransferToSecondBranch([FromQuery] PagingDto pagingDto, int destinationBranchId)
        {
            return Ok(await _orderService.GetPrintsTransferToSecondBranch(pagingDto, destinationBranchId));
        }
        [HttpGet("GetPrintTransferToSecondBranchDetials")]
        public async Task<ActionResult<PagingResualt<IEnumerable<TransferToSecondBranchDetialsReportDto>>>> GetPrintTransferToSecondBranchDetials([FromQuery] int id, [FromQuery] PagingDto pagingDto)
        {
            return Ok(await _orderService.GetPrintTransferToSecondBranchDetials(pagingDto, id));
        }
        [HttpGet("PrintTransferToSecondBranch/{id}")]
        public async Task<IActionResult> PrintTransferToSecondBranch(int id)
        {
            var txt = await _orderService.GetTransferToSecondBranchReportAsString(id);
            _generatePdf.SetConvertOptions(new ConvertOptions()
            {
                PageSize = Wkhtmltopdf.NetCore.Options.Size.A4,

                PageMargins = new Wkhtmltopdf.NetCore.Options.Margins() { Bottom = 10, Left = 10, Right = 10, Top = 10 },

            });
            var pdfBytes = _generatePdf.GetPDF(txt);
            string fileName = "نقل إلى الطريق فرع برقم" + id + ".pdf";
            return File(pdfBytes, System.Net.Mime.MediaTypeNames.Application.Pdf, fileName);
        }
        /// <summary>
        /// الشحنات القادمة من فرع آخر
        /// </summary>
        /// <param name="pagingDto"></param>
        /// <param name="orderFilter"></param>
        /// <returns></returns>
        [HttpGet("GetOrdersComeToMyBranch")]
        public async Task<ActionResult<PagingResualt<IEnumerable<OrderDto>>>> GetOrderComeToBranch([FromQuery] PagingDto pagingDto, [FromQuery] OrderFilter orderFilter)
        {
            return Ok(await _orderService.GetOrdersComeToMyBranch(pagingDto, orderFilter));
        }
        [HttpPost("GetOrdersReturnedToSecondBranch")]
        public async Task<IActionResult> GetOrdersReturnedToSecondBranch([FromBody] SelectedOrdersWithFitlerDto selectedOrdersWithFitlerDto)
        {
            return Ok(await _orderService.GetOrdersReturnedToSecondBranch(selectedOrdersWithFitlerDto));
        }
        [HttpGet("GetOrdersReturnedToMyBranch")]
        public async Task<ActionResult<PagingResualt<IEnumerable<OrderDto>>>> GetOrdersReturnedToMyBranch([FromQuery] PagingDto pagingDto, [FromQuery] int currentBranchId)
        {
            return Ok(await _orderService.GetOrdersReturnedToMyBranch(pagingDto, currentBranchId));
        }
        [HttpPost("ReceiveReturnedToMyBranch")]
        public async Task<ActionResult> ReceiveReturnedToMyBranch([FromBody] SelectedOrdersWithFitlerDto selectedOrdersWithFitlerDto)
        {
            await _orderService.ReceiveReturnedToMyBranch(selectedOrdersWithFitlerDto);
            return Ok();
        }
        [HttpPut("DisApproveReturnedToMyBranch")]
        public async Task<ActionResult> DisApproveReturnedToMyBranch([FromBody] OrderIdAndNote orderIdAndNote)
        {
            await _orderService.DisApproveReturnedToMyBranch(orderIdAndNote);
            return Ok();
        }
        [HttpPut("ReceiveOrdersToMyBranch")]
        public async Task<IActionResult> ReceiveOrdersToMyBranch(ReceiveOrdersToMyBranchDto receiveOrdersToMyBranch)
        {
            await _orderService.ReceiveOrdersToMyBranch(receiveOrdersToMyBranch);
            return Ok();
        }
        [HttpPut("DisApproveOrderComeToMyBranch")]
        public async Task<IActionResult> DisApproveOrderComeToMyBranch([FromBody] int id)
        {
            await _orderService.DisApproveOrderComeToMyBranch(id);
            return Ok(id);
        }
        [HttpGet("GetDisApproveOrdersReturnByBranch")]
        public async Task<IActionResult> GetDisApproveOrdersReturnByBranch([FromQuery] PagingDto pagingDto)
        {
            return Ok(await _orderService.GetDisApprovedOrdersReturnedByBranch(pagingDto));
        }
        [HttpPost("SetDisApproveOrdersReturnByBranchInStore")]
        public async Task<IActionResult> SetDisApproveOrdersReturnByBranchInStore([FromBody] SelectedOrdersWithFitlerDto selectedOrdersWithFitlerDto)
        {
            await _orderService.SetDisApproveOrdersReturnByBranchInStore(selectedOrdersWithFitlerDto);
            return Ok();
        }

        [HttpPut("SendOrdersReturnedToSecondBranch")]
        public async Task<IActionResult> SendOrdersReturnedToSecondBranch([FromBody] ReturnOrderToMainBranchDto returnOrderToMainBranchDto)
        {
            return Ok(await _orderService.SendOrdersReturnedToSecondBranch(returnOrderToMainBranchDto));
        }
        [HttpGet("PrintSendOrdersReturnedToSecondBranchReport/{id}")]
        public async Task<IActionResult> PrintSendOrdersReturnedToSecondBranchReport(int id)
        {
            var txt = await _orderService.GetSendOrdersReturnedToSecondBranchReportAsString(id);
            _generatePdf.SetConvertOptions(new ConvertOptions()
            {
                PageSize = Wkhtmltopdf.NetCore.Options.Size.A4,

                PageMargins = new Wkhtmltopdf.NetCore.Options.Margins() { Bottom = 10, Left = 10, Right = 10, Top = 10 },

            });
            var pdfBytes = _generatePdf.GetPDF(txt);
            string fileName = "إعادة الطلبات المترجعة برقم" + id + ".pdf";
            return File(pdfBytes, System.Net.Mime.MediaTypeNames.Application.Pdf, fileName);
        }
        /// <summary>
        /// استلام الشحنات المستلمة
        /// </summary>
        /// <param name="receiptOfTheStatusOfTheDeliveredShipmentWithCostDtos"></param>
        /// <returns></returns>
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
        /// <summary>
        /// تسديد العميل
        /// </summary>
        /// <param name="orderDontFinishedFilter"></param>
        /// <returns></returns>
        [HttpPost("OrdersDontFinished")]
        public async Task<IActionResult> Get([FromBody] OrderDontFinishedFilter orderDontFinishedFilter)
        {
            var (Data, orderCostTotal, deliveryCostTOtal, p) = await _orderService.OrdersDontFinished2(orderDontFinishedFilter, orderDontFinishedFilter.Paging);
            return Ok(new
            {
                Data,
                OrderTotal = orderCostTotal,
                DeliveryTotal = deliveryCostTOtal,
                PayForCientTotal = p
            });
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
            var includes = new string[] { $"{nameof(Order.Client)}.{nameof(Client.ClientPhones)}", $"{nameof(Order.Client)}.{nameof(Client.Country)}", nameof(Order.Region), $"{nameof(Order.Country)}.{nameof(Country.AgentCountries)}.{nameof(AgentCountry.Agent)}", $"{nameof(Order.OrderItems)}.{nameof(OrderItem.OrderTpye)}" };
            var orders = await _orderService.GetAll(c => c.IsSend == true && c.OrderPlace == OrderPlace.Client, includes);
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

            var includes = new string[] { nameof(Order.Client), nameof(Order.Region), nameof(Order.Country) };
            var orders = await _orderService.GetAsync(c => c.AgentId == agnetId && (c.AgentRequestStatus == (int)AgentRequestStatusEnum.Pending || (c.MoneyPlace == MoneyPalce.WithAgent) || (c.IsClientDiliverdMoney == true && c.OrderPlace == OrderPlace.Way)), includes);
            return Ok(orders);
        }

        [HttpPut("Accept")]
        public async Task<IActionResult> Accept([FromBody] OrderIdAndAgentId idsDto)
        {
            await _orderService.Accept(idsDto);
            return Ok();
        }
        [HttpPut("Acceptmultiple")]
        public async Task<IActionResult> AcceptMultiple([FromBody] List<OrderIdAndAgentId> idsDto)
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
        [HttpPatch("EditForOthrBrnach")]
        public async Task<IActionResult> EditForOthrBrnach([FromBody] UpdateOrder updateOrder)
        {
            await _orderService.EditForOthrBrnach(updateOrder);
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
        /// 
        [HttpPut("DeleiverMoneyForClient")]
        public async Task<IActionResult> DeleiverMoneyForClient([FromBody] DeleiverMoneyForClientDto2 deleiverMoneyForClientDto)
        {
            var id = await _orderService.DeleiverMoneyForClient(deleiverMoneyForClientDto);
            return Ok(id);
        }
        [HttpGet("PrintDeleiverMoneyForClient/{id:int}")]
        public async Task<IActionResult> PrintDeleiverMoneyForClient(int id)
        {
            var txt = await _orderService.GetDeleiverMoneyForClient(id);
            _generatePdf.SetConvertOptions(new ConvertOptions()
            {
                PageSize = Wkhtmltopdf.NetCore.Options.Size.A4,

                PageMargins = new Wkhtmltopdf.NetCore.Options.Margins() { Bottom = 10, Left = 10, Right = 10, Top = 10 },

            });
            var pdfBytes = _generatePdf.GetPDF(txt);
            string fileName = "تم تسديد العميل برقم" + id + ".pdf";
            return File(pdfBytes, System.Net.Mime.MediaTypeNames.Application.Pdf, fileName);
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
        public async Task<IActionResult> GetAgentPrint([FromQuery] PagingDto pagingDto, [FromQuery] int? number, string agnetName, string code)
        {
            return Ok(await _agentPrintService.GetAgentPrint(pagingDto, number, agnetName, code));
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
        [HttpGet("GetOrderInAllBranches")]
        public async Task<IActionResult> GetOrderInAllBranches(string code)
        {
            return Ok(await _orderService.GetOrderInAllBranches(code));
        }
        [HttpPost("CreateOrderWithBranch")]
        public async Task<IActionResult> CreateOrderWithBranch(CreateOrderFromEmployee createOrderFromEmployee)
        {
            await _orderService.CreateOrderForOtherBranch(createOrderFromEmployee);
            return Ok();
        }
        [HttpGet("GetNegativeAlert")]
        public async Task<IActionResult> GetNegativeAlert([FromQuery] PagingDto pagingDto, [FromQuery] OrderFilter orderFilter)
        {
            return Ok(await _orderService.GetNegativeAlert(pagingDto, orderFilter));
        }
        [HttpPost("DeleteNegativeAlert")]
        public async Task<IActionResult> DeleteNegativeAlert([FromBody] int id)
        {
            await _orderService.DeleteNegativeAlert(id);
            return Ok();
        }
    }
}