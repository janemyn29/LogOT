
using System.Net;
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.ConfigWifis.Commands.CreateConfigWifi;
using mentor_v1.Application.ConfigWifis.Commands.DeleteConfigWifi;
using mentor_v1.Application.ConfigWifis.Commands.UpdateConfigWifi;
using mentor_v1.Application.ConfigWifis.Queries.GetList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebUI.Models;

namespace WebUI.Controllers;
[Authorize(Roles ="Manager")]
[ApiController]
[Route("[controller]/[action]")]
public class ConfigWifiController : ApiControllerBase
{

    #region Get name adn addres network đang kết nối
    [HttpGet]
    public async Task<IActionResult> GetAddressConnecting()
    {
        var network = await Mediator.Send(new GetWifiConnectRequest() { });
        string ip = GetIPWifi();
        if (ip == null)
        {
            return BadRequest("Vui lòng kiểm tra lại kết nối Wifi chấm công để thực hiện chấm công!");
        }
        var IpWifi = JsonConvert.DeserializeObject<IpModel>(ip);

        WifiConnect wifiConnect = new WifiConnect();
        wifiConnect.NameWifi = network.NameWifi;
        wifiConnect.IPv6Adderss = IpWifi.ipString;

        return Ok(new { 
            Status = Ok().StatusCode,
            Message = "Lấy dự liệu thành công.",
            Result = wifiConnect
        });
    }
    #endregion


    [NonAction]
    public string GetIPWifi()
    {
        //lấy là Ktr IP wifi
        var urlExteranlAPI = string.Format("https://api-bdc.net/data/client-info");
        WebRequest request = WebRequest.Create(urlExteranlAPI);
        request.Method = "GET";
        HttpWebResponse response = null;
        response = (HttpWebResponse)request.GetResponse();

        string ip = null;
        using (Stream stream = response.GetResponseStream())
        {
            StreamReader sr = new StreamReader(stream);
            ip = sr.ReadToEnd();
            sr.Close();
        }
        if (ip == null)
        {

            return null;
        }
        return ip;
    }



    #region Get list network
    [HttpGet("page")]
    public async Task<IActionResult> GetListNetWork(int page)
    {
        var network = await Mediator.Send(new GetListConfigWifiRequest() {Page = page, Size = 10 });
        return Ok(new
        {
            Status = Ok().StatusCode,
            Message = "Lấy dự liệu thành công.",
            Result = network
        });
    }
    #endregion

    #region Create network
    [HttpPost]
    public async Task<IActionResult> CreateNetwork(string name, string ip)
    {
        try {
            var network = await Mediator.Send(new CreateConfigWifiCommand { NameWifi = name, WifiIPv4 = ip });
            return Ok(new { 
                Status = Ok().StatusCode,
                Message = "Tạo thành công."
            });
        } catch (Exception ex) {
            return BadRequest(new
            {
                Status = BadRequest().StatusCode,
                Message = ex.Message
            });
        }
    }
    #endregion

    #region Update network
    [HttpPut]
    public async Task<IActionResult> UpdateNetwork(Guid id, string name, string ip)
    {
        try {
            var network = await Mediator.Send(new UpdateConfigWifiCommand { Id = id, NameWifi = name, WifiIPv4 = ip });
            return Ok(new
            {
                Status = Ok().StatusCode,
                Message = "Cập nhật thành công."
            });
        }
        catch (NotFoundException ex) {
            return NotFound(new { 
                Status = NotFound().StatusCode,
                Message = ex.Message
            });
                
        }
        catch (Exception ex) {
            return BadRequest(new
            {
                Status = BadRequest().StatusCode,
                Message = "Cập nhật thất bại."
            });
        }
    }
    #endregion

    #region Delete Allowance
    [HttpDelete("{ip}")]
    public async Task<IActionResult> DeleteNetwork(string ip)
    {
        try
        {
            var item = await Mediator.Send(new DeleteConfigWifiCommand { IPv4 = ip });
            return Ok(new
            {
                Status = Ok().StatusCode,
                Message = "Xoá thành công.",
            });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new
            {
                Status = NotFound().StatusCode,
                Message = ex.Message
            });
        }
    }
    #endregion
}
