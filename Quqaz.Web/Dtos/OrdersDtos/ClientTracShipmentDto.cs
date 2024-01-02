using Microsoft.CodeAnalysis;

namespace Quqaz.Web.Dtos.OrdersDtos
{

    public class ClientTracShipmentDto
    {
        public int Number { get; set; }
        public bool Checked { get; set; }
        public string Text { get; set; }
        public string ExtraText { get; set; }
    }
}
