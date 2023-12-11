using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using TrainPro.Data;
using TrainPro.Models;
using TrainPro.Models.Dto;
using TrainPro.Services;
using TrainPro.Utility;
using static System.Net.Mime.MediaTypeNames;

namespace TrainPro.Controllers
{
    [Route("api/MenuItem")]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private ApiResponse _response;
        private readonly IBlobService _blobService;

        public MenuItemController(ApplicationDbContext db, IBlobService blobService)
        {
            _db = db;
            _response = new ApiResponse();
            _blobService = blobService;
        }
        [HttpGet]
        public async Task<IActionResult> GetMenuItems()
        {
            _response.Result = _db.MenuItems;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        [HttpGet("{id:int}",Name = "GetMenuItem")]
        public async Task<IActionResult> GetMenuItem(int id)
        {
            if (id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSsuccess = false;
                return BadRequest(_response);
            }
            MenuItem menuItem = _db.MenuItems.FirstOrDefault(x => x.Id == id);
            if (menuItem == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSsuccess = false;
                return NotFound(_response);
            }
            _response.Result = menuItem;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateMenuItem([FromForm]MenuItemCreateDTO menuItemCreateDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (menuItemCreateDto.File == null || menuItemCreateDto.File.Length == 0)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSsuccess = false;
                        return BadRequest();
                    }
                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(menuItemCreateDto.File.FileName)}";
                    MenuItem menuItemToCreate = new()
                    {
                        Name = menuItemCreateDto.Name,
                        Price = menuItemCreateDto.Price,
                        Category = menuItemCreateDto.Category,
                        SpecialTag = menuItemCreateDto.SpecialTag,
                        Description = menuItemCreateDto.Description,
                        Image = await _blobService.UploadBlob(fileName, SD.SD_STORAGE_Container, menuItemCreateDto.File)
                    };
                    _db.MenuItems.Add(menuItemToCreate);
                    _db.SaveChanges();
                    _response.Result = menuItemToCreate;
                    _response.StatusCode = HttpStatusCode.Created;
                    return CreatedAtRoute("GetMenuItem", new { Id = menuItemToCreate.Id }, _response);
                }
                else
                {
                _response.IsSsuccess = false;
                }
            }
            catch (Exception ex)
            {

                _response.IsSsuccess = false;
                _response.ErrorMessages 
                    = new List<string> { ex.Message };
            }
            return _response;
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse>> UpdateMenuItem(int id, [FromForm] MenuItemUpdateDTO menuItemUpdateDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (menuItemUpdateDto == null || id != menuItemUpdateDto.Id)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSsuccess = false;
                        return BadRequest();
                    }
                    MenuItem menuItemFromDb = await _db.MenuItems.FindAsync(id);
                    if(menuItemFromDb== null)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSsuccess = false;
                        return BadRequest();
                    }

                    menuItemFromDb.Name = menuItemUpdateDto.Name;
                    menuItemFromDb.Price = menuItemUpdateDto.Price;
                    menuItemFromDb.Category = menuItemUpdateDto.Category;
                    menuItemFromDb.Description = menuItemUpdateDto.Description;
                    menuItemFromDb.SpecialTag = menuItemUpdateDto.SpecialTag;

                    if(menuItemUpdateDto.File!=null && menuItemUpdateDto.File.Length > 0)
                    {
                        string fileName = $"{Guid.NewGuid()}{Path.GetExtension(menuItemUpdateDto.File.FileName)}";
                        await _blobService.DeleteBlob(menuItemFromDb.Image.Split('/').Last(),SD.SD_STORAGE_Container);
                        menuItemFromDb.Image = await _blobService.UploadBlob(fileName, SD.SD_STORAGE_Container, menuItemUpdateDto.File);
                    };

                    
                    _db.MenuItems.Update(menuItemFromDb);
                    _db.SaveChanges();
                    _response.StatusCode = HttpStatusCode.NoContent;
                    return Ok(_response);
                }
                else
                {
                    _response.IsSsuccess = false;
                }
            }
            catch (Exception ex)
            {

                _response.IsSsuccess = false;
                _response.ErrorMessages
                    = new List<string> { ex.Message };
            }
            return _response;
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ApiResponse>> DeleteMenuItem(int id)
        {
            try
            {
                    if (id == 0)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSsuccess = false;
                        return BadRequest();
                    }
                    MenuItem menuItemFromDb = await _db.MenuItems.FindAsync(id);
                    if (menuItemFromDb == null)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSsuccess = false;
                        return BadRequest();
                    }
                    await _blobService.DeleteBlob(menuItemFromDb.Image.Split('/').Last(), SD.SD_STORAGE_Container);
                    int milliseconds = 2000;
                    Thread.Sleep(milliseconds);
                    
                    _db.MenuItems.Remove(menuItemFromDb);
                    _db.SaveChanges();
                    _response.StatusCode = HttpStatusCode.NoContent;
                    return Ok(_response);
               
            }
            catch (Exception ex)
            {

                _response.IsSsuccess = false;
                _response.ErrorMessages
                    = new List<string> { ex.Message };
            }
            return _response;
        }
    }
}
