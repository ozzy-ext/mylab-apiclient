﻿using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;
using TestServer.Models;

namespace TestServer.Controllers
{
    [ApiController]
    [Route("param-sending")]
    public class ParamSendingTestController : ControllerBase
    {
        [HttpPost("echo/query")]
        public IActionResult EchoQuery([FromQuery]string msg)
        {
            return Ok(msg);
        }

        [HttpPost("echo/{msg}/path")]
        public IActionResult EchoPath([FromRoute]string msg)
        {
            return Ok(msg);
        }

        [HttpPost("echo/header")]
        public IActionResult EchoHeader([FromHeader(Name = "Message")]string msg)
        {
            return Ok(msg);
        }

        [HttpPost("echo/body/obj/xml")]
        public async Task<IActionResult> EchoObjXml()
        {
            var reader = new StreamReader(Request.Body);
            var strContent = await reader.ReadToEndAsync();

            var ser = new XmlSerializer(typeof(TestModel));
            using var strReader = new StringReader(strContent);
            var rdr = XmlReader.Create(strReader, new XmlReaderSettings
            {
                IgnoreProcessingInstructions = true
            });
            var model = (TestModel)ser.Deserialize(rdr);

            return Ok(model.TestValue);
        }

        [HttpPost("echo/body/obj/json")]
        public IActionResult EchoObjJson([FromBody]TestModel model)
        {
            return Ok(model.TestValue);
        }

        [HttpPost("echo/body/form")]
        public IActionResult EchoForm([FromForm]TestModel model)
        {
            return Ok(model.TestValue);
        }

        [HttpPost("echo/body/text")]
        public async Task<IActionResult> EchoForm()
        {
            var rdr = new StreamReader(Request.Body);
            return Ok(await rdr.ReadToEndAsync());
        }

        [HttpPost("echo/body/bin")]
        public async Task<IActionResult> EchoBin()
        {
            var rdr = new StreamReader(Request.Body, Encoding.UTF8);
            return Ok(await rdr.ReadToEndAsync());
        }
    }
}