using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApplication.Core.Interface;
using WebApplication.Core.Model;
using WebApplication.Models;


namespace WebApplication.Controllers
{
    public class BooksController : Controller
    {

        IBaseRepository<Books> _booksRepository;
        public BooksController(IBaseRepository<Books> booksRepository)
        {
            _booksRepository = booksRepository;

        }

        public IActionResult Index()
        {
            return View();
        }       
    

        public async Task<IActionResult> LoadData()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                // Skiping number of Rows count  
                var start = Request.Form["start"].FirstOrDefault();
                // Paging Length 10,20  
                var length = Request.Form["length"].FirstOrDefault();
                // Sort Column Name  
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                // Sort Column Direction ( asc ,desc)  
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                // Search Value from (Search box)  
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                //Paging Size (10,20,50,100)  
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // Getting all Customer data  
                var customerData = await _booksRepository.GetAllAsync();


                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    customerData = customerData.OrderBy(c => c.Title);
                }
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customerData = customerData.Where(m => m.Title.ToLower() == searchValue.ToLower() 
                    || Convert.ToString(m.Id).ToLower() == searchValue.ToLower());
                }

                //total number of rows count   
                recordsTotal = customerData.Count();
                //Paging   
                var data = customerData.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrio un error", ex);
            }

        }




        public async Task<IActionResult> GetDatabyId(int id)
        {

            try
            {
                ViewBag.list = await  _booksRepository.GetByIdAsync(id);
            
                if (ViewBag.list == null)
                {
                    return RedirectToAction("Index");

                }
        

            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error", ex);
            }

            return View();

             

        }



        [HttpPost]
        public async Task<IActionResult> AddBooks(Books objeto) 
        {
            if (objeto  != null)
            {
                await _booksRepository.AddAsync(objeto);
                return RedirectToAction("Index");

            }

            return Ok();
        }



        public async Task<IActionResult> Delete(int id)
        {

            try
            {
                if (id != 0)
                {
                    var filter = await _booksRepository.DeleteAsync(id);
                    TempData["success"] = "This row was Delete!";
                    ViewBag.Message = "Plan Already Exists";

                    return RedirectToAction("Index");

                }

            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error", ex);
            }

            return View();



        }

        public async Task<IActionResult>Update(int id)
        {
            var list=  await _booksRepository.GetByIdAsync(id);
            if (list!=null)
            {


                foreach (Books books in list)
                    return View(books);
            }

            return RedirectToAction("Index");
        }

        [HttpPut]
        public async Task<IActionResult> Update(Books books)
        {
            try
            {
                await _booksRepository.Updatesync(books);

            }
            catch (Exception)
            {

                throw;
            }
            

            return RedirectToAction("Index");
        }

    }
}
