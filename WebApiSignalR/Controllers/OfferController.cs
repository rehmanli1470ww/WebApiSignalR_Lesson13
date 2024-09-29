using Microsoft.AspNetCore.Mvc;
using WebApiSignalR.Helpers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiSignalR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController : ControllerBase
    {
        public OfferController()
        {
            if (!System.IO.File.Exists("bmw.txt")) 
            {
                FileHelper.Write("bmw",11000);
            }
            if (!System.IO.File.Exists("chevrolet.txt"))
            {
                FileHelper.Write("chevrolet", 9700);
            }
        }

        // GET: api/<OfferController>
        [HttpGet]
        public double Get()
        {
            return FileHelper.Read();
        }

        [HttpGet("Room")]
        public double Get(string room)
        {
            return FileHelper.Read(room);
        }

        [HttpGet("IncreaseRoom")]
        public void Increase(string room,double data)
        {
            var result=FileHelper.Read(room)+data;
            FileHelper.Write(room,result);
        }

        [HttpGet("Increase")]
        public void Increase(double data)
        {
            var result = FileHelper.Read() + data;
            FileHelper.Write(result);
        }
    }
}
