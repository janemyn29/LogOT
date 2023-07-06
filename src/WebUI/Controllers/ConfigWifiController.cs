
using mentor_v1.Application.Common.Exceptions;
using mentor_v1.Application.ConfigWifis.Commands.CreateConfigWifi;
using mentor_v1.Application.ConfigWifis.Commands.DeleteConfigWifi;
using mentor_v1.Application.ConfigWifis.Commands.UpdateConfigWifi;
using mentor_v1.Application.ConfigWifis.Queries.GetList;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ConfigWifiController : ApiControllerBase
{

    #region Get name adn addres network đang kết nối
    [HttpGet]
    public async Task<IActionResult> GetAddressConnecting()
    {
        var network = await Mediator.Send(new GetWifiConnectRequest() { });
        return Ok(new { 
            Status = Ok().StatusCode,
            Message = "Lấy dự liệu thành công.",
            Result = network
        });
    }
    #endregion

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
    public async Task<IActionResult> CreateNetwork([FromForm]string name, string ip)
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
    public async Task<IActionResult> UpdateNetwork([FromForm]Guid id, string name, string ip)
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
